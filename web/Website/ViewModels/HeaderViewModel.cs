﻿using System.EnterpriseServices;
using Glass.Mapper.Sc;
using Glass.Mapper.Sc.Fields;
using Informa.Library.User.Authentication;
using Informa.Library.Company;
using Informa.Library.Globalization;
using Informa.Library.Site;
using Informa.Library.Utilities.References;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Global;
using Jabberwocky.Glass.Autofac.Attributes;
using Sitecore.Analytics;
using System.Web;
using System.Web.Mvc;
using Informa.Library.User.Profile;

namespace Informa.Web.ViewModels
{
	[AutowireService]
	public class HeaderViewModel : IHeaderViewModel
	{
		protected readonly IAuthenticatedUserContext AuthenticatedUserContext;
		protected readonly IUserCompanyNameContext CompanyNameContext;
		protected readonly ITextTranslator TextTranslator;
		protected readonly ISiteHomeContext SiteHomeContext;
		protected readonly ISiteRootContext SiteRootContext;
		protected readonly ISitecoreService SitecoreService;
		protected readonly ISitecoreContext SitecoreContext;
		protected readonly IItemReferences ItemReferences;
        protected readonly IUserProfileContext ProfileContext;

		public HeaderViewModel(
			IAuthenticatedUserContext authenticatedUserContext,
			IUserCompanyNameContext companyNameContext,
			ITextTranslator textTranslator,
			ISiteHomeContext siteHomeContext,
			ISiteRootContext siteRootContext,
			ISitecoreService sitecoreService,
			ISitecoreContext sitecoreContext,
			IItemReferences itemReferences,
            IUserProfileContext profileContext)
		{
			AuthenticatedUserContext = authenticatedUserContext;
			CompanyNameContext = companyNameContext;
			TextTranslator = textTranslator;
			SiteHomeContext = siteHomeContext;
			SiteRootContext = siteRootContext;
			SitecoreService = sitecoreService;
			SitecoreContext = sitecoreContext;
			ItemReferences = itemReferences;
            ProfileContext = profileContext;

			InformaBar = SitecoreContext.GetItem<IInforma_Bar>(ItemReferences.InformaBar);
		}

		public string LogoImageUrl => SiteRootContext.Item?.Site_Logo?.Src ?? string.Empty;
		public string LogoUrl => SiteHomeContext.Item?._Url ?? string.Empty;
		public string WelcomeText
		{
			get
            {
                var user = AuthenticatedUserContext.IsAuthenticated
                     ? ProfileContext.Profile
                     : null;
                var accountName = user != null ? user.FirstName : CompanyNameContext.Name;

				return string.IsNullOrWhiteSpace(accountName) ? string.Empty : string.Concat(TextTranslator.Translate("Header.Greeting"), accountName);
			}
		}

		public string PolicyText => TextTranslator.Translate("Global.PolicyText");
		public bool IsAuthenticated => AuthenticatedUserContext.IsAuthenticated;
		public string MyAccountLinkText => TextTranslator.Translate("Header.MyAccount");
		public string MyAccountLink => SitecoreService.GetItem<I___BasePage>(SiteRootContext.Item.My_Account_Page)?._Url ?? "#";
		public string SignOutLinkText => TextTranslator.Translate("Header.SignOut");
		public string RegisterLinkText => TextTranslator.Translate("Header.RegisterLink");
		public string SignInText => TextTranslator.Translate("Header.SignIn");
		public string SignInLinkText => TextTranslator.Translate("Header.SignInLink");
		public string AdvertisementText => TextTranslator.Translate("Ads.Advertisement");
		private IInforma_Bar InformaBar;
		public string LeftColumnText => InformaBar?.Left_Column_Text.Replace("#PRODUCT_NAME#", SiteRootContext.Item.Publication_Name) ?? string.Empty;
		public string RightColumnText => InformaBar?.Right_Column_Text ?? string.Empty;
		public string Link1 => (InformaBar != null) ? BuildLink(InformaBar.Link_1) : string.Empty;
		public string Link2 => (InformaBar != null) ? BuildLink(InformaBar.Link_2) : string.Empty;
		public string Link3 => (InformaBar != null) ? BuildLink(InformaBar.Link_3) : string.Empty;
		public string Link4 => (InformaBar != null) ? BuildLink(InformaBar.Link_4) : string.Empty;

		public string PrintPageHeaderLogoSrc => (SiteRootContext != null && SiteRootContext.Item != null && SiteRootContext.Item.Print_Logo != null) ? SiteRootContext.Item.Print_Logo.Src : string.Empty;
		public HtmlString PrintPageHeaderMessage => new HtmlString(SiteRootContext.Item.Print_Message);
		public string PrintedByText => TextTranslator.Translate("Header.PrintedBy");
		public string UserName => AuthenticatedUserContext.User.Name;
		public string CorporateName => CompanyNameContext.Name;

		private string BuildLink(Link l)
		{
			if (l == null)
				return string.Empty;

			return $"<a href='{l.Url}' target='{l.Target}'>{l.Text}</a>";
		}

		public string LeaderboardSlotID
		{
			get
			{
				string local = SitecoreContext.GetCurrentItem<I___BasePage>()?.Leaderboard_Slot_ID ?? string.Empty;
				return string.IsNullOrEmpty(local)
					? SiteRootContext?.Item?.Global_Leaderboard_Slot_ID ?? string.Empty
					: local;
			}
		}

		public string LeaderboardAdZone => SitecoreService.GetItem<ISite_Config>(Informa.Library.Utilities.References.Constants.ScripRootNode)?.Global_Leaderboard_Ad_Zone ?? string.Empty;
	}
}
