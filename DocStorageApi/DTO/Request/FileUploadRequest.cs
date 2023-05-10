using DocStorageApi.Utils;
using System.ComponentModel.DataAnnotations;

namespace DocStorageApi.DTO.Request
{
    public record FileUploadRequest([Required] IFormFile File,
        [Required][RegularExpression(RegexPatterns.OnlyASCIIChars, ErrorMessage = "{0}: Please avoid accents and special characters in this field.")] string Name,
        [RegularExpression(RegexPatterns.OnlyASCIIChars, ErrorMessage = "{0}: Please avoid accents and special characters in this field.")] string Description,
        [RegularExpression(RegexPatterns.OnlyASCIIChars, ErrorMessage = "{0}: Please avoid accents and special characters in this field.")] string Category);
}
