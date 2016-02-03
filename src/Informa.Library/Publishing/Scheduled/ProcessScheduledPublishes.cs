using Jabberwocky.Glass.Autofac.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Informa.Library.Publishing.Scheduled
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class ProcessScheduledPublishes : IProcessScheduledPublishes
	{
		protected readonly IProcessScheduledPublish ProcessScheduledPublish;

		public ProcessScheduledPublishes(
			IProcessScheduledPublish processScheduledPublish)
		{
			ProcessScheduledPublish = processScheduledPublish;
		}

		public void Process(IEnumerable<IScheduledPublish> scheduledPublishes)
		{
			scheduledPublishes.ToList().ForEach(sc => ProcessScheduledPublish.Process(sc));
		}
	}
}
