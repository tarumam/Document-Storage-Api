using Dapper;
using DocStorageApi.Data;
using DocStorageApi.Data.Queries;
using DocStorageApi.Domain.Repository.Interfaces;
using DocStorageApi.DTO.Response;

namespace DocStorageApi.Domain.Repository
{
    public class UserAccessGroupRepository : BaseRepository<UserAccessGroupRepository>, IUserAccessGroupRepository
    {
        private readonly DbSession _dbSession;
        public UserAccessGroupRepository(DbSession session, ILogger<UserAccessGroupRepository> logger) : base(session, logger)
        {
            _dbSession = session;
        }

        public async Task<IEnumerable<UserAccessGroupsResponse>> GetUserAccessGroups(Guid? userId)
        {
            var query = new UserAccessGroupsQuery(userId);
            var result = await _dbSession.Connection.QueryAsync<UserAccessGroupsResponse, UserAccessGroupResponse, UserAccessGroupsResponse>(query.Script,
               (user, accessGroup) =>
               {
                   user.AccessGroups.Add(accessGroup);
                   return user;
               },
                splitOn: "AccessGroupId",
                param: query.Param
            );

            return result;
        }
    }
}
