using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Autofac;
using Autofac.Extras.Attributed;
using Autofac.Integration.Mvc;
using Informa.Library.CustomSitecore.Mvc;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using NWebsec.Mvc.HttpHeaders;

namespace Informa.Web.App_Start.Registrations
{
	public static class CustomMvcRegistrar
	{
		/// <summary>
  /// Overrides the registrations provided by Jabberwocky for the GlassViewModel... Enables support for Keyed Service Attributes
  /// </summary>
  /// <param name="builder"></param>
  /// <param name="assemblyNames"></param>
		public static void RegisterDependencies(ContainerBuilder builder, params string[] assemblyNames)
		{
			// Register a custom HtmlHelper
			builder.RegisterType<CustomSitecoreHelper>().AsSelf();
			
			// Allows for property injection in Views/Partials (but NOT layouts)
			builder.RegisterSource(new ViewRegistrationSource());

			builder.RegisterAssemblyTypes(assemblyNames.Select(Assembly.Load).ToArray()).AsClosedTypesOf(typeof(GlassViewModel<>)).AsSelf().WithAttributeFilter();

			builder.Register(c => new SetNoCacheHttpHeadersAttribute()).AsActionFilterFor<Controller>();
			builder.RegisterFilterProvider();
		}
	}
}