using System;
using System.Collections.Generic;
using Informa.Library.Services.NlmExport.Validation.Error;

namespace Informa.Library.Services.NlmExport.Validation
{
    public class ValidationResult
    {
        public bool ValidationSuccessful { get; set; }
        
        public IList<ValidationError> Errors { get; set; }

        public Exception Exception { get; set; }

        public static implicit operator bool(ValidationResult result)
        {
            return result.ValidationSuccessful;
        }
    }
}
