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
using System.Linq;
using Informa.Library.User;
using Sitecore.Social.Infrastructure.Utils;

namespace Informa.Web.ViewModels
{
	public class MainLayoutViewModel : GlassViewModel<I___BasePage>
	{
		protected readonly ISiteRootContext SiteRootContext;
		public readonly IUserCompanyContext UserCompanyContext;
		protected readonly ITextTranslator TextTranslator;
		protected readonly ISiteSettings SiteSettings;
		protected readonly IAuthenticatedUserContext AuthenticatedUserContext;
		protected readonly ISitecoreService Service;
		protected readonly IArticleSearch ArticleSearch;
		protected readonly IUserProfileContext UserProfileContext;
		protected readonly IUserSubscriptionsContext UserSubscriptionsContext;
		protected readonly IUserEntitlementsContext UserEntitlementsContext;
		protected readonly IUserIpAddressContext UserIpAddressContext;
		protected readonly IIsEntitledProducItemContext IsEntitledProductItemContext;

		public MainLayoutViewModel(
			ISiteRootContext siteRootContext,
			IMaintenanceViewModel maintenanceViewModel,
			ICompanyRegisterMessageViewModel companyRegisterMessageViewModel,
			ISignInPopOutViewModel signInPopOutViewModel,
			IEmailArticlePopOutViewModel emailArticlePopOutViewModel,
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
			IUserSubscriptionsContext userSubscriptionsContext,
			IUserEntitlementsContext userEntitlementsContext,
			IUserIpAddressContext userIpAddressContext,
			IIsEntitledProducItemContext isEntitledProductItemContext)
		{
			SiteRootContext = siteRootContext;
			MaintenanceMessage = maintenanceViewModel;
			CompanyRegisterMessage = companyRegisterMessageViewModel;
			SignInPopOutViewModel = signInPopOutViewModel;
			EmailArticlePopOutViewModel = emailArticlePopOutViewModel;
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
			UserSubscriptionsContext = userSubscriptionsContext;
			UserEntitlementsContext = userEntitlementsContext;
			UserIpAddressContext = userIpAddressContext;
			IsEntitledProductItemContext = isEntitledProductItemContext;
		}

		public readonly IIndividualRenewalMessageViewModel IndividualRenewalMessageInfo;
		public readonly IMaintenanceViewModel MaintenanceMessage;
		public readonly ICompanyRegisterMessageViewModel CompanyRegisterMessage;
		public readonly ISignInPopOutViewModel SignInPopOutViewModel;
		public readonly IEmailArticlePopOutViewModel EmailArticlePopOutViewModel;
		public readonly IToolbarViewModel DebugToolbar;
		public readonly IRegisterPopOutViewModel RegisterPopOutViewModel;
		public readonly IAppInsightsConfig AppInsightsConfig;
		public readonly IItemReferences ItemReferences;

		public IArticle Article => GlassModel as IArticle;
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
		public bool DisplayPageTitle => GlassModel is ITopic_Page;
		public string BodyCssClass => string.IsNullOrEmpty(SiteRootContext.Item?.Publication_Theme)
			? string.Empty
			: $"class={SiteRootContext.Item.Publication_Theme}";

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
				string allEntitlements = string.Join(",", UserEntitlementsContext.Entitlements.Select(a => $"'{a.ProductCode}'"));
				return !string.IsNullOrEmpty(allEntitlements) ? $"[{allEntitlements}]" : string.Empty;
			}
		}
		public string UserEntitlementStatus => IsEntitledProductItemContext.IsEntitled(Article) ? "entitled" : "unentitled";
		public string SubscribedProducts
		{
			get
			{
				var subscriptions = UserSubscriptionsContext.Subscriptions;
				string allSubscriptions = subscriptions == null ? string.Empty : string.Join(",", UserSubscriptionsContext.Subscriptions.Select(a => $"'{a.ProductCode}'"));
				return !string.IsNullOrEmpty(allSubscriptions) ? $"[{allSubscriptions}]" : string.Empty;
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
				return Article?.Free ?? false;
			}
		}

		public string GetArticleEntitlements()
		{
			if (IsFree)
			{
				return "Free View";
			}
			if (IsEntitledProductItemContext.IsEntitled(Article))
			{
				return "Entitled Full View";
			}
			return "Unentitled Abstract View";
		}

	}
}