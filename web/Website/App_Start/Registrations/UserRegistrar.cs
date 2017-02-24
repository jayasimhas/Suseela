using Autofac;
using Informa.Library.User;
using Informa.Library.User.ResetPassword.Web;
using Informa.Library.Salesforce.User;
using Informa.Library.Salesforce.User.Entitlement;
using Informa.Library.User.ResetPassword;
using Informa.Library.User.Profile;
using Informa.Library.User.Newsletter;
using Informa.Library.User.Offer;
using Informa.Library.User.Document;
using Informa.Library.Salesforce.User.Profile;
using Informa.Library.Salesforce.User.Newsletter;
using Informa.Library.Salesforce.User.Offer;
using Informa.Library.User.Entitlement;
using Informa.Library.Session;
using Informa.Library.Salesforce.Subscription.User;
using Informa.Library.Subscription.User;
using Informa.Library.Salesforce.User.UserPreferences;
using Informa.Library.User.UserPreference;
using Informa.Library.Salesforce.V2.User.Entitlement;
using Informa.Library.Salesforce.V2.User.Profile;
using Informa.Library.Salesforce.V2.ProductPreferences;
using Informa.Library.User.ProductPreferences;

namespace Informa.Web.App_Start.Registrations
{
    public class UserRegistrar
    {
        public static void RegisterDependencies(ContainerBuilder builder)
        {
            builder.RegisterType<UserSession>()
                .As<IUserSession>()
                .As<ISpecificSessionStore>();
            builder.RegisterType<EntitlementSession>()
                .As<IEntitlementSession>()
                .As<ISpecificSessionStore>();

            builder.RegisterType<SalesforceFindUserByEmail>().As<IFindUserByEmail>();
            builder.RegisterType<SalesforceUpdateUserPassword>().As<IUpdateUserPassword>();

            builder.RegisterType<WebGenerateUserResetPasswordActions>().As<IWebGenerateUserResetPasswordActions>();

            builder.RegisterType<Library.User.ResetPassword.Entity.EntityUserResetPasswordContextFactory>().As<Library.User.ResetPassword.Entity.IEntityUserResetPasswordContextFactory>();
            builder.RegisterType<Library.User.ResetPassword.Entity.EntityFindUserResetPassword>().As<IFindUserResetPassword>();
            builder.RegisterType<Library.User.ResetPassword.Entity.EntityStoreUserResetPassword>().As<IStoreUserResetPassword>();

            builder.RegisterType<SalesforceFindUserProfile>().As<IUserProfileFactory>();
            builder.RegisterType<SalesforceFindUserProfile>().As<IFindUserProfileByUsername>();
            builder.RegisterType<SalesforceFindUserProfileV2>().As<IFindUserProfileByUsernameV2>();

            builder.RegisterType<SalesforceUpdateOfferUserOptIn>().As<IUpdateOfferUserOptIn>();
            builder.RegisterType<SalesforceUpdateNewsletterUserOptIns>().As<IUpdateNewsletterUserOptIns>();

            builder.RegisterType<SalesforceFindNewsletterUserOptIns>().As<IFindNewsletterUserOptIns>();
            builder.RegisterType<SalesforceOfferUserOptedIn>().As<IOfferUserOptedIn>();
            builder.RegisterType<SalesforceGetUserEntitlements>().As<IGetUserEntitlements>();
            builder.RegisterType<SalesforceGetIPEntitlements>().As<IGetIPEntitlements>();

            builder.RegisterType<SalesforceSavedDocuments>()
                .As<IFindSavedDocuments>()
                .As<ISaveDocument>()
                .As<IRemoveDocument>();
            builder.RegisterType<SalesforceFindUserSubscriptions>().As<IFindUserSubscriptions>();

            builder.RegisterType<SalesforceManageAccountInfo>().As<IManageAccountInfo>();
            builder.RegisterType<SalesforceManageAccountInfoV2>().As<IManageAccountInfoV2>();
            builder.RegisterType<SalesforceUserProfile>().As<ISalesforceUserProfile>();
            builder.RegisterType<SalesforceFindUserProfile>().As<ISalesforceFindUserProfile>();
            builder.RegisterType<SalesforceFindUserProfileV2>().As<ISalesforceFindUserProfileV2>();

            builder.RegisterType<SalesforceFindUserPreferences>().As<IFindUserPreferences>();
            builder.RegisterType<SalesforceSetuserPreferences>().As<ISetUserPreferences>();

            builder.RegisterType<SalesforceGetUserEntitlementsV2>().As<IGetUserEntitlementsV2>();
            builder.RegisterType<SalesforceGetUserEntitlementsV2>().As<ISalesforceGetUserEntitlementsV2>();

            builder.RegisterType<SalesforceAddUserProductPreference>().As<ISalesforceAddUserProductPreference>();
            builder.RegisterType<SalesforceAddUserProductPreference>().As<IAddUserProductPreference>();
            builder.RegisterType<SalesforceUpdateUserProductPreference>().As<ISalesforceUpdateUserProductPreference>();
            builder.RegisterType<SalesforceUpdateUserProductPreference>().As<IUpdateUserProductPreference>();
            builder.RegisterType<SalesforceGetUserProductPreferences>().As<ISalesforceGetUserProductPreferences>();
            builder.RegisterType<SalesforceGetUserProductPreferences>().As<IGetUserProductPreferences>();
            builder.RegisterType<SalesforceDeleteUserProductPreferences>().As<ISalesforceDeleteUserProductPreferences>();
            builder.RegisterType<SalesforceDeleteUserProductPreferences>().As<IDeleteUserProductPreferences>();
        }
    }
}
