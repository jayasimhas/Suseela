using Sitecore.Data.Items;

namespace Informa.Library.Publishing.Scheduled
{
	public interface IItemScheduledPublishFactory
	{
		IScheduledPublish Create(Item item);
	}
}
