using Jabberwocky.Glass.Autofac.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Informa.Library.Publishing.Scheduled
{
	[AutowireService(LifetimeScope.Default)]
	public class PublishScheduledPublishes : IPublishScheduledPublishes
	{
		protected readonly IPublishScheduledPublish ProcessScheduledPublish;
		protected readonly IPublishingStatusCheck PublishProcessStatusCheck;

		public PublishScheduledPublishes(
			IPublishScheduledPublish processScheduledPublish,
			IPublishingStatusCheck publishProcessStatusCheck)
		{
			ProcessScheduledPublish = processScheduledPublish;
			PublishProcessStatusCheck = publishProcessStatusCheck;
		}

		public IScheduledPublishesResult Publish(IEnumerable<IScheduledPublish> scheduledPublishes)
		{
			var allPublishes = scheduledPublishes.Select(sp => new ScheduledPublishResult { ScheduledPublish = sp, PublishingStatus = ProcessScheduledPublish.Publish(sp) }).ToList();
			var processingPublishes = allPublishes;

			while (processingPublishes != null)
			{
				processingPublishes.ForEach(pp => pp.PublishingStatus = PublishProcessStatusCheck.Update(pp.PublishingStatus));

				var groupedStatuses = processingPublishes.GroupBy(pp => pp.PublishingStatus.Status);

				processingPublishes = groupedStatuses.FirstOrDefault(gs => gs.Key == PublishStatus.Processing)?.ToList();
			}

			return new ScheduledPublishesResult
			{
				ScheduledPublishes = allPublishes
			};
		}
	}
}
