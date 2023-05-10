namespace DocStorageApi.DTO.Response
{
    public class UserCredentialsResult
    {
        public Guid UserId { get; set; }

        public string TokenId { get; set; }

        public string Role { get; set; }
    }
}
