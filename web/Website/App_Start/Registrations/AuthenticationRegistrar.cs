using Autofac;
using Informa.Library.User.Authentication;
using Informa.Library.Salesforce.User.Authentication;
using Informa.Library.User.Authentication.Web;

namespace Informa.Web.App_Start.Registrations
{
	public class AuthenticationRegistrar
	{
		public static void RegisterDependencies(ContainerBuilder builder)
		{
			builder.RegisterType<SalesforceAuthenticateUser>().As<IAuthenticateUser>();
			builder.RegisterType<WebLoginUserActions>().As<IWebLoginUserActions>();
		}
	}
}