using Autofac;
using Informa.Library.Session;

namespace Informa.Web.App_Start.Registrations
{
	public class SessionRegistrar
	{
		public static void RegisterDependencies(ContainerBuilder builder)
		{
			builder.RegisterType<SpecificSessionStores>().As<ISpecificSessionStores>();
		}
	}
}