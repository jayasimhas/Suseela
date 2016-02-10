using Sitecore.Data.Items;
using System.Collections.Generic;

namespace Informa.Library.Publishing.Scheduled
{
	public interface IItemScheduledPublishesFactory
	{
		IEnumerable<IScheduledPublish> Create(Item item);
	}
}
