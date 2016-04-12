using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using Informa.Library.Services.NlmExport.Validation.Config;
using Informa.Library.Services.NlmExport.Validation.Error;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.Services.NlmExport.Validation
{
    [AutowireService]
    public class NlmValidationService : INlmValidationService
    {
        private readonly NlmValidationConfiguration _config;

        public NlmValidationService(NlmValidationConfiguration config)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));
            _config = config;
        }

        public ValidationResult ValidateXml(Stream sourceStream)
        {
            var errors = new List<ValidationError>();
            var capturedElement = string.Empty;

            try
            {
                var path = Path.GetFullPath(_config.DtdPath);

                var settings = new XmlReaderSettings
                {
                    ValidationType = ValidationType.DTD,
                    DtdProcessing = DtdProcessing.Parse,
                    XmlResolver = new XmlUrlResolver()
                };
                settings.ValidationEventHandler += (sender, args) =>
                {
                    if (!args.Message.Contains("The parameter entity replacement text"))
                    {
                        errors.Add(new ValidationError
                        {
                            Message = args.Message,
                            LineNumber = args.Exception?.LineNumber.ToString(),
                            LinePosition = args.Exception?.LinePosition.ToString(),
                            LastCapturedElement = capturedElement,
                            Exception = args.Exception
                        });
                    }
                };

                using (var reader = XmlReader.Create(sourceStream, settings, path))
                {
                    while (reader.Read())
                    {
                        capturedElement = $"Name: '{reader.Name}', Type: '{reader.NodeType}', Value: '{reader.Value}'";
                    }
                }

                return Result(errors);
            }
            catch (Exception ex)
            {
                return Result(errors, ex);
            }
        }

        private static ValidationResult Result(IList<ValidationError> errors = null, Exception ex = null)
        {
            return new ValidationResult
            {
                Errors = errors ?? new List<ValidationError>(),
                ValidationSuccessful = errors == null || !errors.Any(),
                Exception = ex
            };
        }
    }
}
