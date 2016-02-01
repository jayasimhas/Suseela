using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using Glass.Mapper.Sc;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Tasks;
using Sitecore.Workflows;


namespace Informa.Web.Controllers
{
	public class WorkFlowUtil
	{
		private readonly ISitecoreService _sitecoreMasterService;
		public const string MasterDb = "master";
		protected SitecoreSaverUtil SitecoreUtil;
		protected ArticleUtil ArticleUtil;

		public WorkFlowUtil(Func<string, ISitecoreService> sitecoreFactory,SitecoreSaverUtil sitecoreSaverUtil, ArticleUtil articleUtil)
		{
			_sitecoreMasterService = sitecoreFactory(MasterDb);
			SitecoreUtil = sitecoreSaverUtil;
			ArticleUtil = articleUtil;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="i"></param>
		/// <param name="commandID">Optional: if included, the command will be executed</param>
		/// <returns>The workflow state of the item (after workflow command has executed, if given one)</returns>
		public WorkflowState ExecuteCommandAndGetWorkflowState(Item i, string commandID)
		{
			using (new Sitecore.SecurityModel.SecurityDisabler())
			{
				IWorkflow workflow = i.State.GetWorkflow();

				if (workflow == null)
				{
					//uh oh
					return new WorkflowState();
				}

				if (commandID != null)
				{
					//var oldWorkflow = workflow.WorkflowID;
					// This line will cause the workflow field to be set to null... sometimes
					workflow.Execute(commandID, i, "comments", false);
					//var info = new WorkflowInfo(oldWorkflow, i.Fields[Sitecore.FieldIDs.WorkflowState].Value);
					//i.Database.DataManager.SetWorkflowInfo(i, info);
					//i.Fields[Sitecore.FieldIDs.Workflow].Value = oldWorkflow;

				}
				var state = new WorkflowState();

				var currentState = workflow.GetState(i);
				state.DisplayName = currentState.DisplayName;
				state.IsFinal = currentState.FinalState;

				var commands = new List<WorkflowCommand>();
				foreach (Sitecore.Workflows.WorkflowCommand command in workflow.GetCommands(i))
				{
					var wfCommand = new WorkflowCommand();
					wfCommand.DisplayName = command.DisplayName;
					wfCommand.StringID = command.CommandID;

					CommandItem commandItem = SitecoreUtil.GetItemByGuid(new Guid(command.CommandID));

					StateItem stateItem = SitecoreUtil.GetItemByGuid(commandItem.NextState);

					if (stateItem == null)
					{
						_logger.Warn("WorkflowController.ExecuteCommandAndGetWorkflowState: Next state for command [" + command.CommandID + "] is null!");
					}
					else
					{
						wfCommand.SendsToFinal = stateItem.Final.Checked;
						wfCommand.GlobalNotifyList = new List<SitecoreUtil.StaffStruct>();

						foreach (StaffMemberItem x in stateItem.Staff.ListItems)
						{
							if (x.Inactive.Checked) { continue; }

							var staffMember = new SitecoreUtil.StaffStruct();
							staffMember.ID = x.ID.ToGuid();
							staffMember.Name = x.GetFullName();
							staffMember.Publications = x.Publications.ListItems.Select(p => p.ID.ToGuid()).ToArray();
							wfCommand.GlobalNotifyList.Add(staffMember);
						}

						commands.Add(wfCommand);
					}
				}

				state.Commands = commands;

				return state;
			}
		}

		public WorkflowState GetWorkflowState(Item i)
		{
			return this.ExecuteCommandAndGetWorkflowState(i, null);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="articleNumber"></param>
		/// <param name="commandID">Optional: if included, the command will be executed</param>
		/// <returns>The workflow state of the article (after workflow command has executed, if given one)</returns>
		public WorkflowState GetWorkflowState(string articleNumber, string commandID = null)
		{
			return ExecuteCommandAndGetWorkflowState(ArticleUtil.GetArticleItemByNumber(articleNumber), commandID);
		}

		public WorkflowState GetWorkflowState(Guid articleGuid, string commandID = null)
		{
			return ExecuteCommandAndGetWorkflowState(_sitecoreMasterService.GetItem<Item>(articleGuid), commandID);
		}

	
	}
}
