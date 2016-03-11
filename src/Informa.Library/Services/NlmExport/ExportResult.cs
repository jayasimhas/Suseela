using System;
using Informa.Library.Services.NlmExport.Validation;

namespace Informa.Library.Services.NlmExport
{
    public class ExportResult
    {
        public bool ExportSuccessful { get; set; }
        public ValidationResult ValidationResult { get; set; } 
        public Exception Exception { get; set; }
    }
}
