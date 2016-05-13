using System;
using System.Linq;
using Informa.Library.Logging;
using Informa.Library.Utilities.Extensions;
using Informa.Library.Utilities.Settings;
using Informa.Library.Wrappers;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Emails;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Autofac.Attributes;
using Jabberwocky.Glass.Models;
using Sitecore.Mvc.Extensions;

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
            ISiteSettings SiteSettings { get; }
            IHttpContextProvider HttpContextProvider { get; }
            IWebClientWrapper WebClientWrapper { get; }
            IExactTargetWrapper ExactTargetWrapper { get; }
            ISitecoreSecurityWrapper SitecoreSecurityWrapper { get; }
            ISitecoreServiceMaster SitecoreServiceMaster { get; }
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
                    etEmail.InvokeAction(
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
            var url = GetEmailUrl(emailItem);
            return _dependencies.WebClientWrapper.DownloadString(url);
        }

        public string GetEmailUrl(IGlassBase glassItem)
        {
            var currentSiteItem = glassItem.GetAncestors<IGlassBase>()
                .FirstOrDefault(item => item._TemplateId == ISite_RootConstants.TemplateId.ToGuid());

            if (currentSiteItem == null)
            {
                _dependencies.LogWrapper.SitecoreWarn("Email not pushed to ExactTarget.  Could not find site root node.");
                return null;
            }

            var homeItem =
                currentSiteItem._ChildrenWithInferType.FirstOrDefault(
                    item => item._TemplateId == IHome_PageConstants.TemplateId.ToGuid());

            if (homeItem == null)
            {
                _dependencies.LogWrapper.SitecoreWarn("Email not pushed to ExactTarget.  Could not find site home node.");
                return null;
            }

            var currentSite = _dependencies.SiteSettings.GetSiteInfoList()
                .FirstOrDefault(site => site.RootPath == currentSiteItem._Path);

            if (currentSite == null)
            {
                _dependencies.LogWrapper.SitecoreWarn("Email not pushed to ExactTarget.  Could not find current site configuration.");
                return null;
            }

            var scheme = _dependencies.HttpContextProvider.RequestUrl?.Scheme + "://"; //This assume CD and CM have the same Scheme

            var url = scheme + currentSite.HostName +
                      glassItem._Path.Remove(0, homeItem._Path.Length);

            return url;
        }
    }
}