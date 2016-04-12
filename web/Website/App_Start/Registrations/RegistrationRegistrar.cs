using Autofac;
using Informa.Library.User.Registration;
using Informa.Library.Company;
using Informa.Library.User.Registration.Web;
using Informa.Library.Session;

namespace Informa.Web.App_Start.Registrations
{
	public class RegistrationRegistrar
	{
		public static void RegisterDependencies(ContainerBuilder builder)
		{
			builder.RegisterType<WebRegisterUserSession>()
				.As<IWebRegisterUserSession>()
				.As<ISpecificSessionStore>();
			builder.RegisterType<CompanyRegisterUser>().As<IRegisterUser>();
			builder.RegisterType<WebRegisterUserActions>().As<IWebRegisterUserActions>();
		}
	}
}