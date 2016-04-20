using Informa.Library.Site;
using Informa.Library.Utilities.Extensions;
using Informa.Library.Utilities.Settings;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Informa.Web.ViewModels.SiteDebugging;
using Informa.Web.ViewModels.PopOuts;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Informa.Library.User.Authentication;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using System;
using Glass.Mapper.Sc;
using Informa.Library.Article.Search;
using Informa.Library.Utilities.References;
using System.Web;
using Informa.Library.Globalization;
using Informa.Library.Company;
using Informa.Library.User.Entitlement;
using Informa.Library.Subscription.User;
using Informa.Library.User.Profile;
using System.Text;
using System.Linq;
using Informa.Library.User;
using Sitecore.Social.Infrastructure.Utils;

namespace Informa.Web.ViewModels
{
	public class MainLayoutViewModel : GlassViewModel<I___BasePage>, IEntitledProductItem
	{
		protected readonly ISiteRootContext SiteRootContext;
		public readonly IUserCompanyContext UserCompanyContext;
		protected readonly ITextTranslator TextTranslator;
		protected readonly ISiteSettings SiteSettings;
		protected readonly IAuthenticatedUserContext AuthenticatedUserContext;
		protected readonly ISitecoreService Service;
		protected readonly IArticleSearch ArticleSearch;
		protected readonly IItemReferences ItemReferences;
		protected readonly IUserProfileContext UserProfileContext;
		protected readonly IEntitlementAccessLevelContext EntitlementAccessLevelContext;
		protected readonly IUserSubscriptionsContext UserSubscriptionsContext;
		protected readonly IUserEntitlementsContext UserEntitlementsContext;
		protected readonly IUserIpAddressContext UserIpAddressContext;
		protected readonly IEntitledProductContext EntitledProductContext;

		public MainLayoutViewModel(
			ISiteRootContext siteRootContext,
			IMaintenanceViewModel maintenanceViewModel,
			ICompanyRegisterMessageViewModel companyRegisterMessageViewModel,
			ISideNavigationMenuViewModel sideNavigationMenuViewModel,
			IHeaderViewModel headerViewModel,
			IFooterViewModel footerViewModel,
			ISignInPopOutViewModel signInPopOutViewModel,
			IEmailArticlePopOutViewModel emailArticlePopOutViewModel,
			IEmailSearchPopOutViewModel emailSearchPopOutViewModel,
			IRegisterPopOutViewModel registerPopOutViewModel,
			IAppInsightsConfig appInsightsConfig,
			ISiteSettings siteSettings,
			IToolbarViewModel debugToolbar,
			IIndividualRenewalMessageViewModel renewalInfo,
			IAuthenticatedUserContext authenticatedUserContext,
			ISitecoreService service,
			IArticleSearch articleSearch,
			IItemReferences itemReferences,
			ITextTranslator textTranslator,
		    IUserCompanyContext userCompanyContext,
			IUserProfileContext userProfileContext,
			IEntitlementAccessLevelContext entitlementAccessLevelContext,
			IUserSubscriptionsContext userSubscriptionsContext,
			IUserEntitlementsContext userEntitlementsContext,
			IUserIpAddressContext userIpAddressContext,
			IEntitledProductContext entitledProductContext)
		{
			SiteRootContext = siteRootContext;
			MaintenanceMessage = maintenanceViewModel;
			CompanyRegisterMessage = companyRegisterMessageViewModel;
			SideNavigationMenu = sideNavigationMenuViewModel;
			Header = headerViewModel;
			Footer = footerViewModel;
			SignInPopOutViewModel = signInPopOutViewModel;
			EmailArticlePopOutViewModel = emailArticlePopOutViewModel;
			EmailSearchPopOutViewModel = emailSearchPopOutViewModel;
			RegisterPopOutViewModel = registerPopOutViewModel;
			AppInsightsConfig = appInsightsConfig;
			SiteSettings = siteSettings;
			IndividualRenewalMessageInfo = renewalInfo;
			DebugToolbar = debugToolbar;
			AuthenticatedUserContext = authenticatedUserContext;
			Service = service;
			ArticleSearch = articleSearch;
			ItemReferences = itemReferences;
			TextTranslator = textTranslator;
			UserCompanyContext = userCompanyContext;
			UserProfileContext = userProfileContext;
			EntitlementAccessLevelContext = entitlementAccessLevelContext;
			UserSubscriptionsContext = userSubscriptionsContext;
			UserEntitlementsContext = userEntitlementsContext;
			UserIpAddressContext = userIpAddressContext;
			EntitledProductContext = entitledProductContext;
		}

