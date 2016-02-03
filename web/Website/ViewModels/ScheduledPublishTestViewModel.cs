using Informa.Library.Publishing.Scheduled;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Jabberwocky.Glass.Models;
using System;
using System.Collections.Generic;

namespace Informa.Web.ViewModels
{
	public class ScheduledPublishTestViewModel : GlassViewModel<IGlassBase>
	{
		protected readonly IAllScheduledPublishes AllScheduledPublishes;
		protected readonly IUpsertScheduledPublish UpsertScheduledPublish;

		public ScheduledPublishTestViewModel(
			IAllScheduledPublishes allScheduledPublishes,
			IUpsertScheduledPublish upsertScheduledPublish)
		{
			AllScheduledPublishes = allScheduledPublishes;
			UpsertScheduledPublish = upsertScheduledPublish;
		}

		public IEnumerable<IScheduledPublish> ScheduledPublishes => AllScheduledPublishes.ScheduledPublishes;

		public void Insert()
		{
			var scheduledPublish = new ScheduledPublish
			{
				ItemId = Guid.NewGuid(),
				Language = string.Empty,
				Published = false,
				PublishOn = DateTime.Now,
				Version = string.Empty
			};

			UpsertScheduledPublish.Upsert(scheduledPublish);
		}
	}
}