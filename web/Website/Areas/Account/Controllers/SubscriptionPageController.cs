using System.Web.Mvc;
using Informa.Library.User.EmailOptIns;
using Informa.Library.Utilities.Security;
using Jabberwocky.Autofac.Attributes;

namespace Informa.Web.Areas.Account.Controllers
{
    public class SubscriptionPageController : Controller
    {
        private readonly IDependencies _dependencies;
        private const string SubscribeViewPath = "~/Areas/Account/Views/Management/OneClickSubscribe.cshtml";
        private const string UnsubscribeViewPath = "~/Areas/Account/Views/Management/OneClickUnsubscribe.cshtml";

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
            var responseModel = _dependencies.OptInManager.OptIn(Pub);

            if (!string.IsNullOrEmpty(responseModel.RedirectUrl))
            {
                Redirect(responseModel.RedirectUrl);
            }

            return View(SubscribeViewPath, responseModel);
        }

        public ActionResult Unsubscribe(string User, string Type, string Pub)
        {
            var responseModel = _dependencies.OptInManager.OptOut(User, Type, Pub);

            return View(UnsubscribeViewPath, responseModel);
        }

        public ActionResult AnonnymousUnsubscribe(string Token)
        {
            var responseModel = _dependencies.OptInManager.AnnonymousOptOut(Token);

            return View(UnsubscribeViewPath, responseModel);
        }
    }
}