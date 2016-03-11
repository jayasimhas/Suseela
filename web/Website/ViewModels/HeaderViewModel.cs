using Glass.Mapper.Sc;
using Informa.Library.User.Authentication;
using Informa.Library.Corporate;
using Informa.Library.Globalization;
using Informa.Library.Site;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Jabberwocky.Glass.Autofac.Attributes;
using Sitecore.Analytics;

namespace Informa.Web.ViewModels
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class HeaderViewModel : IHeaderViewModel
	{
		protected readonly IAuthenticatedUserContext AuthenticatedUserContext;
		protected readonly ICorporateAccountNameContext CorporateAccountNameContext;
		protected readonly ITextTranslator TextTranslator;
		protected readonly ISiteHomeContext SiteHomeContext;
		protected readonly ISiteRootContext SiteRootContext;
	    protected readonly ISitecoreService SitecoreService;
	    protected readonly ISitecoreContext SitecoreContext;

		public HeaderViewModel(
			IAuthenticatedUserContext authenticatedUserContext,
			ICorporateAccountNameContext corporateAccountNameContext,
			ITextTranslator textTranslator,
			ISiteHomeContext siteHomeContext,
			ISiteRootContext siteRootContext,
            ISitecoreService sitecoreService,
            ISitecoreContext sitecoreContext)
		{
			AuthenticatedUserContext = authenticatedUserContext;
			CorporateAccountNameContext = corporateAccountNameContext;
			TextTranslator = textTranslator;
			SiteHomeContext = siteHomeContext;
			SiteRootContext = siteRootContext;
		    SitecoreService = sitecoreService;
		    SitecoreContext = sitecoreContext;

		}

		public string LogoImageUrl => SiteRootContext.Item?.Site_Logo?.Src ?? string.Empty;
		public string LogoUrl => SiteHomeContext.Item?._Url ?? string.Empty;
		public string WelcomeText
		{
			get
			{
				var accountName = AuthenticatedUserContext.IsAuthenticated ? AuthenticatedUserContext.User.Name : CorporateAccountNameContext.Name;

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