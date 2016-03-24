using Glass.Mapper.Sc.Fields;
using Informa.Library.User.Authentication;
using Informa.Library.Globalization;
using Informa.Library.Site;
using Informa.Library.Subscription.User;
using Informa.Web.Models;
using Jabberwocky.Glass.Autofac.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Informa.Web.ViewModels
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class FooterViewModel : IFooterViewModel
	{
		protected readonly ISiteRootContext SiteRootContext;
		protected readonly IPageLinksFactory PageLinksFactory;
		protected readonly IAuthenticatedUserContext AuthenticatedUserContext;
		protected readonly IUserSubscribedContext UserSubscriptionContext;
		protected readonly ITextTranslator TextTranslator;

		public FooterViewModel(
			ISiteRootContext siteRootContext,
			IPageLinksFactory pageLinksFactory,
			IAuthenticatedUserContext authenticatedUserContext,
			IUserSubscribedContext userSubscriptionContext,
			ITextTranslator textTranslator)
		{
			SiteRootContext = siteRootContext;
			PageLinksFactory = pageLinksFactory;
			AuthenticatedUserContext = authenticatedUserContext;
			UserSubscriptionContext = userSubscriptionContext;
			TextTranslator = textTranslator;
		}

		public string FooterLogoUrl => SiteRootContext.Item == null ? string.Empty : SiteRootContext.Item.Footer_Logo.Src;

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

		public IEnumerable<IPageLink> MenuOneLinks => SiteRootContext.Item == null ? Enumerable.Empty<IPageLink>() : PageLinksFactory.Create(SiteRootContext.Item.Menu_One_Links);

		public string MenuTwoHeader => SiteRootContext.Item == null ? string.Empty : SiteRootContext.Item.Menu_Two_Header;

		public IEnumerable<IPageLink> MenuTwoLinks => SiteRootContext.Item == null ? Enumerable.Empty<IPageLink>() : PageLinksFactory.Create(SiteRootContext.Item.Menu_Two_Links);
	}
}