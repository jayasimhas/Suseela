using Jabberwocky.Glass.Autofac.Attributes;
using Sitecore.Data.Items;

namespace Informa.Library.Publishing.Scheduled
{
	[AutowireService(LifetimeScope.Default)]
	public class ItemDeleteScheduledPublises : IItemDeleteScheduledPublises
	{
		protected readonly IDeleteItemScheduledPublishes DeleteScheduledPublishes;

		public ItemDeleteScheduledPublises(
			IDeleteItemScheduledPublishes deleteScheduledPublishes)
		{
			DeleteScheduledPublishes = deleteScheduledPublishes;
		}

		public void Delete(Item item)
		{
			DeleteScheduledPublishes.Delete(item.ID.Guid);
		}
	}
}
