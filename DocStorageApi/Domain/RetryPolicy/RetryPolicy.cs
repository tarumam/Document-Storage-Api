using Microsoft.Data.SqlClient;
using Npgsql;
using Polly;
using Polly.Retry;

namespace DocStorageApi.Domain.RetryPolicy
{
    public class RetryDbPolicy : IRetryPolicy
    {
        private const int RetryCount = 3;
        private const int WaitBetweenRetriesInMilliseconds = 1000;
        private readonly int[] _databaseExceptions = new[] { 1 };

        private readonly Policy _retryPolicyAsync;
        private readonly Policy _retryPolicy;

        public RetryDbPolicy()
        {
            _retryPolicyAsync = Policy
            .Handle<SqlException>(exception => _databaseExceptions.Contains(exception.Number))
            .WaitAndRetryAsync(
                retryCount: RetryCount,
                sleepDurationProvider: attempt => TimeSpan.FromMilliseconds(WaitBetweenRetriesInMilliseconds)
            );

            _retryPolicy = Policy
                .Handle<SqlException>(exception => _databaseExceptions.Contains(exception.Number))
                .WaitAndRetry(
                    retryCount: RetryCount,
                    sleepDurationProvider: attempt => TimeSpan.FromMilliseconds(WaitBetweenRetriesInMilliseconds)
                );
        }

        public string PolicyKey => throw new NotImplementedException();

        public void Execute(Action operation)
        {
            _retryPolicy.Execute(operation.Invoke);
        }

        public TResult Execute<TResult>(Func<TResult> operation)
        {
            return _retryPolicy.Execute(() => operation.Invoke());
        }

        public async Task Execute(Func<Task> operation, CancellationToken cancellationToken)
        {
            await _retryPolicyAsync.ExecuteAsync(operation.Invoke);
        }

        public async Task<TResult> Execute<TResult>(Func<Task<TResult>> operation, CancellationToken cancellationToken)
        {
            return await _retryPolicyAsync.ExecuteAsync(operation.Invoke);
        }
    }
}
