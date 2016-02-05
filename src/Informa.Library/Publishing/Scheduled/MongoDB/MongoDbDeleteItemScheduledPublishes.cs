using Jabberwocky.Glass.Autofac.Attributes;
using MongoDB.Driver.Builders;
using System;

namespace Informa.Library.Publishing.Scheduled.MongoDB
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class MongoDbDeleteItemScheduledPublishes : IDeleteItemScheduledPublishes
	{
		protected readonly IMongoDbScheduledPublishContext ScheduledPublishContext;

		public MongoDbDeleteItemScheduledPublishes(
			IMongoDbScheduledPublishContext scheduledPublishContext)
		{
			ScheduledPublishContext = scheduledPublishContext;
		}

		public void Delete(Guid itemId)
		{
			ScheduledPublishContext.ScheduledPublishes.Remove(Query<ScheduledPublishDocument>.Where(spd => spd.ItemId == itemId));
		}
	}
}
