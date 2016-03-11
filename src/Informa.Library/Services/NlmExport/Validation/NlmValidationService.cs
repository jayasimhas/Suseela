using System;
using System.IO;
using System.Text;
using System.Xml;
using Informa.Library.Services.NlmExport.Validation.Config;
using Informa.Library.Services.NlmExport.Validation.XmlResolver;
using Jabberwocky.Glass.Autofac.Attributes;
using log4net;

namespace Informa.Library.Services.NlmExport.Validation
{
    [AutowireService]
    public class NlmValidationService : INlmValidationService
    {
        private readonly NlmValidationConfiguration _config;
        private readonly ILog _log;

        public NlmValidationService(NlmValidationConfiguration config, ILog log)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));
            if (log == null) throw new ArgumentNullException(nameof(log));
            _config = config;
            _log = log;
        }

        public bool ValidateXml(Stream sourceStream)
        {
            StringBuilder errors = new StringBuilder();

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
                        errors.AppendLine(args.Message);
                };

                using (var reader = XmlReader.Create(sourceStream, settings, path))
                {
                    while (reader.Read())
                    {
                    }
                }
                
                var errorString = errors.ToString();
                if (!string.IsNullOrEmpty(errorString))
                {
                    _log.Error("Failed NLM validation: " + errorString);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _log.Error("Failed NLM validation", ex);
                return false;
            }

            return true;
        }
    }
}
