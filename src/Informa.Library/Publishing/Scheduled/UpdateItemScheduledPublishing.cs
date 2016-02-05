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
		protected readonly IFindScheduledPublishes FindScheduledPublishes;
		protected readonly IUpsertScheduledPublishes UpsertScheduledPublishes;
		protected readonly IDeleteScheduledPublish DeleteScheduledPublishes;

		public UpdateItemScheduledPublishing(
			IItemScheduledPublishesFactory itemScheduledPublishesFactory,
			IFindScheduledPublishes findScheduledPublishes,
			IUpsertScheduledPublishes upsertScheduledPublishes,
			IDeleteScheduledPublish deleteScheduledPublishes)
		{
			ItemScheduledPublishesFactory = itemScheduledPublishesFactory;
			FindScheduledPublishes = findScheduledPublishes;
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
			var existingScheduledPublishes = new List<IScheduledPublish>();

			existingScheduledPublishes.AddRange(FindScheduledPublishes.Find(item.ID.Guid, string.Empty, string.Empty));
			existingScheduledPublishes.AddRange(FindScheduledPublishes.Find(item.ID.Guid, item.Language.Name, item.Version.Number.ToString()));
			existingScheduledPublishes
				.Where(esp => !scheduledPublishes.Any(sp =>
					sp.Language == esp.Language &&
					sp.Version == esp.Version &&
					sp.Type == esp.Type))
				.ToList()
				.ForEach(osp => DeleteScheduledPublishes.Delete(osp));

			UpsertScheduledPublishes.Upsert(scheduledPublishes);
		}

		public bool Ignore(Item item)
		{
			return item == null || item.Fields[ScheduledPublishingEnabledFieldName] == null || !((CheckboxField)item.Fields[ScheduledPublishingEnabledFieldName]).Checked;
		}
	}
}
