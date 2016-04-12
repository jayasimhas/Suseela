using System.Web.Mvc;
using Glass.Mapper.Sc.Fields;
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
        Link SubscribeLink { get; }
        bool IsAuthenticated { get; }
    }
}