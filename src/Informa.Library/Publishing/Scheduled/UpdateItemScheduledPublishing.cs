using System.Collections.Generic;
using Sitecore.Data.Items;

namespace Informa.Library.Publishing.Scheduled
{
	public class UpdateItemScheduledPublishing : IUpdateItemScheduledPublishing
	{
		protected readonly IItemScheduledPublishFactory ItemScheduledPublishingFactory;
		protected readonly IUpdateScheduledPublishes UpdateScheduledPublishes;

		public UpdateItemScheduledPublishing(
			IItemScheduledPublishFactory itemScheduledPublishingFactory,
			IUpdateScheduledPublishes updateScheduledPublishes)
		{
			ItemScheduledPublishingFactory = itemScheduledPublishingFactory;
		}

		public void Update(Item item)
		{
			var scheduledPublish = ItemScheduledPublishingFactory.Create(item);

			UpdateScheduledPublishes.Update(new List<IScheduledPublish> { { scheduledPublish } });
		}
	}
}
