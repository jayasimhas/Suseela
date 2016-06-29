using Autofac;
using Informa.Library.User.Entitlement;

namespace Informa.Web.App_Start.Registrations
{
	public class EntitlementsRegistrar
	{
		public static void RegisterDependencies(ContainerBuilder builder)
		{
			builder.RegisterType<EntitlementsContexts>().As<IEntitlementsContexts>();
			builder.RegisterType<EntitlementAccessContexts>().As<IEntitlementAccessContexts>();
		}
	}
}