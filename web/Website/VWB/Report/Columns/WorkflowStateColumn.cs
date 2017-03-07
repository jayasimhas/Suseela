using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Elsevier.Library.CustomItems.Publication.General;
using Informa.Web.Controllers;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Social.Infrastructure.Utils;
using Sitecore.Workflows;

namespace Elsevier.Web.VWB.Report.Columns
{
    public class WorkflowStateColumn : IVwbColumn
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
            Database masterDb = Factory.GetDatabase("master");

            ArticleItem article = articleItemWrapper.InnerItem;
            Item item = article.InnerItem;
            var cell = new TableCell();
            string text = "";
            ItemState itemState = item.State;
            if (itemState == null)
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
            if (workflowState.FinalState)
            {
                //TODO I took this from EmailUtil, need to refactor a bit so that code is not in two places
                List<WorkflowEvent> history = GetWorkflowHistory(masterDb, item);

                if (history.Count == 0)
                {
                    cell.Text = "N/E";
                    return cell;
                }
                else
                {
                    WorkflowEvent latest = history.Last();
                    text += "<br />Signed off: " + latest.Date.ToServerTimeZone();
                    string user = latest.User;
                    text += "<br />By: " + user.Substring(user.LastIndexOf(@"\") + 1);
                }
            }
            cell.Text = text;
            return cell;
        }

        //TODO I took this from EmailUtil, need to refactor a bit so that code is not in two places
        public List<WorkflowEvent> GetWorkflowHistory(Database db, Item currentItem)
        {
            var completeWorkflowHistory = new List<WorkflowEvent>();
            try
            {
                bool addedFirstEvent = false;
                // versions are in a 1-based array; if you give it "0", it will give you the most recent.
                for (int i = 1; i < currentItem.Versions.Count + 1; i++)
                {
                    Item thisVersion = currentItem.Versions[new Sitecore.Data.Version(i)];
                    IWorkflow workflow = db.WorkflowProvider.GetWorkflow(thisVersion[FieldIDs.Workflow]);

                    if (workflow != null)
                    {
                        List<WorkflowEvent> events = workflow.GetHistory(thisVersion).ToList();

                        if (addedFirstEvent)
                        {
                            WorkflowState firstState = workflow.GetStates()[0];
                            events.RemoveAll(e => e.OldState == "" && e.NewState == firstState.StateID);
                            addedFirstEvent = true;
                        }
                        addedFirstEvent = true;

                        completeWorkflowHistory.AddRange(events);
                    }
                }
            }
            catch (Exception exception)
            {
                throw;
            }
            return completeWorkflowHistory;
        }

        public Dictionary<string, string> GetDropDownValues(List<ArticleItemWrapper> results)
        {
            Dictionary<string, string> dictWorkflowStates = new Dictionary<string, string>();
            dictWorkflowStates.Add("0", "Select");
            foreach (var result in results)
            {
                ArticleItem article = result.InnerItem;
                Item item = article.InnerItem;
                ItemState itemState = item.State;
                if (itemState == null)
                {
                    if (!dictWorkflowStates.ContainsValue("N/E"))
                        dictWorkflowStates.Add("N/E", "N/E");
                }
                WorkflowState workflowState = itemState.GetWorkflowState();
                if (workflowState == null)
                {
                    if (!dictWorkflowStates.ContainsValue("N/E"))
                        dictWorkflowStates.Add("N/E", "N/E");
                }
                else
                {
                    if (!dictWorkflowStates.ContainsValue(workflowState.DisplayName))
                        dictWorkflowStates.Add(workflowState.DisplayName, workflowState.DisplayName);
                }
            }
            return dictWorkflowStates;
        }
    }
}