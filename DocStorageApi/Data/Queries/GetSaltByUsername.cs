using System.ComponentModel.DataAnnotations;

namespace DocStorageApi.Data.Queries
{
    /// <summary>
    /// Get salt value by username
    /// </summary>
    /// <param name="Username">Username</param>
    public record GetSaltByUsername([EmailAddress][Required] string Username) : IBaseQuery
    {
        public string Script => @"select salt from users u where u.name = @Username";

        public object Param => new { Username };
    };
}