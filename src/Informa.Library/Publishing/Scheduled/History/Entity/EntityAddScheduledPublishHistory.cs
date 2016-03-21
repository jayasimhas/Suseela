namespace Informa.Library.Publishing.Scheduled.History.Entity
{
	public class EntityAddScheduledPublishHistory : IAddScheduledPublishHistory
	{
		protected readonly IEntityScheduledPublishFactory ScheduledPublishHistoryFactory;
		protected readonly IEntityScheduledPublishHistoryContextFactory ContextFactory;

		public EntityAddScheduledPublishHistory(
			IEntityScheduledPublishFactory scheduledPublishHistoryFactory,
			IEntityScheduledPublishHistoryContextFactory contextFactory)
		{
			ScheduledPublishHistoryFactory = scheduledPublishHistoryFactory;
			ContextFactory = contextFactory;
		}

		public void Add(IScheduledPublishHistory scheduledPublishHistory)
		{
			using (var context = ContextFactory.Create())
			{
				var record = ScheduledPublishHistoryFactory.Create(scheduledPublishHistory);

				context.ScheduledPublishHistories.Add(record);
				context.SaveChanges();
			}
		}
	}
}
