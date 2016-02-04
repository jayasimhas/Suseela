using System.Collections.Generic;
using Sitecore.Data.Items;
using Jabberwocky.Glass.Autofac.Attributes;
using Sitecore.Data.Fields;
using System.Linq;

namespace Informa.Library.Publishing.Scheduled
{
	[AutowireService(LifetimeScope.Default)]
	public class UpdateItemScheduledPublishing : IUpdateItemScheduledPublishing
	{
		private const string ScheduledPublishingEnabledFieldName = "Scheduled Publishing Enabled";

		protected readonly IItemScheduledPublishesFactory ItemScheduledPublishesFactory;
		protected readonly IUpsertScheduledPublishes UpsertScheduledPublishes;
		protected readonly IDeleteScheduledPublish DeleteScheduledPublishes;

		public UpdateItemScheduledPublishing(
			IItemScheduledPublishesFactory itemScheduledPublishesFactory,
			IUpsertScheduledPublishes upsertScheduledPublishes,
			IDeleteScheduledPublish deleteScheduledPublishes)
		{
			ItemScheduledPublishesFactory = itemScheduledPublishesFactory;
			UpsertScheduledPublishes = upsertScheduledPublishes;
			DeleteScheduledPublishes = deleteScheduledPublishes;
		}

		public void Update(Item item)
		{
			if (Ignore(item))
			{
				return;
			}

			var scheduledPublishes = ItemScheduledPublishesFactory.Create(item);

			// -- Clean up existing publishes --
			// 1) Get existing scheduled shared (ID) and language (Language) publishes
			// 2) Compare existing against new to create list of scheduled publishes to be deleted
			var oldScheduledPublishes = Enumerable.Empty<IScheduledPublish>().ToList();
			// 3) Delete any publishes found not to exist any more
			oldScheduledPublishes.ForEach(osp => DeleteScheduledPublishes.Delete(osp));
			
			UpsertScheduledPublishes.Upsert(scheduledPublishes);
		}

		public bool Ignore(Item item)
		{
			return item == null || item.Fields[ScheduledPublishingEnabledFieldName] == null || !((CheckboxField)item.Fields[ScheduledPublishingEnabledFieldName]).Checked;
		}
	}
}
