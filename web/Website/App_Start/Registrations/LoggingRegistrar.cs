using Autofac;
using Informa.Library.Logging;

namespace Informa.Web.App_Start.Registrations
{
	public static class LoggingRegistrar
	{
		public static void RegisterDependencies(ContainerBuilder builder)
		{
			builder.RegisterType<SitecoreErrorLogger>().As<IErrorLogger>();
            builder.RegisterType<SitecoreDebugLogger>().As<IDebugLogger>();
            builder.RegisterType<SitecoreInfoLogger>().As<IInfoLogger>();
        }
	}
}