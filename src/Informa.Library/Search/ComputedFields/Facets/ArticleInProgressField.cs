using System;
using Sitecore.Data.Items;
using Sitecore.Workflows;
using Velir.Search.Core.ComputedFields;
using Log = Sitecore.Diagnostics.Log;

namespace Informa.Library.Search.ComputedFields.Facets
{
    public class ArticleInProgressField : BaseContentComputedField
    {
        public override object GetFieldValue(Item indexItem)
        {
            try
            {
                ItemState itemState = indexItem.State;
                WorkflowState workflowState = itemState?.GetWorkflowState();
                if (workflowState != null)
                {
                    return !workflowState.FinalState;
                }
                return false;
            }
            catch (Exception exc)
            {
                Log.Error("There was an issue with getting the workflow state for: " + indexItem.Paths.Path + " : " + exc.Message, this);
                return false;
            }
        }
    }
}
