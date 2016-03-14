namespace Informa.Library.Publishing.Scheduled.MongoDB
{
	public class MongoDbInsertScheduledPublish : IMongoDbInsertScheduledPublish
	{
		protected readonly IMongoDbScheduledPublishContext ScheduledPublishContext;
		protected readonly IScheduledPublishDocumentFactory DocumentFactory;

		public MongoDbInsertScheduledPublish(
			IMongoDbScheduledPublishContext scheduledPublishContext,
			IScheduledPublishDocumentFactory documentFactory)
		{
			ScheduledPublishContext = scheduledPublishContext;
			DocumentFactory = documentFactory;
		}

		public void Insert(IScheduledPublish scheduledPublish)
		{
			var document = DocumentFactory.Create(scheduledPublish);

			ScheduledPublishContext.ScheduledPublishes.Insert(document);
		}
	}
}
