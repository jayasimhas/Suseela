using Jabberwocky.Glass.Autofac.Attributes;
using MongoDB.Driver.Builders;

namespace Informa.Library.Publishing.Scheduled.MongoDB
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class MongoDbFindOneSchuledPublish : IMongoDbFindOneScheduledPublish
	{
		protected readonly IMongoDbScheduledPublishContext ScheduledPublishContext;

		public MongoDbFindOneSchuledPublish(
			IMongoDbScheduledPublishContext scheduledPublishContext)
		{
			ScheduledPublishContext = scheduledPublishContext;
		}

		public ScheduledPublishDocument Find(IScheduledPublish scheduledPublish)
		{
			var query = Query<ScheduledPublishDocument>.Where(scd =>
				scd.ItemId == scheduledPublish.ItemId &&
				scd.Language == scheduledPublish.Language &&
				scd.Version == scheduledPublish.Version
			);

			return ScheduledPublishContext.ScheduledPublishes.FindOne(query);
		}
	}
}
