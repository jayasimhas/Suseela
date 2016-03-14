namespace Informa.Library.Publishing.Scheduled.Entity
{
	public class EntityScheduledPublishContextFactory : IEntityScheduledPublishContextFactory
	{
		public EntityScheduledPublishContext Create()
		{
			return new EntityScheduledPublishContext();
		}
	}
}
