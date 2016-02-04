using System;
using System.Collections.Generic;
using System.Linq;
using Sitecore.Data;
using Informa.Library.Threading;
using Sitecore.Publishing;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.Publishing.Scheduled
{
	[AutowireService(LifetimeScope.Default)]
	public class ScheduledPublishingTargetsContext : ThreadSafe<IEnumerable<Database>>, IScheduledPublishingTargetsContext
	{
		protected readonly IScheduledPublishingDatabaseContext DatabaseContext;

		public ScheduledPublishingTargetsContext(
			IScheduledPublishingDatabaseContext databaseContext)
		{
			DatabaseContext = databaseContext;
		}

		public IEnumerable<Database> Databases => SafeObject;

		protected override IEnumerable<Database> UnsafeObject
		{
			get
			{
				var publishingTargets = PublishManager.GetPublishingTargets(DatabaseContext.Database);

				return publishingTargets.Select(pt => Database.GetDatabase(pt["Target database"])).ToList();
			}
		}
	}
}
