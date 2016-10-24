using Glass.Mapper.Sc.Fields;
using Informa.Library.User.Authentication;
using Informa.Library.Globalization;
using Informa.Library.Site;
using Informa.Web.Models;
using System.Collections.Generic;
using System.Linq;
using Informa.Library.Navigation;
using Informa.Library.Subscription.User;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Sitecore.Data;
using Glass.Mapper.Sc;
using Informa.Library.Utilities.References;

namespace Informa.Web.ViewModels
{
    public class FooterViewModel : GlassViewModel<I___BasePage>
    {
        protected readonly ISiteRootContext SiteRootContext;
        protected readonly IPageLinksFactory PageLinksFactory;
        protected readonly IAuthenticatedUserContext AuthenticatedUserContext;
        protected readonly IUserSubscribedContext UserSubscriptionContext;
        protected readonly ITextTranslator TextTranslator;
        protected readonly IFooterNavigationMenuViewModel _footerNavViewModel;

        public FooterViewModel(
                ISiteRootContext siteRootContext,
                IPageLinksFactory pageLinksFactory,
                IAuthenticatedUserContext authenticatedUserContext,
                IUserSubscribedContext userSubscriptionContext,
                ITextTranslator textTranslator,
                ISitecoreService sitecoreService,
                IFooterNavigationMenuViewModel footerNavViewModel)
        {
            SiteRootContext = siteRootContext;
            PageLinksFactory = pageLinksFactory;
            AuthenticatedUserContext = authenticatedUserContext;
            UserSubscriptionContext = userSubscriptionContext;
            TextTranslator = textTranslator;
            _footerNavViewModel = footerNavViewModel;

            List<IPageLink> lstLocalLinks = new List<IPageLink>();
            foreach (var item in siteRootContext.Item.Local_Footer_Links)
            {
                if (item._TemplateId == new System.Guid("{354B0538-CB81-4B26-A25E-7B5DBA03C2F5}"))
                {
                    var navItem = Sitecore.Context.Database.GetItem(new ID(item._Id));
                    if (navItem != null)
                    {
                        var nav = sitecoreService.GetItem<Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects.Navigation.INavigation_Link>(navItem.ID.Guid);
                        if (nav != null && nav.Navigation_Link != null && string.IsNullOrEmpty(nav.Navigation_Link.Url) == false && string.IsNullOrEmpty(nav.Navigation_Text) == false)
                            lstLocalLinks.Add(new PageLink { Text = nav.Navigation_Text, Url = nav.Navigation_Link.Url });
                    }
                }
                else
                    lstLocalLinks.Add(new PageLink { Text = item._Name, Url = item._Url });
            }

            LocalLinks = lstLocalLinks;
        }

        public string FooterLogoUrl => SiteRootContext.Item == null ? string.Empty : SiteRootContext.Item.Footer_Logo?.Src;

        public string FooterRssLogoUrl => SiteRootContext.Item == null ? string.Empty : SiteRootContext.Item.RSS_Logo?.Src;
        public Link FooterRssLink => SiteRootContext.Item == null ? null : SiteRootContext.Item.RSS_Link;

        public string CopyrightText => SiteRootContext.Item == null ? string.Empty : SiteRootContext.Item.Copyright_Text;

        public Link SubscribeLink
        {
            get
            {
                if (SiteRootContext.Item == null)
                {
                    return null;
                }

                if (!UserSubscriptionContext.IsSubscribed)
                {
                    return SiteRootContext.Item.Subscribe_Link;
                }

                if (AuthenticatedUserContext.IsAuthenticated)
                {
                    return SiteRootContext.Item.Purchase_Link;
                }
                else
                {
                    return SiteRootContext.Item.Register_Link;
                }
            }
        }

        public IEnumerable<IPageLink> LocalLinks
        {
            get; set;
        }// => SiteRootContext.Item == null ? Enumerable.Empty<IPageLink>() : PageLinksFactory.Create(SiteRootContext.Item.Local_Footer_Links);

        public string FollowText => TextTranslator.Translate("Footer.Follow");

        public Link LinkedInLink => SiteRootContext.Item == null ? null : SiteRootContext.Item.LinkedIn_Link;

        public Link FacebookLink => SiteRootContext.Item == null ? null : SiteRootContext.Item.Facebook_Link;

        public Link TwitterLink => SiteRootContext.Item == null ? null : SiteRootContext.Item.Twitter_Link;

        public string MenuOneHeader => SiteRootContext.Item == null ? string.Empty : SiteRootContext.Item.Menu_One_Header;

        //public IEnumerable<IPageLink> MenuOneLinks => SiteRootContext.Item == null ? Enumerable.Empty<IPageLink>() : PageLinksFactory.Create(SiteRootContext.Item.Menu_One_Links);
        public IEnumerable<INavigation> MenuOneLinks => _footerNavViewModel.MenuOneNavigation;

        public string MenuTwoHeader => SiteRootContext.Item == null ? string.Empty : SiteRootContext.Item.Menu_Two_Header;

        //public IEnumerable<IPageLink> MenuTwoLinks => SiteRootContext.Item == null ? Enumerable.Empty<IPageLink>() : PageLinksFactory.Create(SiteRootContext.Item.Menu_Two_Links);
        public IEnumerable<INavigation> MenuTwoLinks => _footerNavViewModel.MenuTwoNavigation;
    }
}