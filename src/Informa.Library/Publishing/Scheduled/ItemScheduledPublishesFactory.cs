﻿using Sitecore.Data.Items;
using Jabberwocky.Glass.Autofac.Attributes;
using System.Collections.Generic;
using System;
using Sitecore.Data.Fields;

namespace Informa.Library.Publishing.Scheduled
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class ItemScheduledPublishesFactory : IItemScheduledPublishesFactory
	{
		protected readonly IScheduledPublishingDateTime ScheduledPublishingDateTime;

		public ItemScheduledPublishesFactory(
			IScheduledPublishingDateTime scheduledPublishingDateTime)
		{
			ScheduledPublishingDateTime = scheduledPublishingDateTime;
		}

		public IEnumerable<IScheduledPublish> Create(Item item)
		{
			var scheduledPublishes = new List<ScheduledPublish>();
			var itemId = item.ID.Guid;
			var language = item.Language.Name;
			var version = item.Version.Number.ToString();

			var plannedPublishDateField = (DateField)item.Fields["Planned Publish Date"];

			if (plannedPublishDateField != null)
			{
				AddScheduledPublish(scheduledPublishes, itemId, string.Empty, string.Empty, plannedPublishDateField.DateTime, ScheduledPublishType.Planned);
			}

			AddScheduledPublish(scheduledPublishes, itemId, string.Empty, string.Empty, item.Publishing.PublishDate, ScheduledPublishType.From);
			AddScheduledPublish(scheduledPublishes, itemId, string.Empty, string.Empty, item.Publishing.UnpublishDate, ScheduledPublishType.To);
			AddScheduledPublish(scheduledPublishes, itemId, language, version, item.Publishing.ValidFrom, ScheduledPublishType.From);
			AddScheduledPublish(scheduledPublishes, itemId, language, version, item.Publishing.ValidTo, ScheduledPublishType.To);

			return scheduledPublishes;
		}

		public bool HasValidValue(DateTime value)
		{
			return value != DateTime.MinValue && value != DateTime.MaxValue;
		}

		public void AddScheduledPublish(List<ScheduledPublish> scheduledPublishes, Guid itemId, string language, string version, DateTime publishOn, ScheduledPublishType type)
		{
			if (HasValidValue(publishOn))
			{
				scheduledPublishes.Add(new ScheduledPublish
				{
					ItemId = itemId,
					Language = language,
					Published = false,
					PublishOn = publishOn,
					Version = version,
					Type = type
				});
			}
		}
	}
}
