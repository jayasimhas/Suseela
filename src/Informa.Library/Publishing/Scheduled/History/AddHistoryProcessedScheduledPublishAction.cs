namespace Informa.Library.Publishing.Scheduled.History
{
	public class AddHistoryProcessedScheduledPublishAction : IAddHistoryProcessedScheduledPublishAction
	{
		protected readonly IAddScheduledPublishHistory AddScheduledPublishHistory;
		protected readonly IScheduledPublishHistoryFactory ScheduledPublishHistoryFactory;

		public AddHistoryProcessedScheduledPublishAction(
			IAddScheduledPublishHistory addScheduledPublishHistory,
			IScheduledPublishHistoryFactory scheduledPublishHistoryFactory)
		{
			AddScheduledPublishHistory = addScheduledPublishHistory;
			ScheduledPublishHistoryFactory = scheduledPublishHistoryFactory;
		}

		public void Process(IScheduledPublishResult value)
		{
			if (value.PublishingStatus.Status != PublishStatus.Done)
			{
				return;
			}

			var scheduledPublishHistory = ScheduledPublishHistoryFactory.Create(value.ScheduledPublish);

			AddScheduledPublishHistory.Add(scheduledPublishHistory);
		}
	}
}
