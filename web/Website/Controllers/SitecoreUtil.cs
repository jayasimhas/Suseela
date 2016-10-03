using System;
using System.Web.Security;
using Informa.Library.Utilities.References;
using Informa.Web.Areas.Account.Models;
using PluginModels;
using Sitecore.Security.Authentication;
using Jabberwocky.Glass.Autofac.Util;
using Informa.Library.User.Authentication;
using Autofac;

namespace Informa.Web.Controllers
{
    public static class SitecoreUtil
    {
        /// <summary>
        /// This method Generates the Article Number
        /// </summary>
        /// <param name="lastArticleNumber"></param>
        /// <param name="publication"></param>		
        /// <returns></returns>
        public static string GetNextArticleNumber(long lastArticleNumber, Guid publication)
        {
            string number = GetPublicationPrefix(publication) + lastArticleNumber.ToString(Constants.ArticleNumberLength);
            return number;
        }

        public static string GetNextArticleNumber(long lastArticleNumber, Guid publication, string publicationPrefix)
        {
            string number = publicationPrefix + lastArticleNumber.ToString(Constants.ArticleNumberLength);
            return number;
        }

        /// <summary>
        /// This method gets the Publication Prefix which is used in Article Number Generation.
        /// </summary>
        /// <param name="publicationGuid"></param>
        /// <returns></returns>
        public static string GetPublicationPrefix(Guid publicationGuid)
        {
            string value;
            return Constants.PublicationPrefixDictionary.TryGetValue(publicationGuid, out value) ? value : string.Empty;
        }

        public static UserStatusStruct GetUserStatus(string username, string password)
        {
            var userStatus = new UserStatusStruct { UserName = username };

            MembershipUser user = Membership.GetUser(username);
            if (user == null)
            {
                var domainAndUser = Sitecore.Context.Domain + "\\" + username;
                user = Membership.GetUser(domainAndUser);
                if (user == null)
                {
                    userStatus.LoginSuccessful = false;
                    return userStatus;
                }
            }

            userStatus.LoginAttemptsRemaining = AttemptedPasswordAttemptsRemaining(user);
            userStatus.LockedOut = user.IsLockedOut;
            bool wasUserLockedOut = user.IsLockedOut;

            try
            {
                userStatus.LoginSuccessful = AuthenticationManager.Login(username, password);
            }
            catch (Exception e)
            { //regardless of exception, if the above code fails, the user is not authenticated
                userStatus.LoginSuccessful = false;
            }

            user = Membership.GetUser(user.UserName);
            if (user != null)
            {
                if (user.IsLockedOut && !wasUserLockedOut)
                {
                    ISendPluginUserLockedOutEmail emailSender;
                    using (var scope = AutofacConfig.ServiceLocator.BeginLifetimeScope())
                    {
                        emailSender = scope.Resolve<ISendPluginUserLockedOutEmail>();
                    }

                    emailSender.SendEmail(user);
                    //_lockedOutUserEmailer.UserBecameLockedOut(user);
                }
            }

            userStatus.LoginAttemptsRemaining = AttemptedPasswordAttemptsRemaining(user);
            userStatus.LockedOut = user.IsLockedOut;

            return userStatus;
        }

        public static int AttemptedPasswordAttemptsRemaining(MembershipUser user)
        {
            var membershipProvider = (CustomSqlMembershipProvider)Membership.Providers["sql"];
            if (membershipProvider == null)
            {
                return 0;
            }

            return membershipProvider.GetRemainingPasswordAttempts((Guid)user.ProviderUserKey);
        }

    }
}