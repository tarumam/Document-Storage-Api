using DocStorageApi.DTO.Request;
using DocStorageApi.DTO.Response;

namespace DocStorageApi.Services.Interfaces
{
    public interface IS3Service
    {
        Task<FileUploadResponse> UploadFileAsync(FileUploadRequest fileUploadInfo);

        Task<MemoryStream> DownloadWithMetadata(string filename);
    }
}
