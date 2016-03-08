using System.Web.Mvc;
using Informa.Library.User.Entitlement;

namespace Informa.Web.Areas.Account.Controllers
{
    public class SpoofController : Controller
    {
        //TODO: Temporary hack for enabling entitlement
        [Authorize]
        public ActionResult SpoofEntitlement(string returnUrl)
        {
            Sitecore.Context.User.Profile.SetCustomProperty(nameof(Entitlement), "SCRIP");
            Sitecore.Context.User.Profile.Save();

            return Redirect(returnUrl);
        }
    }
}