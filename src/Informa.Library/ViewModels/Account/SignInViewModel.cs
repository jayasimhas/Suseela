using Informa.Library.Globalization;
using Informa.Library.Site;
using System.Web;
using Informa.Library.ViewModels.Account;
using Jabberwocky.Autofac.Attributes;
using System;
using Informa.Library.Services.Global;
using Informa.Library.SalesforceConfiguration;

namespace Informa.Web.ViewModels
{
    [AutowireService(LifetimeScope.PerScope)]
    public class SignInViewModel : ISignInViewModel
    {
        protected readonly ISiteRootContext SiteRootContext;
        protected readonly ITextTranslator TextTranslator;
        protected readonly IGlobalSitecoreService GlobalService;
        protected readonly ISalesforceConfigurationContext SalesforceConfigurationContext;


        public SignInViewModel()
        {
        }

        public SignInViewModel(
                ISiteRootContext siteRootContext,
                ITextTranslator textTranslator, IGlobalSitecoreService globalService,
                ISalesforceConfigurationContext salesforceConfigurationContext)
        {
            SiteRootContext = siteRootContext;
            TextTranslator = textTranslator;
            GlobalService = globalService;
            SalesforceConfigurationContext = salesforceConfigurationContext;
        }

        public string SignInButtonText => TextTranslator.Translate("Authentication.SignIn.Submit");
        public string SignInInvalidText => TextTranslator.Translate("Authentication.SignIn.ErrorInvalid");

        public string SignInPasswordPlaceholderText
                => TextTranslator.Translate("Authentication.SignIn.PasswordPlaceholder");

        public string SignInRememberMeText => TextTranslator.Translate("Authentication.SignIn.RememberMe");

        public string SignInUsernamePlaceholderText
                => TextTranslator.Translate("Authentication.SignIn.UsernamePlaceholder");

        public string SignInResetPasswordLinkText => TextTranslator.Translate("Authentication.SignIn.ResetPasswordLink")
                ;

        public IHtmlString ResetPasswordBody
                => new HtmlString(SiteRootContext.Item?.Forgot_Password_Text ?? string.Empty);

        public string ResetPasswordEmailPlaceholderText
                => TextTranslator.Translate("Authentication.ResetPassword.Request.EmailPlaceholder");

        public string ResetPasswordSuccessText
                => TextTranslator.Translate("Authentication.ResetPassword.Request.Success");

        public string ResetPasswordErrorEmailText
                => TextTranslator.Translate("Authentication.ResetPassword.Request.ErrorEmail");

        public string ResetPasswordErrorGeneralText
                => TextTranslator.Translate("Authentication.ResetPassword.Request.ErrorGeneral");

        public string ResetPasswordSubmitText => TextTranslator.Translate("Authentication.ResetPassword.Request.Submit")
                ;

        public IHtmlString ResetPasswordContactText
                => new HtmlString(SiteRootContext.Item?.Customer_Support_Text ?? string.Empty);

        public bool IsSignInFromMyView { get; set; }

        public string CurVerticalName => GlobalService.GetVerticalRootAncestor(Sitecore.Context.Item.ID.ToGuid())?._Name;

        public bool IsNewSalesforceEnabled => SalesforceConfigurationContext.IsNewSalesforceEnabled;
        public string AuthorizationRequestUrl => SalesforceConfigurationContext.GetLoginEndPoints(SiteRootContext?.Item?.Publication_Code, GetCallbackUrl("/User/ProcessUserRequest"), HttpContext.Current.Request.Url.ToString());
        public string SignInMessage => TextTranslator.Translate("Authentication.SignIn.SignInMessage");

        private string GetCallbackUrl(string url)
        {
            return $"{HttpContext.Current.Request.Url.Scheme}://{HttpContext.Current.Request.Url.Authority}{HttpContext.Current.Request.ApplicationPath.TrimEnd('/')}{url}";
        }
    }

}