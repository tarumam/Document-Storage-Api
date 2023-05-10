using DocStorageApi.Data.Commands;
using DocStorageApi.Domain.Repository.Interfaces;
using DocStorageApi.DTO.Request;
using DocStorageApi.DTO.Response;
using DocStorageApi.Services.Interfaces;

namespace DocStorageApi.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IDocumentRepository _docRepository;
        private readonly ICurrentUserService _currentUserService;
        Guid _currentUserId;
        public DocumentService(IDocumentRepository docRepository, ICurrentUserService currentUserService)
        {
            _docRepository = docRepository;
            _currentUserId = currentUserService.GetUserId();
        }

        public async Task<bool> AddNewDocumentAsync(AddNewDocumentRequest newDocument)
        {
            var command = new InsertDocumentCommand(newDocument.FileKey, newDocument.Name, newDocument.Category, newDocument.Description, newDocument.PostedAt, true, _currentUserId);
            var result = await _docRepository.AddNewDocumentAsync(command);
            return result.Succeeded;
        }

        public async Task<bool> DisableDocumentAsync(Guid documentId)
        {
            var result = await _docRepository.DisableDocumentAsync(new DisableDocumentCommand(documentId));
            return result.Succeeded;
        }

        public async Task<IEnumerable<DocumentsResponse>> GetAllDocumentsAsync()
        {
            return await _docRepository.GetAllDocumentsAsync();
        }
    }
}
