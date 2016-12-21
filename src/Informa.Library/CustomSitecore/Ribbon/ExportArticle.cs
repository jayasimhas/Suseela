using System;
using System.Collections.Specialized;
using System.Threading;
using Autofac;
using Glass.Mapper.Sc;
using Informa.Library.Services.NlmExport;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Util;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Web.UI.Sheer;

using InformaConstants = Informa.Library.Utilities.References.Constants;
using System.Web;

namespace Informa.Library.CustomSitecore.Ribbon
{
    [Serializable]
    public class ExportArticle : Command
    {
        private const string ExportDialogPath = "/sitecore/client/Your Apps/NLM Export/Dialogs/Export Article";
        private const string Ecorrected = "ecorrected";
        private const string Eretracted = "eretracted";

        private static readonly ILog Logger = LogManager.GetLogger(typeof (ExportArticle));

        protected void Run(ClientPipelineArgs args)
        {
            Item[] items = DeserializeItems(args.Parameters["items"]);
            Item publicationNodeItem = items[0];

            if (!args.IsPostBack)
            {
                 var id = HttpUtility.UrlEncode(publicationNodeItem.ID.ToString());
                HttpContext.Current.Items["nlmArticle"] = id;
                SheerResponse.ShowModalDialog(new ModalDialogOptions(ExportDialogPath)
                {
                    Response = true
                });
                args.WaitForPostBack();
            }
            else if (args.Result != null)
            {
                HttpContext.Current.Items["nlmArticle"] = publicationNodeItem.ID.ToString();
                var dialogResult = JsonConvert.DeserializeObject<ExportDialogResult>(args.Result, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver()});
                if (dialogResult == null)
                {
                    return;
                }

                var forDelete = dialogResult.Delete;
                var pubType = PublicationType.Epub;

                switch (dialogResult.Pubtype)
                {
                    case Ecorrected:
                        pubType = PublicationType.Ecorrected;
                        break;
                    case Eretracted:
                        pubType = PublicationType.Eretracted;
                        break;
                }

                try
                {
                    using (var scope = AutofacConfig.ServiceLocator.BeginLifetimeScope())
                    {
                        var exportService = scope.Resolve<INlmExportService>();
                        var serviceFactory = scope.Resolve<Func<string, ISitecoreService>>();
                        var service = serviceFactory(InformaConstants.MasterDb);

                        var articleItem = service.GetItem<ArticleItem>(publicationNodeItem.ID.Guid);

                        if (forDelete)
                        {
                            DeleteNlm(exportService, articleItem);
                        }
                        else
                        {
                            ExportNlm(exportService, articleItem, pubType);
                        }
                    }
                }
                catch (Exception ex) when (!(ex is ThreadAbortException))
                {
                    Logger.Error("Could not export article.", ex);
                    SheerResponse.Alert("There was an error during the export process. Please try again.");
                }
            }
        }

        private static void DeleteNlm(INlmExportService exportService, ArticleItem articleItem)
        {
            if (!exportService.DeleteNlm(articleItem))
            {
                // NLM Delete failed
                SheerResponse.Alert("The deletion XML file could not be generated.");
                return;
            }

            SheerResponse.Alert("The article has been successfully exported for deletion.");
        }

        private static void ExportNlm(INlmExportService exportService, ArticleItem articleItem, PublicationType pubType)
        {
            // Generate NLM XML, and retrieve the validation result
            var result = exportService.ExportNlm(articleItem, ExportType.Manual, pubType);
            if (!result.ExportSuccessful)
            {
                // NLM Export failed
                SheerResponse.Alert(
                    "The article did not pass NLM validation and has not been exported. Please check the fields and try again.");
                return;
            }

            SheerResponse.Alert("The article passed NLM validation and exported successfully.");
        }


        public override void Execute(CommandContext context)
        {
            Assert.ArgumentNotNull(context, "context");
            Item[] items = context.Items;
            if (items.Length == 1)
            {
                NameValueCollection parameters = new NameValueCollection();
                parameters["items"] = SerializeItems(context.Items);
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

        public class ExportDialogResult
        {
            public bool Delete { get; set; }
            public string Pubtype { get; set; }
        }
    }
}
