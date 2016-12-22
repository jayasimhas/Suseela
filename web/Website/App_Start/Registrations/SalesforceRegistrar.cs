using Autofac;
using Informa.Library.Salesforce;
using Informa.Library.Salesforce.Company;
using Informa.Library.Salesforce.User;
using Informa.Library.Salesforce.User.Entitlement;
using Informa.Library.Salesforce.User.Orders;
using Informa.Library.Salesforce.User.Profile;
using Informa.Library.Salesforce.User.Registration;
using Informa.Library.Salesforce.User.Search;
using Informa.Library.Salesforce.V2;
using Informa.Library.Salesforce.Web;
using Informa.Library.User.Content;
using Informa.Library.User.Orders;
using Informa.Library.User.Search;

namespace Informa.Web.App_Start.Registrations
{
	public class SalesforceRegistrar
	{
		public static void RegisterDependencies(ContainerBuilder builder)
		{
			builder.RegisterType<SalesforceErrorLogger>().As<ISalesforceErrorLogger>();
			builder.RegisterType<SalesforceDebugLogger>().As<ISalesforceDebugLogger>();
			builder.RegisterType<SalesforceServiceConfiguration>().As<ISalesforceServiceConfiguration>().SingleInstance();
			builder.RegisterType<SalesforceService>().As<ISalesforceService>();
			builder.RegisterType<SalesforceServiceContextEnabledChecks>().As<ISalesforceServiceContextEnabledChecks>().InstancePerLifetimeScope();
			builder.RegisterType<SalesforceServiceContextEnabled>().As<ISalesforceServiceContextEnabled>().InstancePerLifetimeScope();
			builder.RegisterType<UserAgentServiceContextEnabledCheckConfiguration>().As<IUserAgentServiceContextEnabledCheckConfiguration>().SingleInstance();
			builder.RegisterType<UserAgentServiceContextEnabledCheck>().As<ISalesforceServiceContextEnabledCheck>().InstancePerLifetimeScope();
			builder.RegisterType<SalesforceServiceContext>().As<ISalesforceServiceContext>();
			builder.RegisterType<SalesforceSessionContext>().As<ISalesforceSessionContext>().SingleInstance();
			builder.RegisterType<SalesforceSessionFactory>().As<ISalesforceSessionFactory>();
			builder.RegisterType<SalesforceSessionFactoryConfiguration>().As<ISalesforceSessionFactoryConfiguration>();
			builder.RegisterType<SalesforceSetUserTemporaryPassword>().As<ISalesforceSetUserTemporaryPassword>();
			builder.RegisterType<SalesforceFindUserProfile>().As<ISalesforceFindUserProfile>();
			builder.RegisterType<SalesforceRegisterUser>().As<ISalesforceRegisterUser>();
			builder.RegisterType<SalesforceGetUserEntitlements>().As<ISalesforceGetUserEntitlements>();
			builder.RegisterType<SalesforceGetIPEntitlements>().As<ISalesforceGetIPEntitlements>();
			builder.RegisterType<SalesforceSiteTypeParser>()
				.As<ISalesforceCompanyTypeFromSiteType>()
				.As<ISalesforceSiteTypeFromCompanyType>()
				.As<ISalesforceCompanyTypeFromAccountType>();
			builder.RegisterType<SalesforceEntitlmentFactory>().As<ISalesforceEntitlmentFactory>();
			builder.RegisterType<SalesforceSavedSearchRepository>().As<IUserContentRepository<ISavedSearchEntity>>();
            builder.RegisterType<SalesforceUserOrder>().As<IUserOrder>();
            builder.RegisterType<HttpClientHelper>().As<IHttpClientHelper>();
        }
	}
}