using System;
using System.Web.Mvc;
using System.Web.Mvc.Async;
using Sitecore.Diagnostics;
using Sitecore.Mvc.Configuration;
using Sitecore.Pipelines;
using SCControllerFactory = Sitecore.Mvc.Controllers.SitecoreControllerFactory;

namespace Informa.Library.CustomSitecore.Pipelines
{
	public class InitializeControllerFactory
	{
		public virtual void Process(PipelineArgs args)
		{
            Sitecore.Diagnostics.Log.Info("Started InitializeControllerFactory", " InitializeControllerFactory ");
            this.SetControllerFactory(args);
            Sitecore.Diagnostics.Log.Info("Ended InitializeControllerFactory", " InitializeControllerFactory");
        }

		protected virtual void SetControllerFactory(PipelineArgs args)
		{
			ControllerBuilder.Current.SetControllerFactory((IControllerFactory)new SitecoreControllerFactory(ControllerBuilder.Current.GetControllerFactory()));
		}

		public class SitecoreControllerFactory : SCControllerFactory
		{
			public SitecoreControllerFactory(IControllerFactory innerFactory) : base(innerFactory)
			{
				Assert.ArgumentNotNull((object)innerFactory, "innerFactory");
				this.InnerFactory = innerFactory;
			}

			protected override void PrepareController(IController controller, string controllerName)
			{
				this.WrapActionInvoker(controller, controllerName);
			}

			private void WrapActionInvoker(IController controller, string controllerName)
			{
				if (!MvcSettings.DetailedErrorOnMissingAction)
					return;
				Controller controller1 = controller as Controller;
				if (controller1 == null)
					return;
				IActionInvoker actionInvoker = controller1.ActionInvoker;
				if (actionInvoker == null)
					return;
				controller1.ActionInvoker = (IActionInvoker)new SitecoreActionInvoker(actionInvoker, controllerName);
			}
		}

		public class SitecoreActionInvoker : IActionInvoker, IAsyncActionInvoker
		{
			public string ControllerName { get; protected set; }

			public IActionInvoker InnerInvoker { get; protected set; }

			public SitecoreActionInvoker(IActionInvoker innerInvoker, string controllerName)
			{
				this.InnerInvoker = innerInvoker;
				this.ControllerName = controllerName;
			}

			public bool InvokeAction(ControllerContext controllerContext, string actionName)
			{
				if (!this.InnerInvoker.InvokeAction(controllerContext, actionName))
				{
					string str = controllerContext.Controller.GetType().ToString();
					throw new InvalidOperationException(string.Format("Could not invoke action method: {0}. Controller name: {1}. Controller type: {2}", (object)actionName, (object)this.ControllerName, (object)str));
				}
				return true;
			}

			public IAsyncResult BeginInvokeAction(ControllerContext controllerContext, string actionName, AsyncCallback callback,
				object state)
			{
				var asyncInvoker = InnerInvoker as IAsyncActionInvoker;
				if (asyncInvoker == null)
				{
					string str = controllerContext.Controller.GetType().ToString();
					throw new InvalidOperationException(string.Format("Could not invoke action method: {0}. Controller name: {1}. Controller type: {2}", (object)actionName, (object)this.ControllerName, (object)str));
				}

				return asyncInvoker.BeginInvokeAction(controllerContext, actionName, callback, state);
			}

			public bool EndInvokeAction(IAsyncResult asyncResult)
			{
				var asyncInvoker = (IAsyncActionInvoker)InnerInvoker;

				return asyncInvoker.EndInvokeAction(asyncResult);
			}
		}
	}
}
