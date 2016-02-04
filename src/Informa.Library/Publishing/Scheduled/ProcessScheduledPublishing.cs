using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.Publishing.Scheduled
{
	[AutowireService(LifetimeScope.Default)]
	public class ProcessScheduledPublishing : IProcessScheduledPublishing
	{
		protected readonly IReadyScheduledPublishes ReadyScheduledPublishes;
		protected readonly IPublishScheduledPublishes PublishScheduledPublishes;

		public ProcessScheduledPublishing(
			IReadyScheduledPublishes readyScheduledPublishes,
			IPublishScheduledPublishes publishScheduledPublishes)
		{
			ReadyScheduledPublishes = readyScheduledPublishes;
			PublishScheduledPublishes = publishScheduledPublishes;
		}

		public void Process()
		{
			var scheduledPublishes = ReadyScheduledPublishes.ScheduledPublishes;
			var result = PublishScheduledPublishes.Publish(scheduledPublishes);

			// TODO: Update scheduled publish repository depending on status returned
		}
	}
}
