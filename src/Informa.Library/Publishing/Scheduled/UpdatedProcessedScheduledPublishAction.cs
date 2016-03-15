using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.Publishing.Scheduled
{
	[AutowireService(LifetimeScope.Default)]
	public class UpdatedProcessedScheduledPublishAction : IUpdateProcessedScheduledPublishAction
	{
		protected readonly IUpsertScheduledPublish UpsertScheduledPublish;

		public UpdatedProcessedScheduledPublishAction(
			IUpsertScheduledPublish upsertScheduledPublish)
		{
			UpsertScheduledPublish = upsertScheduledPublish;
		}

		public void Process(IScheduledPublish value)
		{
			UpsertScheduledPublish.Upsert(value);
		}
	}
}
