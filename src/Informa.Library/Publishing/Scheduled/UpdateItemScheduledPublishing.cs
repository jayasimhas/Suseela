using System.Collections.Generic;
using Sitecore.Data.Items;
using Jabberwocky.Glass.Autofac.Attributes;
using Sitecore.Data.Fields;

namespace Informa.Library.Publishing.Scheduled
{
	[AutowireService(LifetimeScope.Default)]
	public class UpdateItemScheduledPublishing : IUpdateItemScheduledPublishing
	{
		private const string ScheduledPublishingEnabledFieldName = "Scheduled Publishing Enabled";

		protected readonly IItemScheduledPublishFactory ItemScheduledPublishingFactory;
		protected readonly IUpsertScheduledPublishes UpdateScheduledPublishes;

		public UpdateItemScheduledPublishing(
			IItemScheduledPublishFactory itemScheduledPublishingFactory,
			IUpsertScheduledPublishes updateScheduledPublishes)
		{
			ItemScheduledPublishingFactory = itemScheduledPublishingFactory;
		}

		public void Update(Item item)
		{
			if (Ignore(item))
			{
				return;
			}

			//var scheduledPublish = ItemScheduledPublishingFactory.Create(item);

			//UpdateScheduledPublishes.Upsert(new List<IScheduledPublish> { { scheduledPublish } });
		}

		public bool Ignore(Item item)
		{
			return item == null || item.Fields[ScheduledPublishingEnabledFieldName] == null || !((CheckboxField)item.Fields[ScheduledPublishingEnabledFieldName]).Checked;
		}
	}
}
