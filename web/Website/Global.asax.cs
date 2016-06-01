using System;
using System.Web.Http;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.UI;
using Informa.Web.App_Start;
using Sitecore.Web;
using Autofac;
using System.Web;
using Sitecore.Configuration;
using StackExchange.Profiling;

namespace Informa.Web
{
	public class InformaApplication : Application //Sitecore.ContentSearch.SolrProvider.AutoFacIntegration.AutoFacApplication
	{
		protected void Application_Start(object sender, EventArgs args)
		{
			// Setup dependencies first
			AutofacConfig.Start();

			// Setup SOLR
			var containerBuilder = new ContainerBuilder();
			int timeout;
			int.TryParse(Settings.GetSetting("SolrConnectionTimeout", "200000"), out timeout);
			containerBuilder.RegisterType<SolrNet.Impl.SolrConnection>().WithParameter(new NamedParameter("Timeout", timeout));

			new CustomAutoFacSolrStartUp(new ContainerBuilder()).Initialize();

			// Code that runs on application startup
			GlobalConfiguration.Configure(WebApiConfig.Register);

			// Setup custom MVC routing for Sitecore
			RouteConfig.RegisterRoutes(RouteTable.Routes, areaNames: null); // Pass in MVC area names (if any, ex. 'Admin')

			EnableUnobtrusiveValidation();

			Library.DCD.XMLImporting.FileImportingManager mgr = new Library.DCD.XMLImporting.FileImportingManager();
			mgr.StartIfStartable();

			//AreaRegistration.RegisterAllAreas();
			//GlobalConfiguration.Configure(WebApiConfig.Register);
			//FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			//RouteConfig.RegisterRoutes(RouteTable.Routes);
		}

#if DEBUG
		protected void Application_BeginRequest()
		{
			if (Sitecore.Context.Item != null && !Sitecore.Context.PageMode.IsExperienceEditorEditing && !Sitecore.Context.PageMode.IsPreview)
			{
				MiniProfiler.Start();
			}
		}

		protected void Application_EndRequest()
		{
			MiniProfiler.Stop();
		}
#endif

		protected virtual void EnableUnobtrusiveValidation()
		{
			const string bundlePath = "~/js/jquery.js";
			ScriptManager.ScriptResourceMapping.AddDefinition("jquery", new ScriptResourceDefinition
			{
				Path = bundlePath,
				LoadSuccessExpression = "window.jQuery"
			});
		}

		protected void Application_PostAuthorizeRequest()
		{
			HttpContext.Current.SetSessionStateBehavior(System.Web.SessionState.SessionStateBehavior.Required);
		}
	}
}

/*
using System;
using System.Web.Http;
using System.Web.Routing;
using System.Web.UI;
using Autofac;
using Informa.Web.App_Start;
using Sitecore.Web;
using StackExchange.Profiling;

namespace Informa.Web
{
public class Global : Application
{
	 public void Application_Start(object sender, EventArgs args)
	 {
			 // Setup dependencies first
			 AutofacConfig.Start();

			 // Setup SOLR
			 //new CustomAutoFacSolrStartUp(new ContainerBuilder()).Initialize();

			 // Code that runs on application startup
			 GlobalConfiguration.Configure(WebApiConfig.Register);

			 // Setup custom MVC routing for Sitecore
			 RouteConfig.RegisterRoutes(RouteTable.Routes, areaNames: null); // Pass in MVC area names (if any, ex. 'Admin')

			 EnableUnobtrusiveValidation();
	 }

#if DEBUG
	 protected void Application_BeginRequest()
	 {
			 if (Sitecore.Context.Item != null && !Sitecore.Context.PageMode.IsPageEditorEditing && !Sitecore.Context.PageMode.IsPreview)
			 {
					 MiniProfiler.Start();
			 }
	 }

	 protected void Application_EndRequest()
	 {
			 MiniProfiler.Stop();
	 }
#endif

	 protected virtual void EnableUnobtrusiveValidation()
	 {
			 const string bundlePath = "~/js/jquery.js";
			 ScriptManager.ScriptResourceMapping.AddDefinition("jquery", new ScriptResourceDefinition
			 {
					 Path = bundlePath,
					 LoadSuccessExpression = "window.jQuery"
			 });
	 }
}
} */
