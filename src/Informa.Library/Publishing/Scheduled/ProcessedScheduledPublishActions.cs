using Informa.Library.Actions;
using Jabberwocky.Glass.Autofac.Attributes;
using System.Collections.Generic;

namespace Informa.Library.Publishing.Scheduled
{
	[AutowireService(LifetimeScope.Default)]
	public class ProcessedScheduledPublishActions : ActionsProcessor<IProcessedScheduledPublishAction, IScheduledPublish>, IProcessedScheduledPublishActions
	{
		public ProcessedScheduledPublishActions(
			IEnumerable<IProcessedScheduledPublishAction> actions)
			: base(actions)
		{

		}
	}
}
