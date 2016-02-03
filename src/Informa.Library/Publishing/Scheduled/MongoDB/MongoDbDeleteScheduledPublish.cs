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
			ScheduledPublishContext.ScheduledPublishes.Remove(Query<ScheduledPublishDocument>.Where(scd =>
				scd.ItemId == scheduledPublish.ItemId &&
				scd.Language == scheduledPublish.Language &&
				scd.Version == scheduledPublish.Version
			));
		}
	}
}
