using System.Web.Mvc;
using System.Web.Routing;
using Informa.Library.CustomSitecore.Mvc;

namespace Informa.Web
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes, params string[] areaNames)
		{
			// Setup a custom AreaControllerFactory
			var innerFactory = ControllerBuilder.Current.GetControllerFactory();
			var delegatedAreaFactory = new AreaControllerFactory(innerFactory, areaNames);
            ControllerBuilder.Current.SetControllerFactory(delegatedAreaFactory);
        }
	}
}
