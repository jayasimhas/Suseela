using Jabberwocky.Glass.Autofac.Attributes;
using MongoDB.Driver.Builders;

namespace Informa.Library.Publishing.Scheduled.MongoDB
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class MongoDbDeleteScheduledPublish : IDeleteScheduledPublish
	{
		protected readonly IMongoDbScheduledPublishContext ScheduledPublishContext;

		public MongoDbDeleteScheduledPublish(
			IMongoDbScheduledPublishContext scheduledPublishContext)
		{
			ScheduledPublishContext = scheduledPublishContext;
		}

		public void Delete(IScheduledPublish scheduledPublish)
		{
			var query = Query<ScheduledPublishDocument>.Where(spd =>
				spd.ItemId == scheduledPublish.ItemId &&
				spd.Language == (scheduledPublish.Language ?? string.Empty) &&
				spd.Version == (scheduledPublish.Version ?? string.Empty));

			ScheduledPublishContext.ScheduledPublishes.Remove(query);
		}
	}
}
