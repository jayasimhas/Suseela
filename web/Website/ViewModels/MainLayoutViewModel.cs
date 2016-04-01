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
using Informa.Library.Salesforce.User.Profile;
using Informa.Library.User.Entitlement;
using Informa.Library.Subscription.User;

namespace Informa.Web.ViewModels
{
	public class MainLayoutViewModel : GlassViewModel<I___BasePage>
	{
		protected readonly ISiteRootContext SiteRootContext;
		protected readonly ICompanyNameContext CompanyNameContext;
		protected readonly ITextTranslator TextTranslator;
		protected readonly ISiteSettings SiteSettings;
		protected readonly IAuthenticatedUserContext AuthenticatedUserContext;
		protected readonly ISitecoreService Service;
		protected readonly IArticleSearch ArticleSearch;
		protected readonly IItemReferences ItemReferences;
		protected readonly ISalesforceFindUserProfile SalesforceFindUserProfile;
		protected readonly IEntitlementAccessLevelContext EntitlementAccessLevelContext;
		protected readonly IUserSubscriptionsContext UserSubscriptionsContext;

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
			ICompanyNameContext companyNameContext,
			ISalesforceFindUserProfile salesforceFindUserProfile,
			IEntitlementAccessLevelContext entitlementAccessLevelContext,
			IUserSubscriptionsContext userSubscriptionsContext)
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
			CompanyNameContext = companyNameContext;
			SalesforceFindUserProfile = salesforceFindUserProfile;
			SfUserProfile = salesforceFindUserProfile.Find(authenticatedUserContext.User.Username);
			EntitlementAccessLevelContext = entitlementAccessLevelContext;
			UserSubscriptionsContext = userSubscriptionsContext;
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

		public string PrintPageHeaderLogoSrc => SiteRootContext?.Item?.Print_Logo?.Src ?? string.Empty;
		public HtmlString PrintPageHeaderMessage => new HtmlString(SiteRootContext.Item.Print_Message);
		public string PrintedByText => TextTranslator.Translate("Header.PrintedBy");
		public string UserName => AuthenticatedUserContext.User.Name;
		public string CorporateName => CompanyNameContext.Name;

		public ISalesforceUserProfile SfUserProfile;
		public string Title
		{
			get
			{
				var pageTitle = string.Empty;

				if (GlassModel is I___BasePage)
				{
					var page = (I___BasePage)GlassModel;

					if (!string.IsNullOrEmpty(page.Meta_Title_Override))
					{
						return page.Meta_Title_Override.StripHtml();
					}

					pageTitle = page.Title.StripHtml();
				}

				if (string.IsNullOrWhiteSpace(pageTitle) && GlassModel != null)
				{
					pageTitle = GlassModel._Name;
				}

				var publicationName = SiteRootContext.Item == null ? string.Empty : string.Format(" :: {0}", SiteRootContext.Item.Publication_Name.StripHtml());

				return string.Concat(pageTitle, publicationName);
			}
		}

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

		public string PageDescription
		{
			get
			{
				if (GlassModel is I___BasePage)
				{
					var page = (I___BasePage)GlassModel;

					if (!string.IsNullOrEmpty(page.Meta_Description))
					{
						return page.Meta_Description;
					}
				}
				return string.Empty;
			}
		}

		public string Page_Title_Override
		{
			get
			{
				if (GlassModel is I___BasePage)
				{
					var page = (I___BasePage)GlassModel;

					if (!string.IsNullOrEmpty(page.Meta_Title_Override))
					{
						return page.Meta_Title_Override;
					}
				}
				return string.Empty;
			}
		}

		public string Custom_Tags
		{
			get
			{
				if (GlassModel is I___BasePage)
				{
					var page = (I___BasePage)GlassModel;

					if (!string.IsNullOrEmpty(page.Custom_Meta_Tags))
					{

						return page.Custom_Meta_Tags;
					}
				}
				return string.Empty;
			}
		}


		public string Meta_KeyWords
		{
			get
			{
				if (GlassModel is I___BasePage)
				{
					var page = (I___BasePage)GlassModel;

					if (!string.IsNullOrEmpty(page.Meta_Keywords))
					{
						return page.Meta_Keywords;
					}
				}
				return string.Empty;
			}
		}

