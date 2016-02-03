using System.Collections.Generic;
using Sitecore.Data.Items;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.Publishing.Scheduled
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class UpdateItemScheduledPublishing : IUpdateItemScheduledPublishing
	{
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
			var scheduledPublish = ItemScheduledPublishingFactory.Create(item);

			UpdateScheduledPublishes.Upsert(new List<IScheduledPublish> { { scheduledPublish } });
		}
	}
}
