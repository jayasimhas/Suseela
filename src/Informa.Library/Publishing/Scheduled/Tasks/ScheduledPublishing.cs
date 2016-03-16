using Autofac;
using Jabberwocky.Glass.Autofac.Util;

namespace Informa.Library.Publishing.Scheduled.Tasks
{
	public class ScheduledPublishing
	{
		protected IProcessScheduledPublishing ProcessScheduledPublishes { get { return AutofacConfig.ServiceLocator.Resolve<IProcessScheduledPublishing>(); } }

		public void Run()
		{
			ProcessScheduledPublishes.Process();
		}
	}
}
