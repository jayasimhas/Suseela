using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;

namespace Informa.Library.CustomSitecore.Mvc
{
	public class AreaControllerFactory : IControllerFactory
	{
		private readonly HashSet<string> _areaNames;
		private readonly IControllerFactory _fallbackControllerFactory;

		public AreaControllerFactory(IControllerFactory fallbackControllerFactory, params string[] areaNames)
		{
			areaNames = areaNames ?? new string[0];

			_fallbackControllerFactory = fallbackControllerFactory;
			_areaNames = new HashSet<string>(areaNames, StringComparer.InvariantCultureIgnoreCase);
		}

		public bool CanHandle(RequestContext requestContext)
		{
			if (requestContext.RouteData.DataTokens.ContainsKey("area"))
				return _areaNames.Contains(requestContext.RouteData.DataTokens["area"].ToString());
			return false;
		}

		public IController CreateController(RequestContext requestContext, string controllerName)
		{
			if (!CanHandle(requestContext))
				return _fallbackControllerFactory.CreateController(requestContext, controllerName);

			var fullName = ((string[]) requestContext.RouteData.DataTokens["namespaces"])[0].TrimEnd('*') + "Controllers." + controllerName + "Controller";
			var controllerType = Type.GetType(fullName, false, true);

			// If we're still unable to create a controller, fallback to the default implementation
			return controllerType == null
				? _fallbackControllerFactory.CreateController(requestContext, controllerName)
                : (IController)DependencyResolver.Current.GetService(controllerType);
		}

		public SessionStateBehavior GetControllerSessionBehavior(RequestContext requestContext, string controllerName)
		{
			return SessionStateBehavior.Default;
		}

		public void ReleaseController(IController controller)
		{
			IDisposable disposable = controller as IDisposable;
			if (disposable == null)
				return;
			disposable.Dispose();
		}
	}
}