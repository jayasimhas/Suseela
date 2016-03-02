using Autofac;
using Informa.Library.Salesforce;
using Informa.Library.Salesforce.User;
using Informa.Library.Salesforce.User.Profile;

namespace Informa.Web.App_Start.Registrations
{
	public class SalesforceRegistrar
	{
		public static void RegisterDependencies(ContainerBuilder builder)
		{
			builder.RegisterType<SalesforceService>().As<ISalesforceService>();
			builder.RegisterType<SalesforceServiceContext>().As<ISalesforceServiceContext>();
			builder.RegisterType<SalesforceSessionContext>().As<ISalesforceSessionContext>().SingleInstance();
			builder.RegisterType<SalesforceSessionFactory>().As<ISalesforceSessionFactory>();
			builder.RegisterType<SalesforceSessionFactoryConfiguration>().As<ISalesforceSessionFactoryConfiguration>();
			builder.RegisterType<SalesforceSetUserTemporaryPassword>().As<ISalesforceSetUserTemporaryPassword>();
			builder.RegisterType<SalesforceFindUserProfile>().As<ISalesforceFindUserProfile>();
		}
	}
}