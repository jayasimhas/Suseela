using System.Linq;
using Glass.Mapper.Sc;
using Informa.Library.Globalization;
using Informa.Library.Mail.ExactTarget;
using Informa.Library.Navigation;
using Informa.Library.Services.Global;
using Informa.Library.Site;
using Informa.Library.Utilities.Extensions;
using Informa.Library.Utilities.References;
using Informa.Library.Wrappers;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Emails;
using Jabberwocky.Autofac.Attributes;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Sitecore.Data.Items;

namespace Informa.Web.ViewModels.Emails
{
    public class EmailLayoutViewModel : GlassViewModel<IExactTarget_Email>
    {
        private readonly IDependencies _dependencies;

        [AutowireService(IsAggregateService = true)]
        public interface IDependencies
        {
            ITextTranslator TextTranslator { get; }
            ISitecoreUrlWrapper SitecoreUrlWrapper { get; }
            ISiteRootContext SiteRootContext { get; }
            IItemNavigationTreeFactory ItemNavigationTreeFactory { get; }
            ICampaignQueryBuilder CampaignQueryBuilder { get; }
            IGlobalSitecoreService SitecoreService { get; }
        }

        public EmailLayoutViewModel(IDependencies dependencies)
        {
            _dependencies = dependencies;
        }

        private string _viewOnlineVersionPreLink;
        public string ViewOnlineVersionPreLink => _viewOnlineVersionPreLink ?? 
                                                (_viewOnlineVersionPreLink = _dependencies.TextTranslator?.Translate(DictionaryKeys.ViewOurOnlineVersionBeforeLink));

        private string _viewOnlineVersionLinkText;
        public string ViewOnlineVersionLinkText => _viewOnlineVersionLinkText ??
                                                (_viewOnlineVersionLinkText = _dependencies.TextTranslator?.Translate(DictionaryKeys.ViewOurOnlineVersionLinkText));

        private string _onlineVersionUrl;

        public string OnlineVersionUrl => _onlineVersionUrl ??
                                          (_onlineVersionUrl = _dependencies.CampaignQueryBuilder.AddCampaignQuery(
                                              _dependencies.SitecoreUrlWrapper.GetItemUrl(GlassModel)));

        private ISite_Root _siteRoot;
        public ISite_Root SiteRoot  {
            get
            {
                if (_siteRoot == null)
                    _siteRoot = _dependencies.SitecoreService.GetSiteRootAncestor(GlassModel._Id);
                
                return _siteRoot;
            }
        }

        public string EmailLogoUrl => SiteRoot.Email_Logo?.Src ?? string.Empty;

        public string RssLogoUrl => SiteRoot.RSS_Logo?.Src ?? string.Empty;

        public string LinkedInLogoUrl => SiteRoot.Linkedin_Logo?.Src ?? string.Empty;

        public string TwitterLogoUrl => SiteRoot.Twitter_Logo?.Src ?? string.Empty;

        private NavItemViewModel[] _headerNavigation;
        public NavItemViewModel[] HeaderNavigation
            => _headerNavigation ??
               (_headerNavigation =
                   _dependencies.ItemNavigationTreeFactory.Create(SiteRoot.Email_Header_Navigation)
                       .FirstOrDefault()?
                       .Children
                       .Select(nav => new NavItemViewModel
                       {
                           LinkUrl = _dependencies.CampaignQueryBuilder.AddCampaignQuery(nav.Link?.Url),
                           Text = nav.Text
                       })
                       .ToArray()
                       .Alter(SetLastItemIsLast));

        private NavItemViewModel[] _footerNavigation;
        public NavItemViewModel[] FooterNavigation
            => _footerNavigation ??
               (_footerNavigation =
                   _dependencies.ItemNavigationTreeFactory.Create(SiteRoot.Email_Footer_Navigation)
                       .FirstOrDefault()?
                       .Children
                       .Select(nav => new NavItemViewModel
                       {
                           LinkUrl = _dependencies.CampaignQueryBuilder.AddCampaignQuery(nav.Link?.Url),
                           Text = nav.Text
                       })
                       .ToArray()
                       .Alter(SetLastItemIsLast));

        private string _rssLinkUrl;
        public string RssLinkUrl => _rssLinkUrl ??
                                    (_rssLinkUrl = _dependencies.CampaignQueryBuilder.AddCampaignQuery(SiteRoot.RSS_Link?.Url));

        private string _linkedInLinkUrl;
        public string LinkedInLinkUrl => _linkedInLinkUrl ??
                                    (_linkedInLinkUrl = _dependencies.CampaignQueryBuilder.AddCampaignQuery(SiteRoot.LinkedIn_Link?.Url));

        private string _twitterLinkUrl;
        public string TwitterLinkUrl => _twitterLinkUrl ??
                                    (_twitterLinkUrl = _dependencies.CampaignQueryBuilder.AddCampaignQuery(SiteRoot.Twitter_Link?.Url));




        private static void SetLastItemIsLast(NavItemViewModel[] arr)
        {
            if (arr.Length != 0) { arr[arr.Length - 1].IsLast = true; }
        }
    }

    public class NavItemViewModel
    {
        public string LinkUrl { get; set; }
        public string Text { get; set; }
        public bool IsLast { get; set; }
    }
}
