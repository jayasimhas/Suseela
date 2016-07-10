using System.Runtime.Caching;
using Autofac;
using Informa.Library.Article.Companies;
using Informa.Model.DCD;
using Jabberwocky.Core.Caching;

namespace Informa.Web.App_Start.Registrations
{
	public class DCDRegistrar
	{
		public static void RegisterDependencies(ContainerBuilder builder)
		{
			builder.Register(c => new GeneralCache(new MemoryCache("DcdCache"))).Named<ICacheProvider>("dcdCache").WithMetadata("name", "dcd").SingleInstance();
			builder.RegisterType<RelatedCompaniesService>().As<IRelatedCompaniesService>().InstancePerLifetimeScope();
			builder.RegisterType<RelatedDealsService>().As<IRelatedDealsService>().InstancePerLifetimeScope();

			builder.RegisterType<DCDManager>().Named<IDCDReader>("dcdReaderImplementor").WithMetadata("name", "provider");
			builder.RegisterDecorator<IDCDReader>(
				(c, provider) => new DCDReaderCacheDecorator(provider, c.ResolveNamed<ICacheProvider>("dcdCache")),
				"dcdReaderImplementor");
		}
	}
}