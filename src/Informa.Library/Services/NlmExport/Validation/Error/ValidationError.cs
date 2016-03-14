using System;

namespace Informa.Library.Services.NlmExport.Validation.Error
{
    public class ValidationError
    {
        public string Message { get; set; }
        public string LineNumber { get; set; }
        public string LinePosition { get; set; }
        public Exception Exception { get; set; }
        public string LastCapturedElement { get; set; }
    }
}
