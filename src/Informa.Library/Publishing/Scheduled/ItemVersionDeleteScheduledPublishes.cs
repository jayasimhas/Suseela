using Jabberwocky.Glass.Autofac.Attributes;
using Sitecore.Data.Items;

namespace Informa.Library.Publishing.Scheduled
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class ItemVersionDeleteScheduledPublishes : IItemVersionDeleteScheduledPublishes
	{
		protected readonly IDeleteVersionScheduledPublishes DeleteScheduledPublishes;

		public ItemVersionDeleteScheduledPublishes(
			IDeleteVersionScheduledPublishes deleteScheduledPublishes)
		{
			DeleteScheduledPublishes = deleteScheduledPublishes;
		}

		public void Delete(Item item)
		{
			DeleteScheduledPublishes.Delete(item.ID.Guid, item.Language.Name, item.Version.Number.ToString());
		}
	}
}
