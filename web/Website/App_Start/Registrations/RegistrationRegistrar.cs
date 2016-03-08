using Autofac;
using Informa.Library.User.Registration;
using Informa.Library.Salesforce.User.Registration;

namespace Informa.Web.App_Start.Registrations
{
	public class RegistrationRegistrar
	{
		public static void RegisterDependencies(ContainerBuilder builder)
		{
			builder.RegisterType<SalesforceRegisterUser>().As<IRegisterUser>();
		}
	}
}