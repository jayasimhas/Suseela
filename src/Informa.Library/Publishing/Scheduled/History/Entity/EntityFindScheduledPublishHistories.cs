using System;
using System.Collections.Generic;
using System.Linq;

namespace Informa.Library.Publishing.Scheduled.History.Entity
{
	public class EntityFindScheduledPublishHistories : IFindItemVersionScheduledPublishHistories, IFindItemLanguageScheduledPublishHistories, IFindItemScheduledPublishHistories
	{
		protected readonly IEntityScheduledPublishHistoryContextFactory ContextFactory;

		public EntityFindScheduledPublishHistories(
			IEntityScheduledPublishHistoryContextFactory contextFactory)
		{
			ContextFactory = contextFactory;
		}

		public IEnumerable<IScheduledPublishHistory> Find(Guid itemId, string language, string version)
		{
			using (var context = ContextFactory.Create())
			{
				return context.ScheduledPublishHistories.Where(sph =>
					sph.ItemId == itemId &&
					string.Equals(sph.Language, (language ?? string.Empty)) &&
					string.Equals(sph.Version, (version ?? string.Empty))
				).ToList();
			}
		}

		public IEnumerable<IScheduledPublishHistory> Find(Guid itemId, string language)
		{
			using (var context = ContextFactory.Create())
			{
				return context.ScheduledPublishHistories.Where(sph =>
					sph.ItemId == itemId &&
					string.Equals(sph.Language, (language ?? string.Empty))
				).ToList();
			}
		}

		public IEnumerable<IScheduledPublishHistory> Find(Guid itemId)
		{
			using (var context = ContextFactory.Create())
			{
				return context.ScheduledPublishHistories.Where(sph =>
					sph.ItemId == itemId
				).ToList();
			}
		}
	}
}
