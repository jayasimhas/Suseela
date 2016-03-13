using Autofac;
using Informa.Library.User.Registration;
using Informa.Library.Company;
using Informa.Library.User.Registration.Web;

namespace Informa.Web.App_Start.Registrations
{
	public class RegistrationRegistrar
	{
		public static void RegisterDependencies(ContainerBuilder builder)
		{
			builder.RegisterType<CompanyRegisterUser>().As<IRegisterUser>();
			builder.RegisterType<WebRegisterUserActions>().As<IWebRegisterUserActions>();
		}
	}
}