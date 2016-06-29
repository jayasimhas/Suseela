using Informa.Library.ViewModels.Account;

namespace Informa.Library.User.EmailOptIns
{
    public class OptInResponseModel
    {
        public string BodyText { get; set; }
        public bool IsAuthenticated { get; set; }
        public ISignInViewModel SignInViewModel { get; set; }
        public string RedirectUrl { get; set; }
        public bool IsSuccessful { get; set; }
    }
}
