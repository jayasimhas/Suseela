using Informa.Library.Data.Entity;
using System.Data.Entity;

namespace Informa.Library.Publishing.Scheduled.Entity
{
	public class EntityScheduledPublishContext : CustomDbContext
	{
		public DbSet<ScheduledPublish> ScheduledPublishes { get; set; }
	}
}
