using Jabberwocky.Glass.Autofac.Attributes;
using System;

namespace Informa.Library.Publishing.Scheduled.MongoDB
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class MongoDbUpsertScheduledPublish : IUpsertScheduledPublish
	{
		protected readonly IScheduledPublishDocumentFactory DocumentFactory;
		protected readonly IMongoDbFindOneScheduledPublish FindOneScheduledPublish;
		protected readonly IMongoDbScheduledPublishContext ScheduledPublishContext;

		public MongoDbUpsertScheduledPublish(
			IScheduledPublishDocumentFactory documentFactory,
			IMongoDbFindOneScheduledPublish findOneScheduledPublish,
			IMongoDbScheduledPublishContext scheduledPublishContext)
		{
			DocumentFactory = documentFactory;
			FindOneScheduledPublish = findOneScheduledPublish;
			ScheduledPublishContext = scheduledPublishContext;
		}

		public void Upsert(IScheduledPublish scheduledPublish)
		{
			var document = FindOneScheduledPublish.Find(scheduledPublish);

			if (document == null)
			{
				document = DocumentFactory.Create(scheduledPublish);
			}
			else
			{
				document.LastUpdated = DateTime.Now;
				document.Published = scheduledPublish.Published;
				document.PublishOn = scheduledPublish.PublishOn;
			}
			
			ScheduledPublishContext.ScheduledPublishes.Save(document);
		}
	}
}