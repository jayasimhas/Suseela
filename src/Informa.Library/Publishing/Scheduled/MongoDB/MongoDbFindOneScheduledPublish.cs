using MongoDB.Driver.Builders;

namespace Informa.Library.Publishing.Scheduled.MongoDB
{
	public class MongoDbFindOneScheduledPublish : IMongoDbFindOneScheduledPublish
	{
		protected readonly IMongoDbScheduledPublishContext ScheduledPublishContext;

		public MongoDbFindOneScheduledPublish(
			IMongoDbScheduledPublishContext scheduledPublishContext)
		{
			ScheduledPublishContext = scheduledPublishContext;
		}

		public ScheduledPublishDocument Find(IScheduledPublish scheduledPublish)
		{
			var query = Query<ScheduledPublishDocument>.Where(spd =>
				spd.ItemId == scheduledPublish.ItemId &&
				spd.Language == (scheduledPublish.Language ?? string.Empty) &&
				spd.Version == (scheduledPublish.Version ?? string.Empty) &&
				spd.Type == scheduledPublish.Type
			);

			return ScheduledPublishContext.ScheduledPublishes.FindOne(query);
		}
	}
}
