using Informa.Library.Publication;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Events;
using Sitecore.Workflows;
using System;
using System.Web.Mvc;
using Sitecore.Data.Events;

namespace Informa.Library.CustomSitecore.Events
{
    public class ArticleItemCreated
    {
        private ISitePublicationWorkflow publicationWorkflow => DependencyResolver.Current.GetService<ISitePublicationWorkflow>();

        public void Process(object sender, EventArgs args)
        {
            Assert.ArgumentNotNull(args, "args");

            ItemCreatedEventArgs icArgs = Event.ExtractParameter(args, 0) as ItemCreatedEventArgs;
            var item = icArgs.Item;

            //If article save process
            if (System.Web.HttpContext.Current != null && item.TemplateID.Guid == Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages.IArticleConstants.TemplateId.Guid)
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
