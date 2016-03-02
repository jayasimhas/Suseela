using Autofac;
using Informa.Library.User;
using Informa.Library.User.ResetPassword.Web;
using Informa.Library.Salesforce.User;
using Informa.Library.User.ResetPassword.MongoDB;
using Informa.Library.User.ResetPassword;
using Informa.Library.User.Profile;
using Informa.Library.Salesforce.User.Profile;

namespace Informa.Web.App_Start.Registrations
{
	public class UserRegistrar
	{
		public static void RegisterDependencies(ContainerBuilder builder)
		{
			builder.RegisterType<SalesforceFindUserByEmail>().As<IFindUserByEmail>();
			builder.RegisterType<SalesforceUpdateUserPassword>().As<IUpdateUserPassword>();

			builder.RegisterType<WebGenerateUserResetPasswordActions>().As<IWebGenerateUserResetPasswordActions>();
			builder.RegisterType<MongoDbFindUserResetPassword>().As<IFindUserResetPassword>();

			builder.RegisterType<SalesforceFindUserProfile>().As<IUserProfileFactory>();
			builder.RegisterType<SalesforceFindUserProfile>().As<IFindUserProfileByUsername>();
		}
	}
}