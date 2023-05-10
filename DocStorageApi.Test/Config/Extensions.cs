using System.Net.Http.Headers;

namespace DocStorageApi.Integration.Tests.Config
{
    public static class Extensions
    {
        public static void SetTokenToRequestHeader(this HttpClient client, string token)
        {
            client.SetJsonMediaType();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public static void SetJsonMediaType(this HttpClient client)
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
