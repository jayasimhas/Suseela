using Jabberwocky.Glass.Autofac.Attributes;
using System.Linq;

namespace Informa.Library.Publishing.Scheduled
{
	[AutowireService(LifetimeScope.Default)]
	public class ProcessScheduledPublishing : IProcessScheduledPublishing
	{
		protected readonly IReadyScheduledPublishes ReadyScheduledPublishes;
		protected readonly IPublishScheduledPublishes PublishScheduledPublishes;
		protected readonly IUpsertScheduledPublishes UpsertScheduledPublishes;

		public ProcessScheduledPublishing(
			IReadyScheduledPublishes readyScheduledPublishes,
			IPublishScheduledPublishes publishScheduledPublishes,
			IUpsertScheduledPublishes upsertScheduledPublishes)
		{
			ReadyScheduledPublishes = readyScheduledPublishes;
			PublishScheduledPublishes = publishScheduledPublishes;
			UpsertScheduledPublishes = upsertScheduledPublishes;
		}

		public void Process()
		{
			var scheduledPublishes = ReadyScheduledPublishes.ScheduledPublishes;
			var scheduledPublishesResult = PublishScheduledPublishes.Publish(scheduledPublishes);
			var groupedResults = scheduledPublishesResult.ScheduledPublishes.GroupBy(sp => sp.PublishingStatus.Status);

			var doneScheduledPublishes = groupedResults.FirstOrDefault(gr => gr.Key == PublishStatus.Done)?.Select(sp => sp.ScheduledPublish).ToList();

			if (doneScheduledPublishes != null)
			{
				doneScheduledPublishes.ForEach(dsp => dsp.Published = true);

				UpsertScheduledPublishes.Upsert(doneScheduledPublishes);
			}
		}
	}
}
