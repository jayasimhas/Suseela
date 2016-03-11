using Autofac;
using Informa.Library.User;
using Informa.Library.User.ResetPassword.Web;
using Informa.Library.Salesforce.User;
using Informa.Library.Salesforce.User.Entitlement;
using Informa.Library.User.ResetPassword.MongoDB;
using Informa.Library.User.ResetPassword;
using Informa.Library.User.Profile;
using Informa.Library.Salesforce.User.Profile;
using Informa.Library.User.Entitlement;
using Informa.Library.User.Registration.Web;

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

			builder.RegisterType<WebRegisterUserActions>().As<IWebRegisterUserActions>();

			builder.RegisterType<SalesforceUpdateOfferUserOptIn>().As<IUpdateOfferUserOptIn>();
			builder.RegisterType<SalesforceUpdateNewsletterUserOptIn>().As<IUpdateNewsletterUserOptIn>();

			builder.RegisterType<SalesforceQueryNewsletterUserOptIn>().As<IQueryNewsletterUserOptIn>();
            builder.RegisterType<SalesforceQueryOfferUserOptIn>().As<IQueryOfferUserOptIn>();
		    builder.RegisterType<SalesforceGetUserEntitlements>().As<IGetUserEntitlements>();      
		    builder.RegisterType<SalesforceGetIPEntitlements>().As<IGetIPEntitlements>();

		    builder.RegisterType<SalesforceManageSavedDocuments>().As<IManageSavedDocuments>();
		}
	}
}