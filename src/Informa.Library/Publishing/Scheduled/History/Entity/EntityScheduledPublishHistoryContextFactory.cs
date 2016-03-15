namespace Informa.Library.Publishing.Scheduled.History.Entity
{
	public class EntityScheduledPublishHistoryContextFactory : IEntityScheduledPublishHistoryContextFactory
	{
		public EntityScheduledPublishHistoryContext Create()
		{
			return new EntityScheduledPublishHistoryContext();
		}
	}
}
