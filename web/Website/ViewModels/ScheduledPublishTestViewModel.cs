using Informa.Library.Publishing.Scheduled;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Jabberwocky.Glass.Models;
using System;
using System.Collections.Generic;
using System.Web;

namespace Informa.Web.ViewModels
{
	public class ScheduledPublishTestViewModel : GlassViewModel<IGlassBase>
	{
		protected readonly IAllScheduledPublishes AllScheduledPublishes;
		protected readonly IUpsertScheduledPublish UpsertScheduledPublish;
		protected readonly IProcessScheduledPublishing ProcessScheduledPublishing;

		public ScheduledPublishTestViewModel(
			IAllScheduledPublishes allScheduledPublishes,
			IUpsertScheduledPublish upsertScheduledPublish,
			IProcessScheduledPublishing processScheduledPublishing)
		{
			AllScheduledPublishes = allScheduledPublishes;
			UpsertScheduledPublish = upsertScheduledPublish;
			ProcessScheduledPublishing = processScheduledPublishing;
		}

		public string ProcessedMessage
		{
			get
			{
				var process = HttpContext.Current.Request["spTestRun"];

				if (string.IsNullOrEmpty(process))
				{
					return "Not Processing";
				}

				ProcessScheduledPublishing.Process();

				return "Processed";
			}
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