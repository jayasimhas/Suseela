using Autofac;
using Informa.Library.SiteDebugging;

namespace Informa.Web.App_Start.Registrations
{
	public static class SiteDebuggingRegistrar
	{
		public static void RegisterDependencies(ContainerBuilder builder)
		{
			builder.RegisterType<SiteDebuggingAllowedConfiguration>().As<ISiteDebuggingAllowedConfiguration>();
			builder.RegisterType<SiteDebuggingAllowedContext>().As<ISiteDebuggingAllowedContext>();
			builder.RegisterType<SiteDebuggingSession>().As<ISiteDebuggingSession>();
		}
	}
}