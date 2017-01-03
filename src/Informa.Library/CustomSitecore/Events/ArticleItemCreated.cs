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
using Informa.Models.Informa.Models.sitecore.templates.System.Workflow;

namespace Informa.Library.CustomSitecore.Events
{
	public class ArticleItemCreated
	{
		private ISitePublicationWorkflow publicationWorkflow
		{
			get
			{
				try
				{
					return DependencyResolver.Current.GetService<ISitePublicationWorkflow>();
				}
				catch
				{
					return null;
				}
			}
		}

		public void Process(object sender, EventArgs args)
		{

            if (publicationWorkflow == null)
                return;

            Assert.ArgumentNotNull(args, "args");

            ItemCreatedEventArgs icArgs = Event.ExtractParameter(args, 0) as ItemCreatedEventArgs;
            var item = icArgs.Item;

            if (item == null)
                return;
            try
			{
				

				//If article save process
				var itemPath = item.Paths.Path.ToLower();
				if (itemPath.StartsWith("/sitecore/content")
						&& !itemPath.Contains("/issues/") // detect if this is an issue clone, since the fields arent set yet on item created
						&& item.TemplateID.Guid == Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages.IArticleConstants.TemplateId.Guid)
				{
					changeWorkflowStateAndExecuteActions(item, new ID(publicationWorkflow.GetInitialState(item)._Id), new ID(publicationWorkflow.GetPublicationWorkflow(item)._Id));
				}
			}
			catch (Exception ex)
			{

                IState initialState = publicationWorkflow.GetInitialState(item);

                if (initialState != null)
                    changeWorkflowStateAndExecuteActions(item, new ID(initialState._Id), new ID(publicationWorkflow.GetPublicationWorkflow(item)._Id));

				Sitecore.Diagnostics.Log.Fatal(ex.ToString(), this);

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
