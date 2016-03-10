using Autofac;
using Informa.Library.Company;
using Informa.Library.Salesforce.Company;

namespace Informa.Web.App_Start.Registrations
{
	public class CompanyRegistrar
	{
		public static void RegisterDependencies(ContainerBuilder builder)
		{
			builder.RegisterType<SalesforceFindCompanyByIpAddress>()
				.As<IFindCompanyByIpAddress>()
				.As<ISalesforceFindCompanyByIpAddress>();
		}
	}
}