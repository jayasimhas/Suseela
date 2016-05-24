using Autofac;
using Glass.Mapper.Sc;
using Informa.Library.Mail.ExactTarget;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Emails;
using Jabberwocky.Glass.Autofac.Util;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Web.UI.Sheer;

namespace Informa.Library.CustomSitecore.Ribbon
{
    public class PushToExactTarget : Command
    {
        public override void Execute(CommandContext context)
        {
            var item = context.Items[0];
            var emailItem = item.GlassCast<IExactTarget_Email>();

            using (var scope = AutofacConfig.ServiceLocator.BeginLifetimeScope())
            {
                var response = scope.Resolve<IExactTargetClient>().PushEmail(emailItem);

                SheerResponse.Alert(response.Message);

                if (response.Success)
                {
                    var targetUrl = scope.Resolve<IExactTargetWrapper>().EtFrontEndUrl;
                    SheerResponse.Eval($"window.open('{targetUrl}');");
                }
            }
        }

        public override CommandState QueryState(CommandContext context)
        {
            var item = context.Items[0];

            return item?.TemplateID == IExactTarget_EmailConstants.TemplateId
                ? CommandState.Enabled
                : CommandState.Hidden;
        }
    }
}