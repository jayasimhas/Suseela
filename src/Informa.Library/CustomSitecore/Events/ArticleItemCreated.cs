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
		private ISitePublicationWorkflow publicationWorkflow; 

		public ArticleItemCreated()
		{
			try
			{
				publicationWorkflow = DependencyResolver.Current.GetService<ISitePublicationWorkflow>();
			}
			catch { }
		}

		public void Process(object sender, EventArgs args)
		{
			try
			{
				if (publicationWorkflow == null)
					return;

				Assert.ArgumentNotNull(args, "args");

				ItemCreatedEventArgs icArgs = Event.ExtractParameter(args, 0) as ItemCreatedEventArgs;
				var item = icArgs.Item;

				if (item == null)
					return;

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
