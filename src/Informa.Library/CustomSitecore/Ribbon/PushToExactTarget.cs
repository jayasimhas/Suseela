using Autofac;
using Glass.Mapper.Sc;
using Informa.Library.Mail.ExactTarget;
using Informa.Library.Utilities.CMSHelpers;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Emails;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Jabberwocky.Glass.Autofac.Util;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Web.UI.Sheer;
using System.Linq;

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
                var currentSiteRoot = SiteRootHelper.GetSiteRoot(item);
                var response = scope.Resolve<IExactTargetClient>().PushEmail(emailItem, currentSiteRoot);

                SheerResponse.Alert(response.Message);

                if (response.Success)
                {
                    var exactTargetConfig = currentSiteRoot.Fields[ISite_ConfigConstants.Exact_Target_ConfigFieldName].Value;
                    var ETConfig = currentSiteRoot.Database.GetItem(exactTargetConfig);
                    if (ETConfig != null)
                    {
                        Sitecore.Data.Fields.LinkField link = ETConfig.Fields
                            [IExactTarget_ConfigurationConstants.Exact_Target_Front_End_UrlFieldName];
                        SheerResponse.Eval($"window.open('{link.Url}');");
                    }
                    else
                        SheerResponse.Alert("Target Url is not valid");
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