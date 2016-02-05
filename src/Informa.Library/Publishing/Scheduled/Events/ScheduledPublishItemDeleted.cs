using Autofac;
using Jabberwocky.Glass.Autofac.Util;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Events;
using System;

namespace Informa.Library.Publishing.Scheduled.Events
{
	public class ScheduledPublishItemDeleted
	{
		protected IItemDeleteScheduledPublises DeleteScheduledPublishes { get { return AutofacConfig.ServiceLocator.Resolve<IItemDeleteScheduledPublises>(); } }

		public void Process(object sender, EventArgs args)
		{
			Assert.ArgumentNotNull(args, "args");

			Item item = Event.ExtractParameter(args, 0) as Item;

			DeleteScheduledPublishes.Delete(item);
		}
	}
}
