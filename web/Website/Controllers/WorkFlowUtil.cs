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
using Informa.Models.Informa.Models.sitecore.templates.System.Workflow;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Jabberwocky.Glass.Autofac.Attributes;
using PluginModels;
using Sitecore.Common;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Tasks;
using Sitecore.Workflows;
using IWorkflow = Sitecore.Workflows.IWorkflow;
using WorkflowCommand = Sitecore.Workflows.WorkflowCommand;


namespace Informa.Web.Controllers
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public interface IWorkFlowUtil
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="i"></param>
		/// <param name="commandID">Optional: if included, the command will be executed</param>
		/// <returns>The workflow state of the item (after workflow command has executed, if given one)</returns>
		ArticleWorkflowState ExecuteCommandAndGetWorkflowState(Item i, string commandID);

		ArticleWorkflowState GetWorkflowState(Item i);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="articleNumber"></param>
		/// <param name="commandID">Optional: if included, the command will be executed</param>
		/// <returns>The workflow state of the article (after workflow command has executed, if given one)</returns>
		ArticleWorkflowState GetWorkflowState(string articleNumber, string commandID = null);

		ArticleWorkflowState GetWorkflowState(Guid articleGuid, string commandID = null);
	}

	public class WorkFlowUtil : IWorkFlowUtil
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
		public ArticleWorkflowState ExecuteCommandAndGetWorkflowState(Item i, string commandID)
		{
			using (new Sitecore.SecurityModel.SecurityDisabler())
			{
				IWorkflow workflow = i.State.GetWorkflow();

				if (workflow == null)
				{
					//uh oh
					return new ArticleWorkflowState();
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
				var state = new ArticleWorkflowState();

				var currentState = workflow.GetState(i);
				state.DisplayName = currentState.DisplayName;
				state.IsFinal = currentState.FinalState;

				var commands = new List<ArticleWorkflowCommand>();
				foreach (Sitecore.Workflows.WorkflowCommand command in workflow.GetCommands(i))
				{
					var wfCommand = new PluginModels.ArticleWorkflowCommand();
					wfCommand.DisplayName = command.DisplayName;
					wfCommand.StringID = command.CommandID;

					ICommand commandItem = _sitecoreMasterService.GetItem<ICommand>(new Guid(command.CommandID));

					IState stateItem = _sitecoreMasterService.GetItem<IState>(commandItem.Next_State);

					if (stateItem == null)
					{
						//_logger.Warn("WorkflowController.ExecuteCommandAndGetWorkflowState: Next state for command [" + command.CommandID + "] is null!");
					}
					else
					{
						wfCommand.SendsToFinal = stateItem.Final;
						wfCommand.GlobalNotifyList = new List<StaffStruct>();

						//foreach (IStaff_Item x in stateItem.Staff.ListItems)
						//{
						//	if (x.Inactive.Checked) { continue; }

						//	var staffMember = new SitecoreUtil.StaffStruct();
						//	staffMember.ID = x.ID.ToGuid();
						//	staffMember.Name = x.GetFullName();
						//	staffMember.Publications = x.Publications.ListItems.Select(p => p.ID.ToGuid()).ToArray();
						//	wfCommand.GlobalNotifyList.Add(staffMember);
						//}

						commands.Add(wfCommand);
					}
				}

				state.Commands = commands;

				return state;
			}
		}

		
		public ArticleWorkflowState GetWorkflowState(Item i)
		{
			return this.ExecuteCommandAndGetWorkflowState(i, null);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="articleNumber"></param>
		/// <param name="commandID">Optional: if included, the command will be executed</param>
		/// <returns>The workflow state of the article (after workflow command has executed, if given one)</returns>
		public ArticleWorkflowState GetWorkflowState(string articleNumber, string commandID = null)
		{
			return ExecuteCommandAndGetWorkflowState(ArticleUtil.GetArticleItemByNumber(articleNumber), commandID);
		}

		public ArticleWorkflowState GetWorkflowState(Guid articleGuid, string commandID = null)
		{
			return ExecuteCommandAndGetWorkflowState(_sitecoreMasterService.GetItem<Item>(articleGuid), commandID);
		}

	
	}
}
