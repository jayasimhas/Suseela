using Sitecore.Data.Items;
using Jabberwocky.Glass.Autofac.Attributes;
using System.Collections.Generic;
using System;
using Sitecore.Data.Fields;

namespace Informa.Library.Publishing.Scheduled
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class ItemScheduledPublishesFactory : IItemScheduledPublishesFactory
	{
		public IEnumerable<IScheduledPublish> Create(Item item)
		{
			var scheduledPublishes = new List<ScheduledPublish>();
			var itemId = item.ID.Guid;
			var language = item.Language.Name;
			var version = item.Version.Number.ToString();

			// Planned Publish Date -> ID, Language, Version
			var plannedPublishDateField = (DateField)item.Fields["Planned Publish Date"];

			if (plannedPublishDateField != null)
			{
				var plannedPublishhDate = plannedPublishDateField.DateTime;

				if (HasValidValue(plannedPublishhDate))
				{
					scheduledPublishes.Add(CreateScheduledPublish(itemId, string.Empty, string.Empty, plannedPublishhDate));
				}
			}

			// Publishing Restrictions
			// Item -> ID
			// From
			var publishDate = item.Publishing.PublishDate;

			if (HasValidValue(publishDate))
			{
				scheduledPublishes.Add(CreateScheduledPublish(itemId, string.Empty, string.Empty, publishDate));
			}
			// To
			var unpublishDate = item.Publishing.UnpublishDate;

			if (HasValidValue(unpublishDate))
			{
				scheduledPublishes.Add(CreateScheduledPublish(itemId, string.Empty, string.Empty, unpublishDate));
			}
			// Version -> ID, Language, Version
			// From
			var validFrom = item.Publishing.ValidFrom;

			if (HasValidValue(validFrom))
			{
				scheduledPublishes.Add(CreateScheduledPublish(itemId, language, version, validFrom));
			}
			// To
			var validTo = item.Publishing.ValidTo;

			if (HasValidValue(validTo))
			{
				scheduledPublishes.Add(CreateScheduledPublish(itemId, language, version, validTo));
			}

			return scheduledPublishes;
		}

		public bool HasValidValue(DateTime value)
		{
			return value != DateTime.MinValue && value != DateTime.MaxValue && value > DateTime.Now;
		}

		public ScheduledPublish CreateScheduledPublish(Guid itemId, string language, string version, DateTime publishOn)
		{
			return new ScheduledPublish
			{
				ItemId = itemId,
				Language = language,
				Published = false,
				PublishOn = publishOn,
				Version = version
			};
		}
	}
}
