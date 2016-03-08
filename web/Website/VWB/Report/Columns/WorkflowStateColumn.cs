using System.Linq;
using System.Web.UI.WebControls;
using Elsevier.Library.CustomItems.Publication.General;
using Sitecore.Data.Items;
using Sitecore.Workflows;

namespace Elsevier.Web.VWB.Report.Columns
{
	public class WorkflowStateColumn :IVwbColumn
	{
		public int Compare(ArticleItemWrapper x, ArticleItemWrapper y)
		{
			return GetStateIndex(x.InnerItem.InnerItem).CompareTo(GetStateIndex(y.InnerItem.InnerItem));
		}

		/// <summary>
		/// Gets index of workflow state of item in its workflow
		/// </summary>
		/// <param name="item"></param>
		private static int GetStateIndex(Item item)
		{
			ItemState itemState = item.State;
			WorkflowState workflowState = itemState.GetWorkflowState();
			WorkflowState[] workflowStates = itemState.GetWorkflow().GetStates();
			int stateIndex = workflowStates.TakeWhile(state => state.StateID != workflowState.StateID).Count();
			return stateIndex;
		}

		public string GetHeader()
		{
			return "Workflow State";
		}

		public string Key()
		{
			return "wsc";
		}

		public TableCell GetCell(ArticleItemWrapper articleItemWrapper)
		{
			ArticleItem article = articleItemWrapper.InnerItem;
			Item item = article.InnerItem;
			var cell = new TableCell();
			string text = "";
			ItemState itemState = item.State;
			if(itemState == null)
			{
				cell.Text = "N/E";
				return cell;
			}
			WorkflowState workflowState = itemState.GetWorkflowState();
			if (workflowState == null)
			{
				cell.Text = "N/E";
				return cell;
			}
			text += workflowState.DisplayName;
			if(workflowState.FinalState)
			{
				var history = article.GetWorkflowHistory();
				WorkflowEvent latest = history.Last();
				text += "<br />Signed off: " + latest.Date.ToString();
				string user = latest.User;
				text += "<br />By: " + user.Substring(user.LastIndexOf(@"\")+1);
			}
			cell.Text = text;
			return cell;
		}
	}
}