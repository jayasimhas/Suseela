﻿using Jabberwocky.Glass.Autofac.Attributes;
using System;

namespace Informa.Library.Publishing.Scheduled.MongoDB
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class ScheduledPublishDocumentFactory : IScheduledPublishDocumentFactory
	{
		public ScheduledPublishDocument Create(IScheduledPublish scheduledPublish)
		{
			var now = DateTime.Now;

			return new ScheduledPublishDocument
			{
				Added = now,
				ItemId = scheduledPublish.ItemId,
				Language = scheduledPublish.Language,
				Published = scheduledPublish.Published,
				PublishOn = scheduledPublish.PublishOn,
				Version = scheduledPublish.Version,
				LastUpdated = now
			};
		}
	}
}
