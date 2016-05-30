using System.Web.Mvc;
using Informa.Library.User.Newsletter.EmailOptIns;
using Informa.Library.Utilities.References;
using Informa.Library.Utilities.Security;
using Jabberwocky.Autofac.Attributes;

namespace Informa.Web.Areas.Account.Controllers
{
    public class SubscriptionPageController : Controller
    {
        private readonly IDependencies _dependencies;

        [AutowireService(true)]
        public interface IDependencies
        {
            IOptInManager OptInManager { get; }
            ICrypto Crypto { get; }
        }

        public SubscriptionPageController(IDependencies dependencies)
        {
            _dependencies = dependencies;
        }

        public ActionResult Subscribe(string Pub)
        {
            const string oneClickView = "~/Areas/Account/Views/Management/OneClickUnsubscribe.cshtml";
            var responseModel = _dependencies.OptInManager.OptIn(Pub);

            if (!string.IsNullOrEmpty(responseModel.RedirectUrl))
            {
                Redirect(responseModel.RedirectUrl);
            }

            return View(oneClickView, responseModel);
        }

        public ActionResult Unsubscribe(string User, string Type, string Pub)
        {
            const string oneClickView = "~/Areas/Account/Views/Management/OneClickUnsubscribe.cshtml";

            var responseModel = _dependencies.OptInManager.OptOut(User, Type, Pub);

            return View(oneClickView, responseModel);
        }

        public ActionResult SafeUnsubscribe(string Token)
        {
            var decrypted = _dependencies.Crypto.DecryptStringAes(Token, Constants.CryptoKey);
            

            return null;
        }
    }
}