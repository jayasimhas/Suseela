using Jabberwocky.Glass.Autofac.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Informa.Library.Publishing.Scheduled
{
	[AutowireService(LifetimeScope.Default)]
	public class ProcessScheduledPublishing : IProcessScheduledPublishing
	{
		protected readonly IProcessScheduledPublish ProcessScheduledPublish;
		protected readonly IPublishProcessStatusCheck PublishProcessStatusCheck;

		public ProcessScheduledPublishing(
			IProcessScheduledPublish processScheduledPublish,
			IPublishProcessStatusCheck publishProcessStatusCheck)
		{
			ProcessScheduledPublish = processScheduledPublish;
			PublishProcessStatusCheck = publishProcessStatusCheck;
		}

		public void Process(IEnumerable<IScheduledPublish> scheduledPublishes)
		{
			var statuses = scheduledPublishes.Select(sp => new { ScheduledPublish = sp, Status = ProcessScheduledPublish.Process(sp) }).ToList();
			var incompleteStatuses = statuses;
			var publishingRunning = true;

			while (publishingRunning)
			{
				var groupedStatues = incompleteStatuses.GroupBy(i => PublishProcessStatusCheck.Update(i.Status).Status).ToList();
				var processingStatues = groupedStatues.FirstOrDefault(gs => gs.Key == PublishStatus.Processing);
				
				if (processingStatues == null)
				{
					publishingRunning = false;

					continue;
				}

				incompleteStatuses = processingStatues.ToList();
			}
		}
	}
}
