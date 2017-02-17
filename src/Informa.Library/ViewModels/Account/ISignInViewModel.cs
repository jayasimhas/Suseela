using System.Web;

namespace Informa.Library.ViewModels.Account
{
    public interface ISignInViewModel
    {
        string SignInButtonText { get; }
        string SignInInvalidText { get; }
        string SignInPasswordPlaceholderText { get; }
        string SignInRememberMeText { get; }
        string SignInUsernamePlaceholderText { get; }
        string SignInResetPasswordLinkText { get; }
        IHtmlString ResetPasswordBody { get; }
        string ResetPasswordEmailPlaceholderText { get; }
        string ResetPasswordSuccessText { get; }
        string ResetPasswordErrorEmailText { get; }
        string ResetPasswordErrorGeneralText { get; }
        string ResetPasswordSubmitText { get; }
        IHtmlString ResetPasswordContactText { get; }
        bool IsSignInFromMyView { get; set; }
        string CurVerticalName { get; }
        bool IsNewSalesforceEnabled { get; }
        string SignInMessage { get; }
        string AuthorizationRequestUrl { get; }
    }
}
