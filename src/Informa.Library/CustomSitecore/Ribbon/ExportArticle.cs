﻿using System;
using System.Collections.Specialized;
using System.Threading;
using Autofac;
using Glass.Mapper.Sc;
using Informa.Library.Services.NlmExport;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Util;
using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Web.UI.Sheer;

using InformaConstants = Informa.Library.Utilities.References.Constants;

namespace Informa.Library.CustomSitecore.Ribbon
{
    [Serializable]
    public class ExportArticle : Command
    {
        protected void Run(ClientPipelineArgs args)
        {
            Item[] items = DeserializeItems(args.Parameters["items"]);
            Item publicationNodeItem = items[0];

            if (!args.IsPostBack)
            {
                SheerResponse.Confirm("You are about to export this article as NLM. Do you want to proceed?");
                args.WaitForPostBack();
            }
            else if (args.Result == "yes")
            {
                //bool forDelete = false;

                try
                {
                    using (var scope = AutofacConfig.ServiceLocator.BeginLifetimeScope())
                    {
                        var exportService = scope.Resolve<INlmExportService>();
                        var serviceFactory = scope.Resolve<Func<string, ISitecoreService>>();
                        var service = serviceFactory(InformaConstants.MasterDb);

                        // Generate NLM XML, and retrieve the validation result
                        var result = exportService.ExportNlm(service.GetItem<ArticleItem>(publicationNodeItem.ID.Guid), ExportType.Manual);
                        if (!result.ExportSuccessful)
                        {
                            // NLM Export failed
                            SheerResponse.Alert(
                                "The article did not pass NLM validation and has not been exported. Please check the fields and try again.");
                                return;
                        }

                        SheerResponse.Alert("The article passed NLM validation and exported successfully.");
                    }
                }
                catch (Exception ex) when (!(ex is ThreadAbortException))
                {
                    SheerResponse.Alert("The article did not pass NLM validation and has not been exported. Please check the fields and try again.");
                }
            }
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
    }
}
