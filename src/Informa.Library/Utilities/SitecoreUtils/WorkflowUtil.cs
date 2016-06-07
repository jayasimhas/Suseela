
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Workflows;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Informa.Library.Utilities.SitecoreUtils
{
    public class WorkflowUtil
    {
        /// <summary>
		/// Gets the workflow history for the current item
		/// </summary>
		/// <returns>A list of all workflow events.</returns>
		/// <remarks>Because of versioning, there are duplicate item created workflow events. This method filters
		/// those duplicates out.</remarks>
		public List<Tuple<DateTime, string, bool>> GetWorkflowHistory(ID itemID)
        {
            var completeWorkflowHistory = new List<Tuple<DateTime, string, bool>>();
            try
            {
                var masterDb = Sitecore.Data.Database.GetDatabase(Informa.Library.Utilities.References.Constants.MasterDb);
                var currentItem = masterDb.GetItem(itemID);
                bool addedFirstEvent = false;
                // versions are in a 1-based array; if you give it "0", it will give you the most recent.
                for (int i = 1; i < currentItem.Versions.Count + 1; i++)
                {
                    Item thisVersion = currentItem.Versions[new Sitecore.Data.Version(i)];
                    IWorkflow workflow = masterDb.WorkflowProvider.GetWorkflow(thisVersion[FieldIDs.Workflow]);

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

                        completeWorkflowHistory.AddRange(events.Select(s => new Tuple<DateTime, string, bool>(s.Date, s.NewState, workflow.GetStates()[0].FinalState)));
                    }
                }
            }
            catch (Exception exception)
            {
                Sitecore.Diagnostics.Log.Error(exception.ToString(), this);
            }

            return completeWorkflowHistory;
        }

        //public Dictionary<DateTime, string> GetWorkflowHistoryLight(ID itemID)
        //{
        //    var history = GetWorkflowHistory(itemID);

        //    //Dictionary<DateTime, string> wfDic = new Dictionary<DateTime, string>();
        //    List<Tuple<DateTime, string, bool>> dataList = new List<Tuple<DateTime, string, bool>>();
        //    //Tuple<DateTime, string, bool> dataTuple = new Tuple<DateTime, string, bool>()
        //    foreach (var item in history)
        //    {
        //        dataList.Add(new Tuple<DateTime, string, bool>(item.date, item.NewState, state. ))
        //        wfDic.Add(item.Date, item.NewState);
        //    }

        //    //ArticleWorkflowCommand

        //    return wfDic;
        //}
    }
}
