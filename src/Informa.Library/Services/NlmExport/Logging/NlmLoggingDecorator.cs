using System;
using System.IO;
using Glass.Mapper.Sc;
using Informa.Library.Mail;
using Informa.Library.User;
using Informa.Library.Utilities.References;
using Informa.Library.Utilities.Settings;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Attributes;
using log4net;
using Newtonsoft.Json;

namespace Informa.Library.Services.NlmExport.Logging
{
    public class NlmLoggingDecorator : INlmExportService
    {
        private readonly INlmExportService _innerService;
        private readonly IDependencies _;
        private readonly ILog _logger;

        protected IError_Distribution_List EmailDistributionList
            => _.SitecoreService.GetItem<IError_Distribution_List>(_.ItemReferences.NlmErrorDistributionList);

        public NlmLoggingDecorator(INlmExportService innerService, ILog logger, IDependencies _)
        {
            if (innerService == null) throw new ArgumentNullException(nameof(innerService));
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            _innerService = innerService;
            _logger = logger;
            this._ = _;
        }

        public Stream GenerateNlm(ArticleItem article, PublicationType? type = null)
        {
            return _innerService.GenerateNlm(article, type);
        }

        public ExportResult ExportNlm(ArticleItem article, ExportType exportType, PublicationType? type = null)
        {
            // Capture some state
            var username = _.UserContext.User?.Name ?? "Unknown User";
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

            // Log & report any export errors
            if (!result.ExportSuccessful)
            {
                var errorMessage = string.Empty;
                if (result.Exception != null)
                {
                    errorMessage = "Error while exporting NLM: \n" + JsonConvert.SerializeObject(logMessage);
                }
                // Log & report on any validation errors
                else if (!result.ValidationResult)
                {
                    errorMessage = "NLM Export failed on validation. \n" + JsonConvert.SerializeObject(logMessage);
                }

                _logger.Error(errorMessage, result.Exception);
                SendMail(errorMessage);
            }

            return result;
        }

        private void SendMail(string errorMessage)
        {
            var mail = new Email
            {
                IsBodyHtml = false,
                To = GetEmailRecipients(),
                From = _.SiteSettings.MailFromAddress,
                Subject = "NLM Export Error",
                Body = errorMessage
            };

            _.EmailService.Send(mail);
        }

        public bool DeleteNlm(ArticleItem article)
        {
            return _innerService.DeleteNlm(article);
        }

        protected virtual string GetEmailRecipients()
        {
            return EmailDistributionList?.NLM_Error_Distribution_List ?? string.Empty;
        }

        [AutowireService(true)]
        public interface IDependencies
        {
            IItemReferences ItemReferences { get; }
            ISiteSettings SiteSettings { get; }
            ISitecoreService SitecoreService { get; }
            ISitecoreUserContext UserContext { get; }
            IEmailSender EmailService { get; }
        }
    }
}
