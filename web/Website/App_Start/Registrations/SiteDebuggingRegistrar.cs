using Autofac;
using Informa.Library.Session;
using Informa.Library.SiteDebugging;
using Jabberwocky.Autofac.Extras.MiniProfiler;

namespace Informa.Web.App_Start.Registrations
{
	public static class SiteDebuggingRegistrar
	{
		public static void RegisterDependencies(ContainerBuilder builder)
		{
			builder.RegisterType<SiteDebuggingAllowedConfiguration>().As<ISiteDebuggingAllowedConfiguration>();
			builder.RegisterType<SiteDebuggingAllowedContext>().As<ISiteDebuggingAllowedContext>();
			builder.RegisterType<SiteDebuggingSession>()
				.As<ISiteDebuggingSession>()
				.As<ISpecificSessionStore>();

#if DEBUG
            builder.RegisterModule(new MiniProfilerModule("Informa.Web", "Informa.Library", "Informa.Library.Salesforce"));
#endif
        }
    }
}