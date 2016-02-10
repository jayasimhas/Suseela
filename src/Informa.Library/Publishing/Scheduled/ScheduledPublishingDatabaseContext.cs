using Sitecore.Data;
using Informa.Library.Threading;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.Publishing.Scheduled
{
	[AutowireService(LifetimeScope.Default)]
	public class ScheduledPublishingDatabaseContext : ThreadSafe<Database>, IScheduledPublishingDatabaseContext
	{
		public Database Database => SafeObject;

		protected override Database UnsafeObject => Database.GetDatabase("master");
	}
}
