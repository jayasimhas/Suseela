using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Publishing.Pipelines.PublishItem;
using System;
using System.Linq;
using System.Collections.Generic;
using Informa.Library.CustomSitecore.Extensions;

namespace Informa.Library.Publishing.Events
{
    public abstract class PublishItemProcessing
    {
        public void Process(object sender, EventArgs args)
        {
            var publishArgs = args as ItemProcessingEventArgs;

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
