namespace DocStorageApi.DTO.Response
{
    public record FileUploadResponse(bool Status, string Key, DateTime? postedDate, int? statusCode = null)
    {
    }
}
