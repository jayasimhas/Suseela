using System.Web.Mvc;
using Glass.Mapper.Sc.Fields;
using Informa.Library.ViewModels.Account;
using Informa.Web.ViewModels.PopOuts;

namespace Informa.Web.ViewModels
{
    public interface ICallToActionViewModel
    {
        ISignInViewModel SignInViewModel { get; }
        IRegisterPopOutViewModel RegisterPopOutViewModel { get; }
        string SigninTitle { get; }
        string SigninSubtitle { get; }
        string RegisterTitle { get; }
        string RegisterSubtitle { get; }
        string SubscribeTitle { get; }
        string SubscribeLinkUrl { get; }
        string SubscribeLinkText { get; }
        bool IsAuthenticated { get; }
        bool IsNewSalesforceEnabled { get; }
        string RegistrationUrl { get; }
        string AuthorizationRequestUrl { get; }
    }
}