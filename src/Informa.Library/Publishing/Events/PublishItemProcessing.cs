using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Publishing.Pipelines.PublishItem;
using System;
using System.Linq;
using System.Collections.Generic;
using Informa.Library.CustomSitecore.Extensions;
using Informa.Library.Utilities.References;

namespace Informa.Library.Publishing.Events
{
	public abstract class PublishItemProcessing
	{
		public void Process(object sender, EventArgs args)
		{
			var publishArgs = args as ItemProcessingEventArgs;

            //This prevents from publishing the whole site by mistake
            if (publishArgs.Context.PublishOptions.Deep)
            {
                var rootItem = publishArgs.Context.PublishOptions.RootItem;
                if (rootItem != null && (rootItem.TemplateID.Guid.Equals(new Guid(Constants.PublicationRootTemplateID))
                    ||
                    rootItem.TemplateID.Guid.Equals(new Guid(Constants.HomeRootTemplateID))
                    ))
                {
                    publishArgs.Context.PublishOptions.Deep = false;
                    publishArgs.Context.AddMessage("Subitems were not published. Publishing subitems from Publication/Home Root items has been stopped");
                }
            }

            if (publishArgs == null)
			{
				return;
			}

			var item = publishArgs.Context.PublishHelper.GetSourceItem(publishArgs.Context.ItemId);

			if (item == null || (IgnoreStandardValues && item.Name.IsStandardValues()) || (TemplateIds.Any() && !TemplateIds.Contains(item.TemplateID)))
			{
				return;
			}

			ProcessPublish(item, publishArgs);
		}

		public abstract IEnumerable<ID> TemplateIds { get; }

		public virtual bool IgnoreStandardValues => true;

		public abstract void ProcessPublish(Item item, ItemProcessingEventArgs args);
	}
}
