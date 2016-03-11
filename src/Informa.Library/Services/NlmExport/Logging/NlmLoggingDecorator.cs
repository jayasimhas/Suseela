using System;
using System.IO;
using Informa.Library.User;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using log4net;
using Newtonsoft.Json;

namespace Informa.Library.Services.NlmExport.Logging
{
    public class NlmLoggingDecorator : INlmExportService
    {
        private readonly INlmExportService _innerService;
        private readonly ISitecoreUserContext _userContext;
        private readonly ILog _logger;

        public NlmLoggingDecorator(INlmExportService innerService, ILog logger, ISitecoreUserContext userContext)
        {
            if (innerService == null) throw new ArgumentNullException(nameof(innerService));
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            if (userContext == null) throw new ArgumentNullException(nameof(userContext));
            _innerService = innerService;
            _logger = logger;
            _userContext = userContext;
        }

        public Stream GenerateNlm(ArticleItem article, PublicationType? type = null)
        {
            return _innerService.GenerateNlm(article, type);
        }

        public ExportResult ExportNlm(ArticleItem article, ExportType exportType, PublicationType? type = null)
        {
            // Capture some state
            var username = _userContext.User?.Name ?? "Unknown User";
            var articlePath = article._Path;
            var articleId = article.Article_Number;
            var currentTime = DateTime.UtcNow;

            var result = _innerService.ExportNlm(article, exportType, type);

            var logMessage = new NlmLogMessage
            {
                ArticleId = articleId,
                ArticlePath = articlePath,
                Username = username,
                ExportTime = currentTime,
                ExportResult = result
            };

            // Log any export errors
            if (result.Exception != null)
            {
                var serializedMessage = JsonConvert.SerializeObject(logMessage);
                _logger.Error("Error while exporting NLM: \n" + serializedMessage, result.Exception);
            }
            // Log & report on any validation errors
            else if (!result.ValidationResult)
            {
                var serializedMessage = JsonConvert.SerializeObject(logMessage);
                _logger.Warn("NLM Export failed on validation. \n" + serializedMessage, result.Exception);
            }

            return result;
        }

        public bool DeleteNlm(ArticleItem article)
        {
            return _innerService.DeleteNlm(article);
        }
    }
}
