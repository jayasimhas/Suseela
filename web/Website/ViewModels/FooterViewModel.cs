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
				IFooterNavigationMenuViewModel footerNavViewModel)
		{
			SiteRootContext = siteRootContext;
			PageLinksFactory = pageLinksFactory;
			AuthenticatedUserContext = authenticatedUserContext;
			UserSubscriptionContext = userSubscriptionContext;
			TextTranslator = textTranslator;
			_footerNavViewModel = footerNavViewModel;
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

		public IEnumerable<IPageLink> LocalLinks => SiteRootContext.Item == null ? Enumerable.Empty<IPageLink>() : PageLinksFactory.Create(SiteRootContext.Item.Local_Footer_Links);

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