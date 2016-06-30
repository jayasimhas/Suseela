using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Extras.Attributed;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Informa.Library.Caching;
using Informa.Web.App_Start.Registrations;
using Jabberwocky.Autofac.Modules;
using Jabberwocky.Glass.Autofac.Extensions;
using Jabberwocky.Glass.Autofac.Mvc.Extensions;
using log4net;
using Informa.Library.CustomSitecore.Mvc;
using Informa.Library.Utilities.Autofac.Modules;
using Informa.Web.Controllers;
using Informa.Web.Controllers.Search;
using Jabberwocky.Autofac.Extras.MiniProfiler;
using Jabberwocky.Core.Caching;
using Velir.Search.Autofac.Modules;

namespace Informa.Web.App_Start
{
	public static class AutofacConfig
	{
		private const string WebsiteDll = "Informa.Web";
		private const string LibraryDll = "Informa.Library";
		private const string ModelsDll = "Informa.Models";
		private const string VelirSearchDll = "Velir.Search.Core";

		private const string GlassMapperDll = "Glass.Mapper";
		private const string GlassMapperScDll = "Glass.Mapper.Sc";
		private const string GlassMapperMvcDll = "Glass.Mapper.Sc.Mvc";

		public static void Start()
		{
			var builder = new ContainerBuilder();

			// Auto-Wire
			builder.AutowireDependencies(assemblyNames: new[] { LibraryDll, WebsiteDll });

			builder.RegisterGlassServices();
			builder.RegisterCacheServices();
			builder.RegisterProcessors(new[] { WebsiteDll, LibraryDll });
			builder.RegisterGlassMvcServices(WebsiteDll, LibraryDll);
			builder.RegisterGlassFactory(LibraryDll, VelirSearchDll, ModelsDll);
			builder.RegisterApiControllers(Assembly.GetExecutingAssembly()).WithAttributeFilter();
			builder.RegisterControllers(Assembly.GetExecutingAssembly()).WithAttributeFilter();
			builder.RegisterType<CustomSitecoreHelper>().AsSelf();
			builder.RegisterType<ArticleUtil>().AsSelf();
			builder.RegisterType<SitecoreSaverUtil>().AsSelf();
			builder.RegisterType<EmailUtil>().AsSelf();

			//Velir Search Library
			builder.RegisterModule<SearchModule>();
			builder.RegisterModule<SolrSearchModule>();
			SearchRegistrar.RegisterDependencies(builder);

			SessionRegistrar.RegisterDependencies(builder);
			AuthenticationRegistrar.RegisterDependencies(builder);
			SalesforceRegistrar.RegisterDependencies(builder);
			UserRegistrar.RegisterDependencies(builder);
			RegistrationRegistrar.RegisterDependencies(builder);
			CompanyRegistrar.RegisterDependencies(builder);
			NlmRegistrar.RegisterDependencies(builder);
			ScheduledPublishingRegistrar.RegisterDependencies(builder);
			SiteDebuggingRegistrar.RegisterDependencies(builder);
			LoggingRegistrar.RegisterDependencies(builder);
			EntitlementsRegistrar.RegisterDependencies(builder);
			PurchaseRegistrar.RegisterDependencies(builder);
			DCDRegistrar.RegisterDependencies(builder);

			// Custom Modules
			builder.RegisterModule(new LogInjectionModule<ILog>(LogManager.GetLogger));
			builder.RegisterModule(new AutomapperModule(LibraryDll));

#if DEBUG
			builder.RegisterModule(new MiniProfilerModule(WebsiteDll, LibraryDll, VelirSearchDll, GlassMapperDll, GlassMapperScDll, GlassMapperMvcDll));
#endif

			// Custom Registrations
			CustomMvcRegistrar.RegisterDependencies(builder, WebsiteDll);
			GlassMapperRegistrar.RegisterDependencies(builder);
			builder.RegisterType<Library.Caching.CrossSiteCacheProvider>().As<Library.Caching.ICrossSiteCacheProvider>().SingleInstance();

			// This is necessary to 'seal' the container, and make it resolvable from the AutofacStartup.ServiceLocator singleton
			IContainer container = builder.Build();
			container.RegisterContainer();

			// Create the dependency resolver.
			var resolver = new AutofacWebApiDependencyResolver(container);

			// Configure Web API with the dependency resolver.
			GlobalConfiguration.Configuration.DependencyResolver = resolver;

			// Configure MVC with dependency resolver
			DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
		}

	}
}