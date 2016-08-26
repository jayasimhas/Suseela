using Autofac;
using Informa.Library.User.Authentication;
using Jabberwocky.Glass.Autofac.Util;
using Sitecore.Pipelines.PasswordRecovery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Library.CustomSitecore.Pipelines
{
    public class PasswordRecoveryPipeline : PasswordRecoveryProcessor
    {
        ISendRecoveryPassword sendRecovery;

        public PasswordRecoveryPipeline()
        {
            using (var scope = AutofacConfig.ServiceLocator.BeginLifetimeScope())
            {
                sendRecovery = scope.Resolve<ISendRecoveryPassword>();
            }
        }

        public override void Process(PasswordRecoveryArgs args)
        {
            args.SendFromDisplayName = " My Sitecore site";
            args.SendFromEmail = "donotreply@mysitecoresite.net";
            args.Subject = "Password recovery";
            args.HtmlEmailContent = GetEmailContent(args);

            sendRecovery.SendEmail(args.Username, args.UserEmail, args.Password);
        }

        private string GetEmailContent(PasswordRecoveryArgs args)
        {
            var sb = new StringBuilder();

            sb.AppendLine("Your password has been reset");
            sb.AppendLine("Hi" + args.Username);
            sb.AppendLine("Your new password is  " + args.Password);

            return sb.ToString();
        }

    }
}
