using System.Web.Mvc;
using System.Web.Routing;
using Informa.Library.CustomSitecore.Mvc;

namespace Informa.Web
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes, params string[] areaNames)
		{
			routes.MapRoute(
				"addIssue",
				"vwb/{controller}/{action}/{id}",
				new {controller = "AddIssue", action = "Get", id = UrlParameter.Optional }
			);

			// Setup a custom AreaControllerFactory
			var innerFactory = ControllerBuilder.Current.GetControllerFactory();
			var delegatedAreaFactory = new AreaControllerFactory(innerFactory, areaNames);
            ControllerBuilder.Current.SetControllerFactory(delegatedAreaFactory);
        }
	}
}
