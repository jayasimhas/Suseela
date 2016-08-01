using Informa.Library.Utilities.References;
using Sitecore.Events;
using System;

namespace Informa.Library.Publishing.Events
{
    public class BeginPublishItemProcessing
    {
        public void Process(object sender, EventArgs args)
        {
            SitecoreEventArgs sArgs = args as SitecoreEventArgs;
            Sitecore.Publishing.Publisher pbl = sArgs.Parameters[0] as Sitecore.Publishing.Publisher;

            //This prevents from publishing the whole site by mistake
            if (pbl.Options.Deep)
            {
                var rootItem = pbl.Options.RootItem;
                if (rootItem != null && (rootItem.TemplateID.Guid.Equals(new Guid(Constants.PublicationRootTemplateID))
                    ||
                    rootItem.TemplateID.Guid.Equals(new Guid(Constants.HomeRootTemplateID))
                    ))
                {
                    pbl.Options.Deep = false;
                }
            }
        }
    }
}
