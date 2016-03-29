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

namespace Informa.Web.ViewModels
{
	public class MainLayoutViewModel : GlassViewModel<I___BasePage>
	{
		protected readonly ISiteRootContext SiteRootContext;

		public MainLayoutViewModel(
			ISiteRootContext siteRootContext,
			IMaintenanceViewModel maintenanceViewModel,
			ICompanyRegisterMessageViewModel companyRegisterMessageViewModel,
			ISideNavigationMenuViewModel sideNavigationMenuViewModel,
			IHeaderViewModel headerViewModel,
			IFooterViewModel footerViewModel,
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
			IItemReferences itemReferences)
		{
			SiteRootContext = siteRootContext;
			MaintenanceMessage = maintenanceViewModel;
			CompanyRegisterMessage = companyRegisterMessageViewModel;
			SideNavigationMenu = sideNavigationMenuViewModel;
			Header = headerViewModel;
			Footer = footerViewModel;
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

		}

		public IIndividualRenewalMessageViewModel IndividualRenewalMessageInfo;
		public IMaintenanceViewModel MaintenanceMessage;
		public ICompanyRegisterMessageViewModel CompanyRegisterMessage;
		public ISideNavigationMenuViewModel SideNavigationMenu;
		public IFooterViewModel Footer;
		public IHeaderViewModel Header;
		public ISignInPopOutViewModel SignInPopOutViewModel;
		public IEmailArticlePopOutViewModel EmailArticlePopOutViewModel;
		public IRegisterPopOutViewModel RegisterPopOutViewModel;
		public IAppInsightsConfig AppInsightsConfig;
		public ISiteSettings SiteSettings;
		public IToolbarViewModel DebugToolbar;
		public IAuthenticatedUserContext AuthenticatedUserContext;
		private ISitecoreService Service;
		private IArticleSearch ArticleSearch;
		private IItemReferences ItemReferences;

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

		public string CountryCode { get { return "123"; } }

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

		public string CanonicalUrl => GlassModel?.Canonical_Link?.GetLink();
	}
}