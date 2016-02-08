using Jabberwocky.Glass.Autofac.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Informa.Library.Publishing.Scheduled
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class UpsertScheduledPublishes : IUpsertScheduledPublishes
	{
		protected readonly IUpsertScheduledPublish UpsertScheduledPublish;

		public UpsertScheduledPublishes(
			IUpsertScheduledPublish upsertScheduledPublish)
		{
			UpsertScheduledPublish = upsertScheduledPublish;
		}

		public void Upsert(IEnumerable<IScheduledPublish> scheduledPublishes)
		{
			scheduledPublishes.ToList().ForEach(sp => UpsertScheduledPublish.Upsert(sp));
		}
	}
}
