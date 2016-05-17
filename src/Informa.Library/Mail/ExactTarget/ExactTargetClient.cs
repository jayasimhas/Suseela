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
        void PushEmail(IExactTarget_Email emailItem);
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
        }

        public ExactTargetClient(IDependencies dependencies)
        {
            _dependencies = dependencies;
        }

        private static bool IsEmailNewToExactTarget(IExactTarget_Email emailItem) => emailItem.Exact_Target_External_Key == 0;

        private static bool IsNullOrEmptyEmailItem(IExactTarget_Email emailItem)
            => emailItem == null || emailItem._Id == Guid.Empty;

        public void PushEmail(IExactTarget_Email emailItem)
        {
            if (IsNullOrEmptyEmailItem(emailItem))
            {
                _dependencies.LogWrapper.SitecoreWarn("Email not pushed to ExactTarget.  Email item was null.");
                return;
            }

            _dependencies.LogWrapper.SitecoreInfo($"Email push to ExactTarget starting.  Email item Sitecore id: {emailItem._Id.ToString("B")}");

            var etEmail = PopulateEtModel(emailItem);

            var response = IsEmailNewToExactTarget(emailItem)
                ? _dependencies.ExactTargetWrapper.CreateEmail(etEmail)
                : _dependencies.ExactTargetWrapper.UpdateEmail(etEmail);

            if (response.Success && IsEmailNewToExactTarget(emailItem)) { UpdateSitecoreWithEmailId(emailItem, response); }
            
            if (response.Success)
            {
                _dependencies.LogWrapper.SitecoreInfo(
                    $"Email push to ExactTarget finished. Email item Sitecore id: {emailItem._Id.ToString("B")}, ET id: {emailItem.Exact_Target_External_Key}, "
                    + $"Result message: {response.Message}");
            }
            else
            {
                _dependencies.LogWrapper.SitecoreWarn(
                    $"Email push to ExactTarget FAILED. Email item Sitecore id: {emailItem._Id.ToString("B")}, ET id: {emailItem.Exact_Target_External_Key}, "
                    + $"Result message: {response.Message}");
            }

        }

        private void UpdateSitecoreWithEmailId(IExactTarget_Email etEmail, ExactTargetResponse response) =>
            _dependencies.SitecoreSecurityWrapper.SecurityDisabledAction(
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
                CategoryIDSpecified = true,
                EmailType = "HTML",
                IsHTMLPaste = true
            };
        }

        public string GetEmailHtml(IExactTarget_Email emailItem)
        {
            var url = _dependencies.SitecoreUrlWrapper.GetItemUrl(emailItem);
            return _dependencies.WebClientWrapper.DownloadString(url);
        }

    }
}