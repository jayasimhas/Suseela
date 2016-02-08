using Jabberwocky.Glass.Autofac.Attributes;
using MongoDB.Driver.Builders;
using System;

namespace Informa.Library.Publishing.Scheduled.MongoDB
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class MongoDbDeleteVersionScheduledPublishes : IDeleteVersionScheduledPublishes
	{
		protected readonly IMongoDbScheduledPublishContext ScheduledPublishContext;

		public MongoDbDeleteVersionScheduledPublishes(
			IMongoDbScheduledPublishContext scheduledPublishContext)
		{
			ScheduledPublishContext = scheduledPublishContext;
		}

		public void Delete(Guid itemId, string language, string version)
		{
			ScheduledPublishContext.ScheduledPublishes.Remove(Query<ScheduledPublishDocument>.Where(spd =>
				spd.ItemId == itemId &&
				spd.Language == language &&
				spd.Version == version));
		}
	}
}
