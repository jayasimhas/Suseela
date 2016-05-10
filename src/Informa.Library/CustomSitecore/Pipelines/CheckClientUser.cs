using Autofac;
using Jabberwocky.Glass.Autofac.Util;
using Sitecore.Pipelines.LoggingIn;
using System.Web.Security;
using Informa.Library.User.Authentication;
using Informa.Library.Utilities.UserMembership;

namespace Informa.Library.CustomSitecore.Pipelines
{
    public class CheckClientUser
    {

        public void Process(LoggingInArgs args)
        {
            //Assert.ArgumentNotNull(args, "args");

            MembershipUser usert = Membership.GetUser(args.Username);

            int number = UserAttemp.AttemptedPasswordAttemptsRemaining(usert);

            if (number == 1)
            {
                if (!usert.IsLockedOut)
                {
                    ISendUserLockedOutEmail emailSender;
                    using (var scope = AutofacConfig.ServiceLocator.BeginLifetimeScope())
                    {
                        emailSender = scope.Resolve<ISendUserLockedOutEmail>();
                    }

                    emailSender.SendEmail(usert);
                }
            }


        }
    }
}
