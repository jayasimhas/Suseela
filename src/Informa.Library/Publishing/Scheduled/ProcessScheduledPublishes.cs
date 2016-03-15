using Jabberwocky.Glass.Autofac.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Informa.Library.Publishing.Scheduled
{
	[AutowireService(LifetimeScope.Default)]
	public class ProcessScheduledPublishes : IProcessScheduledPublishes
	{
		protected readonly IPublishScheduledPublishes PublishScheduledPublishes;
		protected readonly IUpsertScheduledPublishes UpsertScheduledPublishes;
		protected readonly IProcessedScheduledPublishActions ProcessedActions;

		public ProcessScheduledPublishes(
			IPublishScheduledPublishes publishScheduledPublishes,
			IUpsertScheduledPublishes upsertScheduledPublishes,
			IProcessedScheduledPublishActions processedActions)
		{
			PublishScheduledPublishes = publishScheduledPublishes;
			UpsertScheduledPublishes = upsertScheduledPublishes;
			ProcessedActions = processedActions;
		}

		public void Process(IEnumerable<IScheduledPublish> scheduledPublishes)
		{
			var scheduledPublishesResult = PublishScheduledPublishes.Publish(scheduledPublishes);
			var results = scheduledPublishesResult.ScheduledPublishes.ToList();

			results.ForEach(r => ProcessedActions.Process(r));
		}
	}
}
