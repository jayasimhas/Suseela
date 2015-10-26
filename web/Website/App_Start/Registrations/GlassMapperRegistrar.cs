using Autofac;
using Glass.Mapper.Sc;

namespace Informa.Web.App_Start.Registrations
{
	public static class GlassMapperRegistrar
	{

		public static void RegisterDependencies(ContainerBuilder builder)
		{
			builder.Register(c => c.Resolve<ISitecoreService>(new TypedParameter(typeof(string), "master")))
				.Named<ISitecoreService>("master")
				.ExternallyOwned()
				.PreserveExistingDefaults();
			builder.Register(c => c.Resolve<ISitecoreService>(new TypedParameter(typeof(string), "web")))
				.Named<ISitecoreService>("web")
				.ExternallyOwned()
				.PreserveExistingDefaults();
		}

	}
}