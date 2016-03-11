using System.IO;

namespace Informa.Library.Services.NlmExport.Validation
{
    public interface INlmValidationService
    {
        bool ValidateXml(Stream sourceStream);
    }
}
