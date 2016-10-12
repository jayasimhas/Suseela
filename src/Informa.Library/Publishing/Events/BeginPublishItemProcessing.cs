using Glass.Mapper.Sc;
using Informa.Library.Utilities.References;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Jabberwocky.Autofac.Attributes;
using Jabberwocky.Glass.Autofac.Util;
using Sitecore.Events;
using System;
using Autofac;

namespace Informa.Library.Publishing.Events
{
    [AutowireService]
    public class BeginPublishItemProcessing
    {
        private Func<string, ISitecoreService> sitecoreServiceFactory { get { return AutofacConfig.ServiceLocator.Resolve<Func<string, ISitecoreService>>(); } }
        private ISitecoreService sitecoreService { get; set; }
        private ICustom_Publishing_Config customPublishConfig { get; set; }

        public BeginPublishItemProcessing()
        {
            sitecoreService = sitecoreServiceFactory("master");
        }

        public void Process(object sender, EventArgs args)
        {
            SitecoreEventArgs sArgs = args as SitecoreEventArgs;
            Sitecore.Publishing.Publisher pbl = sArgs.Parameters[0] as Sitecore.Publishing.Publisher;

            //This prevents from publishing the whole site by mistake
            if (pbl.Options.Deep)
            {
                var allowPublishigWithSubitems = false;
                customPublishConfig = sitecoreService.GetItem<ICustom_Publishing_Config>(ItemReferences.Instance.CustomPublishingConfig);
                if (customPublishConfig != null)
                {
                    allowPublishigWithSubitems = customPublishConfig.Allow_Publishing_With_Subitems;
                }

                if (allowPublishigWithSubitems == false)
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
}
