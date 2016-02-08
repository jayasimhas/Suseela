using Autofac;
using Jabberwocky.Glass.Autofac.Util;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Events;
using System;

namespace Informa.Library.Publishing.Scheduled.Events
{
	public class ScheduledPublishItemSave
	{
		protected IUpdateItemScheduledPublishing UpdateItemScheduledPublishing { get { return AutofacConfig.ServiceLocator.Resolve<IUpdateItemScheduledPublishing>(); } }

		public void Process(object sender, EventArgs args)
		{
			Assert.ArgumentNotNull(args, "args");

			Item item = Event.ExtractParameter(args, 0) as Item;

			UpdateItemScheduledPublishing.Update(item);
		}
	}
}
