using Autofac;
using Jabberwocky.Glass.Autofac.Util;
using Sitecore.Data.Items;
using Sitecore.Tasks;

namespace Informa.Library.Publishing.Scheduled.Tasks
{
	public class ProcessScheduledPublishing
	{
		protected IProcessScheduledPublishing ProcessScheduledPublishes { get { return AutofacConfig.ServiceLocator.Resolve<IProcessScheduledPublishing>(); } }

		public void Process(Item[] items, CommandItem command, ScheduleItem schedule)
		{
			ProcessScheduledPublishes.Process();
		}
	}
}
