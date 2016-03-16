using Jabberwocky.Glass.Autofac.Attributes;
using System.Linq;

namespace Informa.Library.Publishing.Scheduled
{
	[AutowireService(LifetimeScope.Default)]
	public class ProcessScheduledPublishing : IProcessScheduledPublishing
	{
		protected readonly IReadyScheduledPublishes ReadyScheduledPublishes;
		protected readonly IProcessScheduledPublishes ProcessScheduledPublishes;

		public ProcessScheduledPublishing(
			IReadyScheduledPublishes readyScheduledPublishes,
			IProcessScheduledPublishes processScheduledPublishes)
		{
			ReadyScheduledPublishes = readyScheduledPublishes;
			ProcessScheduledPublishes = processScheduledPublishes;
		}

		public void Process()
		{
			var scheduledPublishes = ReadyScheduledPublishes.ScheduledPublishes;

			if (scheduledPublishes.Any())
			{
				ProcessScheduledPublishes.Process(scheduledPublishes);
			}
		}
	}
}
