using System;
using System.Collections.Specialized;
using System.Threading;
using System.Web;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using log4net;
using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Web.UI.Sheer;

namespace Informa.Library.CustomSitecore.Ribbon
{
    [Serializable]
    public class ViewNlm : Command
    {
        private const string ViewNlmDialogPath = "/sitecore/client/Your Apps/View NLM/Dialogs/View NLM";

        private static readonly ILog Logger = LogManager.GetLogger(typeof(ExportArticle));

        protected void Run(ClientPipelineArgs args)
        {
            Item[] items = DeserializeItems(args.Parameters["items"]);
            Item publicationNodeItem = items[0];

            try
            {
                var id = HttpUtility.UrlEncode(publicationNodeItem.ID.ToString());

                SheerResponse.ShowModalDialog(new ModalDialogOptions(ViewNlmDialogPath + $"?id={id}"));
            }
            catch (Exception ex) when (!(ex is ThreadAbortException))
            {
                Logger.Error("Could not export article.", ex);
                SheerResponse.Alert("There was an error during the export process. Please try again.");
            }
        }

        public override void Execute(CommandContext context)
        {
            Assert.ArgumentNotNull(context, "context");
            Item[] items = context.Items;
            if (items.Length == 1)
            {
                NameValueCollection parameters = new NameValueCollection
                {
                    ["items"] = SerializeItems(context.Items)
                };
                Context.ClientPage.Start(this, "Run", parameters);
            }
        }

        public override CommandState QueryState(CommandContext context)
        {
            Item item = context.Items[0];

            return item?.TemplateID == IArticleConstants.TemplateId
                ? CommandState.Enabled
                : CommandState.Hidden;
        }
    }
}
