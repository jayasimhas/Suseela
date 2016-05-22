using Informa.Library.Globalization;
using Informa.Library.User.Authentication;
using Informa.Library.User.Profile;
using Informa.Library.User.ResetPassword;
using Informa.Library.User.ResetPassword.Web;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using System.Web;
using System.Web.Mvc;

namespace Informa.Web.Areas.Account.ViewModels.ResetPassword
{
    public class ChangeViewModel : GlassViewModel<I___BasePage>
    {
        protected readonly IAuthenticatedUserContext AuthenticatedUserContext;
        protected readonly ITextTranslator TextTranslator;
        protected readonly IFindUserProfileByUsername FindUserProfile;
        protected readonly IFindUserResetPassword FindUserResetPassword;
        protected readonly IWebUserResetPasswordTokenContext TokenContext;

        public ChangeViewModel(
            IAuthenticatedUserContext authenticatedUserContext,
            ITextTranslator textTranslator,
            IFindUserProfileByUsername findUserProfile,
            IFindUserResetPassword findUserResetPassword,
            IWebUserResetPasswordTokenContext tokenContext)
        {
            AuthenticatedUserContext = authenticatedUserContext;
            TextTranslator = textTranslator;
            FindUserProfile = findUserProfile;
            FindUserResetPassword = findUserResetPassword;
            TokenContext = tokenContext;
        }

        protected IUserResetPassword UserResetPassword
        {
            get
            {
                return FindUserResetPassword.Find(Token);
            }
        }

        public string Title => GlassModel?.Title;
        public IHtmlString ResetBody
        {
            get
            {
                var userProfile = FindUserProfile.Find(UserResetPassword?.Username ?? string.Empty);
                var body = GlassModel?.Body ?? string.Empty;

                if (userProfile != null && !string.IsNullOrWhiteSpace(body))
                {
                    body = body.Replace("#first_name#", userProfile.FirstName).Replace("#last_name#", userProfile.LastName);
                }

                return new MvcHtmlString(body);
            }
        }
        public string NewPasswordLabelText => TextTranslator.Translate("Authentication.ResetPassword.Change.NewPasswordLabel");
        public string NewPasswordPlaceholderText => TextTranslator.Translate("Authentication.ResetPassword.Change.NewPasswordPlaceholder");
        public string NewPasswordRepeatLabelText => TextTranslator.Translate("Authentication.ResetPassword.Change.NewPasswordRepeatLabel");
        public string NewPasswordRepeatPlaceholderText => TextTranslator.Translate("Authentication.ResetPassword.Change.NewPasswordRepeatPlaceholder");
        public string SubmitButtonText => TextTranslator.Translate("Authentication.ResetPassword.Change.Submit");
        public string SendNewLinkButtonText => TextTranslator.Translate("Authentication.ResetPassword.Change.SendNewLink");
        public string ResetErrorRequirementsText => TextTranslator.Translate("Authentication.ResetPassword.Change.ResetErrorRequirements");
        public string ResetErrorMismatchText => TextTranslator.Translate("Authentication.ResetPassword.Change.ResetErrorMismatch");
        public string ResetErrorGeneralText => TextTranslator.Translate("Authentication.ResetPassword.Change.ResetErrorGeneral");
        public string ResetSuccessText => TextTranslator.Translate("Authentication.ResetPassword.Change.ResetSuccess");
        public string RetryBody => TextTranslator.Translate("Authentication.ResetPassword.Change.RetryBody");
        public string RetryErrorGeneralText => TextTranslator.Translate("Authentication.ResetPassword.Change.RetryErrorGeneral");
        public string RetrySuccessText => TextTranslator.Translate("Authentication.ResetPassword.Change.RetrySuccess");
        public string TokenNotFoundBody => TextTranslator.Translate("Authentication.ResetPassword.Change.TokenNotFound");
        public bool IsValidToken => UserResetPassword.IsValid();
        public bool TokenFound => UserResetPassword != null;
        public string Token => TokenContext.Token;
        public bool IsAuthenticated => AuthenticatedUserContext.IsAuthenticated;
    }
}