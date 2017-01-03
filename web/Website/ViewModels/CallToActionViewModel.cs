using Informa.Library.Globalization;
using Informa.Library.Site;
using Jabberwocky.Glass.Autofac.Attributes;
using Glass.Mapper.Sc.Fields;
using Informa.Library.User.Authentication;
using Informa.Library.ViewModels.Account;
using Informa.Web.ViewModels.PopOuts;
using Glass.Mapper.Sc;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;

namespace Informa.Web.ViewModels
{
	[AutowireService]
	public class CallToActionViewModel : ICallToActionViewModel
	{
		protected readonly ITextTranslator TextTranslator;
		protected readonly ISiteRootContext SiteRootContext;
		protected readonly IAuthenticatedUserContext AuthenticatedUserContext;
		protected readonly IArticle CurrentItem;

		public CallToActionViewModel(
			ITextTranslator textTranslator,
			ISignInViewModel signInViewModel,
			IRegisterPopOutViewModel registerPopOutViewModel,
			ISiteRootContext siteRootContext,
			IAuthenticatedUserContext authenticatedUserContext,
			ISitecoreContext sitecoreContext)
		{
			TextTranslator = textTranslator;
			SignInViewModel = signInViewModel;
			RegisterPopOutViewModel = registerPopOutViewModel;
			SiteRootContext = siteRootContext;
			AuthenticatedUserContext = authenticatedUserContext;
			CurrentItem = sitecoreContext.GetCurrentItem<IArticle>();
		}

        #region Implementation of ICallToActionViewModel

        //public ISignInViewModel SignInViewModel { get; }
        //public IRegisterPopOutViewModel RegisterPopOutViewModel { get; }
        //public string SigninTitle => TextTranslator.Translate("CallToAction.SignIn.Title");
        //public string SigninSubtitle => string.IsNullOrEmpty(SiteRootContext?.Item?.SignIn_SubTitle) ? TextTranslator.Translate("CallToAction.SignIn.SubTitle") : SiteRootContext?.Item?.SignIn_SubTitle;
        //public string RegisterTitle => CurrentItem.Free_With_Registration ? TextTranslator.Translate("CallToAction.Register.TitleForFreeArticles") : TextTranslator.Translate("CallToAction.Register.Title");
        //public string RegisterSubtitle
        //{
        //	get
        //	{
        //		if (CurrentItem.Free_With_Registration)
        //			return string.IsNullOrEmpty(SiteRootContext?.Item?.Register_SubTitle_For_Free_Articles) ? TextTranslator.Translate("CallToAction.Register.SubTitleForFreeArticles") : SiteRootContext?.Item?.Register_SubTitle_For_Free_Articles;
        //		else
        //			return string.IsNullOrEmpty(SiteRootContext?.Item?.Register_SubTitle) ? TextTranslator.Translate("CallToAction.Register.SubTitle") : SiteRootContext?.Item?.Register_SubTitle;
        //	}
        //}
        //public string SubscribeTitle => TextTranslator.Translate("CallToAction.Subscribe.Title");
        //public string SubscribeLinkUrl => SiteRootContext?.Item?.Subscribe_Link?.Url ?? string.Empty;
        //public string SubscribeLinkText => SiteRootContext?.Item?.Subscribe_Link?.Text ?? string.Empty;
        //public Link PurchaseLink => SiteRootContext?.Item?.Purchase_Link;
        //public bool IsAuthenticated => AuthenticatedUserContext.IsAuthenticated;
        public ISignInViewModel SignInViewModel { get; }
        public IRegisterPopOutViewModel RegisterPopOutViewModel { get; }
        public string SigninTitle => TextTranslator.Translate("CallToAction.SignIn.Title");
        public string SigninSubtitle => TextTranslator.Translate("CallToAction.SignIn.SubTitle");
        public string RegisterTitle => TextTranslator.Translate("CallToAction.Register.Title");
        public string RegisterSubtitle => TextTranslator.Translate("CallToAction.Register.SubTitle");
        public string SubscribeTitle => TextTranslator.Translate("CallToAction.Subscribe.Title");
        public string SubscribeLinkUrl => SiteRootContext?.Item?.Subscribe_Link?.Url ?? string.Empty;
        public string SubscribeLinkText => SiteRootContext?.Item?.Subscribe_Link?.Text ?? string.Empty;
        public Link PurchaseLink => SiteRootContext?.Item?.Purchase_Link;
        public bool IsAuthenticated => AuthenticatedUserContext.IsAuthenticated;

        #endregion
    }
}