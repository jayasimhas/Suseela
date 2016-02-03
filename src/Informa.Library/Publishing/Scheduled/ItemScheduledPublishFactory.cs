using System;
using Sitecore.Data.Items;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.Publishing.Scheduled
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class ItemScheduledPublishFactory : IItemScheduledPublishFactory
	{
		public IScheduledPublish Create(Item item)
		{
			// TODO: Create object based on item information

			throw new NotImplementedException();
		}
	}
}
