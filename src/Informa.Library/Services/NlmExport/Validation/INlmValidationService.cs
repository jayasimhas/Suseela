using System.IO;

namespace Informa.Library.Services.NlmExport.Validation
{
    public interface INlmValidationService
    {
        ValidationResult ValidateXml(Stream sourceStream);
    }
}
