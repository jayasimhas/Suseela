using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver.Builders;

namespace Informa.Library.Publishing.Scheduled.MongoDB
{
	public class MongoDbFindScheduledPublishes : IFindScheduledPublishes
	{
		protected readonly IMongoDbScheduledPublishContext ScheduledPublishContext;

		public MongoDbFindScheduledPublishes(
			IMongoDbScheduledPublishContext scheduledPublishContext)
		{
			ScheduledPublishContext = scheduledPublishContext;
		}

		public IEnumerable<IScheduledPublish> Find(Guid itemId, string language, string version)
		{
			var query = Query<ScheduledPublishDocument>.Where(spd =>
				spd.ItemId == itemId &&
				spd.Language == (language ?? string.Empty) &&
				spd.Version == (version ?? string.Empty)
			);

			return ScheduledPublishContext.ScheduledPublishes.Find(query).ToList();
		}
	}
}
