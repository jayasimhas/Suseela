using Sitecore.Data.Items;
using Jabberwocky.Glass.Autofac.Attributes;
using Sitecore.Data;

namespace Informa.Library.Publishing.Scheduled
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class RetrieveItemToPublish : IRetrieveItemToPublish
	{
		protected readonly IScheduledPublishingDatabaseContext DatabaseContext;

		public RetrieveItemToPublish(
			IScheduledPublishingDatabaseContext databaseContext)
		{
			DatabaseContext = databaseContext;
		}

		public Item Get(IScheduledPublish scheduledPublish)
		{
			// TODO: Determine how to get item based on language and version properties

			var id = ID.Parse(scheduledPublish.ItemId);

			return DatabaseContext.Database.GetItem(id);
		}
	}
}
