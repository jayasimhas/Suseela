using System;
using Informa.Library.Logging;
using Informa.Library.Utilities.Extensions;
using Informa.Library.Wrappers;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Emails;
using Jabberwocky.Autofac.Attributes;

namespace Informa.Library.Mail.ExactTarget
{
    public interface IExactTargetClient
    {
        PushEmailResponse PushEmail(IExactTarget_Email emailItem);
    }

    [AutowireService]
    public class ExactTargetClient : IExactTargetClient
    {
        private readonly IDependencies _dependencies;

        [AutowireService(IsAggregateService = true)]
        public interface IDependencies
        {
            ILogWrapper LogWrapper { get; }
            IWebClientWrapper WebClientWrapper { get; }
            IExactTargetWrapper ExactTargetWrapper { get; }
            ISitecoreSecurityWrapper SitecoreSecurityWrapper { get; }
            ISitecoreServiceMaster SitecoreServiceMaster { get; }
            ISitecoreUrlWrapper SitecoreUrlWrapper { get; }
            IPremailerWrapper Premailer { get; }
        }

        public ExactTargetClient(IDependencies dependencies)
        {
            _dependencies = dependencies;
        }

        private static bool IsEmailNewToExactTarget(IExactTarget_Email emailItem) => emailItem.Exact_Target_External_Key == 0;

        private static bool IsNullOrEmptyEmailItem(IExactTarget_Email emailItem)
            => emailItem == null || emailItem._Id == Guid.Empty;

        public PushEmailResponse PushEmail(IExactTarget_Email emailItem)
        {
            if (IsNullOrEmptyEmailItem(emailItem))
            { return Respond(false, "Email not pushed to ExactTarget. Email item was null."); }

            _dependencies.LogWrapper.SitecoreInfo($"Email push to ExactTarget starting.  Email item Sitecore id: {emailItem._Id.ToString("B")}");

            FuelSDK.ET_Email etEmail;
            try
            {
                etEmail = PopulateEtModel(emailItem);
                if(string.IsNullOrWhiteSpace(etEmail.HTMLBody)) { throw new Exception("HTML body was empty."); }
            } catch (Exception ex)
            {
                return Respond(false, $"Failed to populate email to send to ExactTarget.  Error: {ex.Message}");
            }

            var response = IsEmailNewToExactTarget(emailItem)
                ? _dependencies.ExactTargetWrapper.CreateEmail(etEmail)
                : _dependencies.ExactTargetWrapper.UpdateEmail(etEmail);

            if (response.Success && IsEmailNewToExactTarget(emailItem)) { UpdateSitecoreWithEmailId(emailItem, response); }
            
            if (response.Success)
            {
                return Respond(true,
                    $"Email push to ExactTarget finished. Email item Sitecore id: {emailItem._Id.ToString("B")}, ET id: {emailItem.Exact_Target_External_Key}, "
                    + $"Result message: {response.Message}");
            }
            else
            {
                return Respond(false,
                    $"Email push to ExactTarget FAILED. Email item Sitecore id: {emailItem._Id.ToString("B")}, ET id: {emailItem.Exact_Target_External_Key}, "
                    + $"Result message: {response.Message}");
            }

        }

        private PushEmailResponse Respond(bool success, string message)
        {
            var response = new PushEmailResponse {Success = success, Message = message};
            if (success)
            {
                _dependencies.LogWrapper.SitecoreInfo(message);
            }
            else
            {

                _dependencies.LogWrapper.SitecoreWarn(message);
            }
            return response;
        }

        private void UpdateSitecoreWithEmailId(IExactTarget_Email etEmail, ExactTargetResponse response) =>
            _dependencies.SitecoreSecurityWrapper.WithSecurityDisabled(
                () => _dependencies.SitecoreServiceMaster.Save(
                    etEmail.Alter(
                        email => email.Exact_Target_External_Key = response.ExactTargetEmailId)));

        public FuelSDK.ET_Email PopulateEtModel(IExactTarget_Email emailItem)
        {
            return new FuelSDK.ET_Email()
            {
                ID = emailItem.Exact_Target_External_Key,
                Subject = emailItem.Subject,
                Name = emailItem._Name,
                HTMLBody = GetEmailHtml(emailItem),
                TextBody = emailItem.Text_Body,
                CharacterSet = emailItem.Character_Set,
                CategoryID = emailItem.Category_Id,
                EmailType = "HTML",
                IsHTMLPaste = true
            };
        }

        public string GetEmailHtml(IExactTarget_Email emailItem)
        {
            var url = _dependencies.SitecoreUrlWrapper.GetItemUrl(emailItem);
            var htmlResponse = _dependencies.WebClientWrapper.DownloadString(url);
            var inlineHtml = _dependencies.Premailer.InlineCss(htmlResponse);
            return inlineHtml;
        }

    }

    public class PushEmailResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}