		public readonly IIndividualRenewalMessageViewModel IndividualRenewalMessageInfo;
		public readonly IMaintenanceViewModel MaintenanceMessage;
		public readonly ICompanyRegisterMessageViewModel CompanyRegisterMessage;
		public readonly ISideNavigationMenuViewModel SideNavigationMenu;
		public readonly IFooterViewModel Footer;
		public readonly IHeaderViewModel Header;
		public readonly ISignInPopOutViewModel SignInPopOutViewModel;
		public readonly IEmailArticlePopOutViewModel EmailArticlePopOutViewModel;
		public readonly IEmailSearchPopOutViewModel EmailSearchPopOutViewModel;
		public readonly IToolbarViewModel DebugToolbar;
		public readonly IRegisterPopOutViewModel RegisterPopOutViewModel;
		public readonly IAppInsightsConfig AppInsightsConfig;

		public IArticle Article => GlassModel is IArticle ? (IArticle)GlassModel : null;
		public string PrintPageHeaderLogoSrc => SiteRootContext?.Item?.Print_Logo?.Src ?? string.Empty;
		public HtmlString PrintPageHeaderMessage => new HtmlString(SiteRootContext.Item.Print_Message);
		public string PrintedByText => TextTranslator.Translate("Header.PrintedBy");
		public string UserName => AuthenticatedUserContext.User.Name;
		public string CorporateName => UserCompanyContext?.Company?.Name;
		public string Title
		{
			get
			{
				var pageTitle = GlassModel?.Meta_Title_Override.StripHtml() ?? string.Empty;
				if (string.IsNullOrWhiteSpace(pageTitle))
					pageTitle = GlassModel?.Title?.StripHtml() ?? string.Empty;
				if (string.IsNullOrWhiteSpace(pageTitle))
					pageTitle = GlassModel?._Name ?? string.Empty;

				var publicationName = (SiteRootContext.Item == null)
					? string.Empty
					: $" :: {SiteRootContext.Item.Publication_Name.StripHtml()}";

				return string.Concat(pageTitle, publicationName);
			}
		}
		public string PageTitleAnalytics => GlassModel?.Title ?? string.Empty;
		public string SiteEnvrionment
		{
			get
			{
				return SiteSettings.GetSetting("Env.Value", string.Empty);
			}
		}
		public string PageType { get { return Sitecore.Context.Item.TemplateName; } }
		//TODO : Uncomment this once the value to be passed is determined
		//	public string CountryCode { get { return "123"; } }
		public string PageDescription => GlassModel.Meta_Description;
		public string PageTitleOverride => GlassModel.Meta_Title_Override;
		public string CustomTags => GlassModel.Custom_Meta_Tags;
		public string MetaKeyWords => GlassModel.Meta_Keywords;
		public bool IsUserLoggedIn => AuthenticatedUserContext.IsAuthenticated;
		public string ArticlePublishDate
		{
			get
			{
				if (Article != null && Article.Actual_Publish_Date > DateTime.MinValue)
				{
					return Article.Actual_Publish_Date.ToString("MM/dd/yyyy");
				}
				return DateTime.MinValue.ToString("MM/dd/yyyy");
			}
		}
		public string ArticleContentType => Article?.Content_Type?.Item_Name;
		public string ArcticleNumber => Article?.Article_Number;
		public bool ArticleEmbargoed => Article?.Embargoed ?? false;
		public string ArticleMediaType => Article?.Media_Type?.Item_Name;
		public string DateCreated
		{
			get
			{
				var currentItem = Sitecore.Context.Item;
				if (currentItem != null && currentItem.Statistics.Created > DateTime.MinValue)
				{
					return currentItem.Statistics.Created.ToServerTimeZone().ToString("MM/dd/yyyy");
				}

				return DateTime.MinValue.ToString("MM/dd/yyyy");
			}
		}
		public string ArticleAuthors => Article != null ? ArticleSearch.GetArticleAuthors(Article._Id) : string.Empty;
		public string ArticleRegions => GetArticleTaxonomy(ItemReferences.RegionsTaxonomyFolder);
		public string ArticleSubject => GetArticleTaxonomy(ItemReferences.SubjectsTaxonomyFolder);
		public string ArticleTherapy => GetArticleTaxonomy(ItemReferences.TherapyAreasTaxonomyFolder);
		public string UserEntitlements
		{
			get
			{
				var entitlementList = UserEntitlementsContext.Entitlements;
				if (entitlementList != null && entitlementList.Any())
				{
					StringBuilder strEntitledProducts = new StringBuilder();
					var lastEntitlement = entitlementList.LastOrDefault();
					strEntitledProducts.Append("[");
					foreach (var entitlement in entitlementList)
					{
						strEntitledProducts.Append("'");
						strEntitledProducts.Append(entitlement.ProductCode);
						strEntitledProducts.Append("'");
						if (entitlementList.Count() > 1 && !lastEntitlement.Equals(entitlement))
						{
							strEntitledProducts.Append(",");
						}
					}
					strEntitledProducts.Append("]");
					return strEntitledProducts.ToString();
				}
				return string.Empty;
			}
		}
		public string UserEntitlementStatus
		{
			get
			{
				var entitlementList = UserEntitlementsContext.Entitlements;
				if (entitlementList != null && entitlementList.Any())
				{
					StringBuilder strEntitledProducts = new StringBuilder();
					var lastEntitlement = entitlementList.LastOrDefault();
					strEntitledProducts.Append("[");
					foreach (var entitlement in entitlementList)
					{
						var entitledStatus = EntitlementAccessLevelContext.Determine(entitlement);
						strEntitledProducts.Append("'");
						strEntitledProducts.Append(entitledStatus.ToString());
						strEntitledProducts.Append("'");
						if (entitlementList.Count() > 1 && !lastEntitlement.Equals(entitlement))
						{
							strEntitledProducts.Append(",");
						}
					}
					strEntitledProducts.Append("]");
					return strEntitledProducts.ToString();
				}
				return string.Empty;
			}
		}
		public string SubscribedProducts
		{
			get
			{
				var subscriptions = UserSubscriptionsContext.Subscriptions;

				if (subscriptions != null && subscriptions.Any())
				{
					StringBuilder strSubscription = new StringBuilder();
					var lastSubscription = subscriptions.LastOrDefault();
					strSubscription.Append("[");
					foreach (var subscription in subscriptions)
					{
						strSubscription.Append("'");
						strSubscription.Append(subscription.ProductCode);
						strSubscription.Append("'");
						if (subscriptions.Count() > 1 && !lastSubscription.Equals(subscription))
						{
							strSubscription.Append(",");
						}
					}
					strSubscription.Append("]");
					return strSubscription.ToString();
				}
				return string.Empty;
			}
		}
		public string UserCompany => UserCompanyContext?.Company?.Name;
		public string CompanyId => UserCompanyContext?.Company?.Id;
		public string UserIndustry => UserProfileContext.Profile?.JobIndustry ?? string.Empty;
		public string UserEmail => UserProfileContext.Profile?.Email ?? string.Empty;
		public string CanonicalUrl => GlassModel?.Canonical_Link?.GetLink();

		public string UserIp => UserIpAddressContext.IpAddress.ToString();
		public string GetArticleTaxonomy(Guid itemId)
		{
			return Article != null ? ArticleSearch.GetArticleTaxonomies(Article._Id, itemId) : string.Empty;
		}
		public string Article_Entitlement => GetArticleEntitlements();

		public bool IsFree
		{
			get
			{
				return Article?.Free_Article ?? false;
			}
		}

		public string GetArticleEntitlements()
		{
			if (IsFree)
			{
				return "Free View";
			}
			else if (EntitledProductContext.IsEntitled(this))
			{
				return "Entitled Full View";
			}
			return "Unentitled Abstract View";
		}

	}
}