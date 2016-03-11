using Autofac;
using Informa.Library.Company;
using Informa.Library.Salesforce.Company;
using Informa.Library.Salesforce.User.Registration;

namespace Informa.Web.App_Start.Registrations
{
	public class CompanyRegistrar
	{
		public static void RegisterDependencies(ContainerBuilder builder)
		{
			builder.RegisterType<SalesforceFindCompanyByIpAddress>()
				.As<IFindCompanyByIpAddress>()
				.As<ISalesforceFindCompanyByIpAddress>();
			builder.RegisterType<SalesforceFindCompanyByUser>()
				.As<IFindCompanyByUser>()
				.As<ISalesforceFindCompanyByUser>();
			builder.RegisterType<SalesforceRegisterUser>().As<IRegisterCompanyUser>();
		}
	}
}