using System.Web.Mvc;
using System.Web.Routing;
//using Informa.Web.Areas.Admin;
using Jabberwocky.Glass.Autofac.Pipelines.Processors;
using Sitecore.Configuration;
using Sitecore.Pipelines;

namespace Informa.Web.CustomMvc.Pipelines
{
	public class RegisterMvcRoutes : ProcessorBase<PipelineArgs>
	{
		protected override void Run(PipelineArgs pipelineArgs)
		{
			var isProduction = Settings.GetBoolSetting("Env.IsProduction", false);
			if (isProduction) return;
			
			// Only register Admin area if we're NOT in production (front-ends)
			//var adminRegistration = new AdminAreaRegistration();
			//RegisterArea(adminRegistration);
			
			// Add other Area registrations here if needed
		}

		private static void RegisterArea(AreaRegistration areaRegistration)
		{
            Sitecore.Diagnostics.Log.Info("Started RegisterArea", " RegisterArea ");
            var context = new AreaRegistrationContext(areaRegistration.AreaName, RouteTable.Routes);

			string @namespace = areaRegistration.GetType().Namespace;
			if (@namespace != null)
			{
				context.Namespaces.Add(@namespace + ".*");
			}

			areaRegistration.RegisterArea(context);
            Sitecore.Diagnostics.Log.Info("Ended RegisterArea", " RegisterArea");
        }
	}
}
           