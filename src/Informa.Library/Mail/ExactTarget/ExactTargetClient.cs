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

        [AutowireService(true)]
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

        public void PushEmail(IExactTarget_Email emailItem)
        {
            if (emailItem == null || emailItem._Id == Guid.Empty)
            {
                _dependencies.LogWrapper.SitecoreWarn("Email not pushed to ExactTarget.  Email item was null.");
                return;
            }

            _dependencies.LogWrapper.SitecoreInfo($"Email push to ExactTarget starting.  Email item Sitecore id: {emailItem._Id.ToString("B")}");

            var etEmail = PopulateEtModel(emailItem);

            string status;
            if (string.IsNullOrEmpty(emailItem.ET_Email_Id))
            {
                var result = _dependencies.ExactTargetWrapper.CreateEmail(etEmail, out status);

                emailItem.ET_Email_Id = result.NewObjectID;
                _dependencies.SitecoreSecurityWrapper.SecurityDisabledAction(
                    () => _dependencies.SitecoreServiceMaster.Save(emailItem));
            }
            else
            {
                var result = _dependencies.ExactTargetWrapper.UpdateEmail(etEmail, out status);
            }

            _dependencies.LogWrapper.SitecoreInfo(
                $"Email push to ExactTarget finished. Email item Sitecore id: {emailItem._Id.ToString("B")}, ET id: {emailItem.ET_Email_Id}");

        }

        public FuelSDK.Email PopulateEtModel(IExactTarget_Email emailItem)
        {
            return new FuelSDK.Email
            {
                Subject = emailItem.Subject,
                Name = emailItem._Name,
                HTMLBody = GetEmailHtml(emailItem),
                TextBody = emailItem.Text_Body,
                CharacterSet = emailItem.Character_Set,
                CategoryID = emailItem.Category_Id,
                CategoryIDSpecified = true
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