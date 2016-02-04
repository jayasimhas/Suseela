using Sitecore.Data.Items;

namespace Informa.Library.Publishing.Scheduled
{
	public interface IRetrieveItemToPublish
	{
		Item Get(IScheduledPublish scheduledPublish);
	}
}
