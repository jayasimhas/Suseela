using Jabberwocky.Glass.Autofac.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Informa.Library.Publishing.Scheduled.MongoDB
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class MongoDbAllScheduledPublishes : IAllScheduledPublishes
	{
		protected readonly IMongoDbScheduledPublishContext ScheduledPublishContext;

		public MongoDbAllScheduledPublishes(
			IMongoDbScheduledPublishContext scheduledPublishContext)
		{
			ScheduledPublishContext = scheduledPublishContext;
		}

		public IEnumerable<IScheduledPublish> ScheduledPublishes => ScheduledPublishContext.ScheduledPublishes.FindAll().ToList();
	}
}
