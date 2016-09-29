using Informa.Models.Informa.Models.sitecore.templates.System.Workflow;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Library.Publication
{
    public interface ISitePublicationWorkflow
    {
        IWorkflow GetPublicationWorkflow(Item item);
        IState GetInitialState(Item item);
        IState GetFinalState(Item item);
        IState GetEditAfterPublishState(Item item);
    }
}
