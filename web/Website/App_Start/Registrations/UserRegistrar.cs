using Autofac;
using Informa.Library.User;
using Informa.Library.User.ResetPassword.Web;
using Informa.Library.Salesforce.User;
using Informa.Library.Salesforce.User.Entitlement;
using Informa.Library.User.ResetPassword;
using Informa.Library.User.Profile;
using Informa.Library.Salesforce.User.Profile;
using Informa.Library.User.Entitlement;

namespace Informa.Web.App_Start.Registrations
{
	public class UserRegistrar
	{
		public static void RegisterDependencies(ContainerBuilder builder)
		{
			builder.RegisterType<UserSession>().As<IUserSession>();
			builder.RegisterType<EntitlementSession>().As<IEntitlementSession>();

			builder.RegisterType<SalesforceFindUserByEmail>().As<IFindUserByEmail>();
			builder.RegisterType<SalesforceUpdateUserPassword>().As<IUpdateUserPassword>();

			builder.RegisterType<WebGenerateUserResetPasswordActions>().As<IWebGenerateUserResetPasswordActions>();

			builder.RegisterType<Library.User.ResetPassword.MongoDB.MongoDbUserResetPasswordConfiguration>().As<Library.User.ResetPassword.MongoDB.IMongoDbUserResetPasswordConfiguration>();
			builder.RegisterType<Library.User.ResetPassword.MongoDB.MongoDbUserResetPasswordContext>().As<Library.User.ResetPassword.MongoDB.IMongoDbUserResetPasswordContext>();
			builder.RegisterType<Library.User.ResetPassword.MongoDB.UserResetPasswordDocumentFactory>().As<Library.User.ResetPassword.MongoDB.IUserResetPasswordDocumentFactory>();
			builder.RegisterType<Library.User.ResetPassword.MongoDB.MongoDbFindUserResetPassword>().As<IFindUserResetPassword>();
			builder.RegisterType<Library.User.ResetPassword.MongoDB.MongoDbStoreUserResetPassword>().As<IStoreUserResetPassword>();

			//builder.RegisterType<Library.User.ResetPassword.Entity.EntityUserResetPasswordContextFactory>().As<Library.User.ResetPassword.Entity.IEntityUserResetPasswordContextFactory>();
			//builder.RegisterType<Library.User.ResetPassword.Entity.EntityFindUserResetPassword>().As<IFindUserResetPassword>();
			//builder.RegisterType<Library.User.ResetPassword.Entity.EntityStoreUserResetPassword>().As<IStoreUserResetPassword>();

			builder.RegisterType<SalesforceFindUserProfile>().As<IUserProfileFactory>();
			builder.RegisterType<SalesforceFindUserProfile>().As<IFindUserProfileByUsername>();

			builder.RegisterType<SalesforceUpdateOfferUserOptIn>().As<IUpdateOfferUserOptIn>();
			builder.RegisterType<SalesforceUpdateNewsletterUserOptIn>().As<IUpdateNewsletterUserOptIn>();

			builder.RegisterType<SalesforceQueryNewsletterUserOptIn>().As<IQueryNewsletterUserOptIn>();
            builder.RegisterType<SalesforceQueryOfferUserOptIn>().As<IQueryOfferUserOptIn>();
		    builder.RegisterType<SalesforceGetUserEntitlements>().As<IGetUserEntitlements>();      
		    builder.RegisterType<SalesforceGetIPEntitlements>().As<IGetIPEntitlements>();

		    builder.RegisterType<SalesforceManageSavedDocuments>().As<IManageSavedDocuments>();
            builder.RegisterType<SalesforceManageSubscriptions>().As<IManageSubscriptions>();
            builder.RegisterType<SalesforceManageAccountInfo>().As<IManageAccountInfo>();
            builder.RegisterType<SalesforceUserProfile>().As<ISalesforceUserProfile>();
            builder.RegisterType<SalesforceFindUserProfile>().As<ISalesforceFindUserProfile>();
        }
	}
}