		public bool IsUserLoggedIn
		{
			get
			{
				return AuthenticatedUserContext.IsAuthenticated;
			}
		}

		public string Article_Publish_Date
		{
			get
			{
				if (GlassModel is IArticle__Raw)
				{
					var page = (IArticle__Raw)GlassModel;

					if (page.Actual_Publish_Date > DateTime.MinValue)
					{
						return page.Actual_Publish_Date.ToString();
					}
				}
				return string.Empty;
			}
		}
		public string Article_Content_Type
		{
			get
			{
				if (GlassModel is IArticle__Raw)
				{
					ArticleItem articleItem = Service.GetItem<ArticleItem>(GlassModel._Id);

					if (articleItem.Content_Type != null)
					{
						return articleItem.Content_Type.Item_Name;
					}
				}
				return string.Empty;
			}
		}

		public string Arcticle_Number
		{
			get
			{
				if (GlassModel is IArticle__Raw)
				{
					var page = (IArticle__Raw)GlassModel;
					if (!string.IsNullOrEmpty(page.Article_Number))
					{
						return page.Article_Number;
					}
				}
				return string.Empty;
			}
		}
		public bool Article_Embargoed
		{
			get
			{
				if (GlassModel is IArticle__Raw)
				{
					ArticleItem articleItem = Service.GetItem<ArticleItem>(GlassModel._Id);
					return articleItem.Embargoed;

				}
				return false;
			}
		}

		public string Article_Media_Type
		{
			get
			{
				if (GlassModel is IArticle__Raw)
				{
					ArticleItem articleItem = Service.GetItem<ArticleItem>(GlassModel._Id);
					if (articleItem.Media_Type != null)
					{
						return articleItem.Media_Type.Item_Name;
					}

				}
				return string.Empty;
			}
		}

		public string Article_Date_Created
		{
			get
			{
				if (GlassModel is IArticle__Raw)
				{
					var page = (IArticle__Raw)GlassModel;

					if (page.Created_Date > DateTime.MinValue)
					{
						return page.Created_Date.ToString();
					}
				}
				return string.Empty;
			}

		}

		public string Article_Authors
		{
			get
			{
				if (GlassModel is IArticle__Raw)
				{
					return ArticleSearch.GetArticleAuthors(GlassModel._Id);
				}
				return string.Empty;
			}

		}

		public string Article_Regions
		{
			get
			{
				if (GlassModel is IArticle__Raw)
				{
					return ArticleSearch.GetArticleTaxonomies(GlassModel._Id, ItemReferences.RegionsTaxonomyFolder);
				}
				return string.Empty;
			}

		}
		public string Article_Subject
		{
			get
			{
				if (GlassModel is IArticle__Raw)
				{
					return ArticleSearch.GetArticleTaxonomies(GlassModel._Id, ItemReferences.SubjectsTaxonomyFolder);
				}
				return string.Empty;
			}

		}
		public string Article_Therapy
		{
			get
			{
				if (GlassModel is IArticle__Raw)
				{
					return ArticleSearch.GetArticleTaxonomies(GlassModel._Id, ItemReferences.TherapyAreasTaxonomyFolder);
				}
				return string.Empty;
			}

		}

		public bool IsArticleFree
		{
			get
			{
				if (GlassModel is IArticle__Raw)
				{
					var page = (IArticle__Raw)GlassModel;
					return page.Free_Article;
				}
				return false;
			}

		}

		public string UserEntitlements
		{
			get
			{
				return EntitlementAccessLevelContext.GetEntitledProducts();
			}
		}

		public string UserEntitlementStatus
		{
			get { return EntitlementAccessLevelContext.GetEntitledProductStatus(); }
		}

		public string Subscribed_Products
		{
			get { return UserSubscriptionsContext.GetSubscribed_Products(); }
		}

		public string User_Company
		{
			get { return (SfUserProfile != null ? SfUserProfile.Company : string.Empty); }
		}

		public string User_Industry
		{
			get
			{
				return (SfUserProfile != null ? SfUserProfile.JobIndustry : string.Empty);
			}
		}

		public string User_Email
		{
			get
			{
				return (SfUserProfile != null ? SfUserProfile.Email : string.Empty);
			}
		}
		public string CanonicalUrl => GlassModel?.Canonical_Link?.GetLink();
	}
}