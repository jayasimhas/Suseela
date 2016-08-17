using Informa.Library.Publication;
using Jabberwocky.Glass.Autofac.Util;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Diagnostics.PerformanceCounters;
using Sitecore.Events;
using Sitecore.Pipelines;
using Sitecore.Workflows;
using Sitecore.Workflows.Simple;
using System;
using Autofac;
using Glass.Mapper.Sc;
using Jabberwocky.Core.Caching;
using Informa.Library.Utilities.Extensions;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using System.Web.Mvc;

namespace Informa.Library.CustomSitecore.Events
{
    public class ArticleItemSave
    {
        private ISitePublicationWorkflow publicationWorkflow => DependencyResolver.Current.GetService<ISitePublicationWorkflow>();

        public void Process(object sender, EventArgs args)
        {
            Assert.ArgumentNotNull(args, "args");

            Item item = Event.ExtractParameter(args, 0) as Item;

            //If article save process
            if (item.TemplateID.Guid == Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages.IArticleConstants.TemplateId.Guid)
            {
                changeWorkflowStateAndExecuteActions(item, new ID(publicationWorkflow.GetInitialState(item)._Id), new ID(publicationWorkflow.GetPublicationWorkflow(item)._Id));
            }
        }

        private static WorkflowResult changeWorkflowStateAndExecuteActions(Item item, ID workflowStateId, ID workflowID)
        {
            if (item[FieldIDs.DefaultWorkflow] != workflowID.ToString())
            {
                using (new EditContext(item))
                {
                    item[FieldIDs.DefaultWorkflow] = workflowID.ToString();
                }
            }

            return new WorkflowResult(true, "OK", workflowStateId);
        }
    }
}
