using Autofac;
using Informa.Library.User.Authentication;
using Informa.Library.Salesforce.User.Authentication;

namespace Informa.Web.App_Start.Registrations
{
	public class AuthenticationRegistrar
	{
		public static void RegisterDependencies(ContainerBuilder builder)
		{
			builder.RegisterType<SalesforceAuthenticateUser>().As<IAuthenticateUser>();
		}
	}
}