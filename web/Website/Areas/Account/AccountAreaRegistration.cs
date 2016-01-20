using System.Web.Mvc;
using System.Web.Http;

namespace Informa.Web.Areas.Account
{
    public class AccountAreaRegistration : AreaRegistration 
    {
        public override string AreaName => "Account";

        public override void RegisterArea(AreaRegistrationContext context) 
        {
			context.Routes.MapHttpRoute(
				"Account_api",
				"Account/api/{controller}/{action}/{id}",
				new { id = RouteParameter.Optional }
			);

			context.MapRoute(
                "Account_default",
                "Account/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}