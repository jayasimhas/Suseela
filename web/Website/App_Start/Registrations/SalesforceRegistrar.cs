﻿using Autofac;
using Informa.Library.Salesforce;
using Informa.Library.Salesforce.Company;
using Informa.Library.Salesforce.User;
using Informa.Library.Salesforce.User.Entitlement;
using Informa.Library.Salesforce.User.Profile;
using Informa.Library.Salesforce.User.Registration;
using Informa.Library.Salesforce.User.Search;
using Informa.Library.Salesforce.Web;
using Informa.Library.User;
using Informa.Library.User.Search;

namespace Informa.Web.App_Start.Registrations
{
	public class SalesforceRegistrar
	{
		public static void RegisterDependencies(ContainerBuilder builder)
		{
			builder.RegisterType<SalesforceErrorLogger>().As<ISalesforceErrorLogger>();
			builder.RegisterType<SalesforceServiceConfiguration>().As<ISalesforceServiceConfiguration>();
			builder.RegisterType<SalesforceService>().As<ISalesforceService>();
			builder.RegisterType<SalesforceServiceContextEnabledChecks>().As<ISalesforceServiceContextEnabledChecks>();
			builder.RegisterType<SalesforceServiceContextEnabled>().As<ISalesforceServiceContextEnabled>();
			builder.RegisterType<UserAgentServiceContextEnabledCheckConfiguration>().As<IUserAgentServiceContextEnabledCheckConfiguration>().SingleInstance();
			builder.RegisterType<UserAgentServiceContextEnabledCheck>().As<ISalesforceServiceContextEnabledCheck>();
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
			builder.RegisterType<SavedSearchUserContext>().As<ISavedSearchUserContext>();
		}
	}
}