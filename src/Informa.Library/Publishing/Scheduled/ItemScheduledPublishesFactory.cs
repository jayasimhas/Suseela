using Sitecore.Data.Items;
using Jabberwocky.Glass.Autofac.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Informa.Library.Publishing.Scheduled
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class ItemScheduledPublishesFactory : IItemScheduledPublishesFactory
	{
		public IEnumerable<IScheduledPublish> Create(Item item)
		{
			// TODO: Create object based on item information

			//*Update
			//	* Planned Publish Date -> ID, Language, Version
			//	* Publishing Restrictions
			//		* Item -> ID
			//		* Version -> ID, Language, Version

			return Enumerable.Empty<IScheduledPublish>();
		}
	}
}
