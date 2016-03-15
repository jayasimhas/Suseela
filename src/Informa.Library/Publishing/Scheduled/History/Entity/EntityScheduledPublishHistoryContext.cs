using Informa.Library.Data.Entity;
using System.Data.Entity;

namespace Informa.Library.Publishing.Scheduled.History.Entity
{
	public class EntityScheduledPublishHistoryContext : CustomDbContext
	{
		public DbSet<ScheduledPublishHistory> ScheduledPublishHistories { get; set; }
	}
}
