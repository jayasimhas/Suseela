using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Sc;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Shell.Applications.ContentEditor;
using Sitecore.Workflows;
using Velir.Search.Core.ComputedFields;
using DateTime = System.DateTime;

namespace Informa.Library.Search.ComputedFields.Facets
{
    public class ArticleInProgressField : BaseContentComputedField
    {
        public override object GetFieldValue(Item indexItem)
        {
            Database masterDatabase = Factory.GetDatabase("master");

            try
            {
                IWorkflow workflow = masterDatabase.WorkflowProvider.GetWorkflow(indexItem);
                WorkflowState state = workflow.GetState(indexItem);
                return !state.FinalState;
            }
            catch (Exception exc)
            {
                Log.Error("There was an issue with getting the workflow state for: " + indexItem.Paths.Path + " : " + exc.Message, this);
                return false;
            }
        }
    }
}
