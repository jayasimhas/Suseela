using Informa.Library.Globalization;
using Informa.Library.Site;
using Jabberwocky.Glass.Autofac.Attributes;
using Glass.Mapper.Sc.Fields;
using Informa.Library.User.Authentication;
using Informa.Library.ViewModels.Account;
using Informa.Web.ViewModels.PopOuts;
using Glass.Mapper.Sc;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Informa.Library.SalesforceConfiguration;
using System.Web;
using Informa.Library.Services.Global;

namespace Informa.Web.ViewModels
{
    [AutowireService]
    public class CallToActionViewModel : ICallToActionViewModel
    {
        protected readonly ITextTranslator TextTranslator;
        protected readonly ISiteRootContext SiteRootContext;
        protected readonly IAuthenticatedUserContext AuthenticatedUserContext;
        protected readonly IArticle CurrentItem;
        protected readonly ISalesforceConfigurationContext SalesforceConfigurationContext;
        protected readonly IGlobalSitecoreService GlobalService;

        public CallToActionViewModel(
            ITextTranslator textTranslator,
            ISignInViewModel signInViewModel,
            IRegisterPopOutViewModel registerPopOutViewModel,
            ISiteRootContext siteRootContext,
            IAuthenticatedUserContext authenticatedUserContext,
            ISitecoreContext sitecoreContext,
            ISalesforceConfigurationContext salesforceConfigurationContext, IGlobalSitecoreService globalService)
        {
            TextTranslator = textTranslator;
            SignInViewModel = signInViewModel;
            RegisterPopOutViewModel = registerPopOutViewModel;
            SiteRootContext = siteRootContext;
            AuthenticatedUserContext = authenticatedUserContext;
            CurrentItem = sitecoreContext.GetCurrentItem<IArticle>();
            SalesforceConfigurationContext = salesforceConfigurationContext;
            GlobalService = globalService;
        }

        #region Implementation of ICallToActionViewModel
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
        public bool IsNewSalesforceEnabled => SalesforceConfigurationContext.IsNewSalesforceEnabled;
        public string AuthorizationRequestUrl => SalesforceConfigurationContext.GetLoginEndPoints(SiteRootContext?.Item?.Publication_Code, GetCallbackUrl("/User/ProcessUserRequest"), HttpContext.Current.Request.Url.ToString().Contains("?") ? HttpContext.Current.Request.Url.ToString() + "&vid=" + CurVerticalName : HttpContext.Current.Request.Url.ToString() + "?vid=" + CurVerticalName);
        public string RegistrationUrl => SalesforceConfigurationContext?.GetRegistrationEndPoints(GetCallbackUrl("/User/ProcessUserRequest/Register"), SiteRootContext?.Item?.Publication_Code);
        private string GetCallbackUrl(string url)
        {
            return $"{HttpContext.Current.Request.Url.Scheme}://{HttpContext.Current.Request.Url.Authority}{HttpContext.Current.Request.ApplicationPath.TrimEnd('/')}{url}";

        }
        public string CurVerticalName => GlobalService.GetVerticalRootAncestor(Sitecore.Context.Item.ID.ToGuid())?._Name;
        #endregion
    }
}