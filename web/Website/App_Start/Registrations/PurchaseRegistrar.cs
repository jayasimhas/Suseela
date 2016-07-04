using Autofac;
using Informa.Library.Purchase.User;
using Informa.Library.Salesforce.Purchase.User;

namespace Informa.Web.App_Start.Registrations
{
	public class PurchaseRegistrar
	{
		public static void RegisterDependencies(ContainerBuilder builder)
		{
			builder.RegisterType<SalesforceFindUserArticlePurchases>().As<IFindUserArticlePurchases>();
		}
	}
}