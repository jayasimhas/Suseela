using Jabberwocky.Glass.Autofac.Attributes;
using System;

namespace Informa.Library.Publishing.Scheduled
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class ScheduledPublishingDateTime : IScheduledPublishingDateTime
	{
		public DateTime Now => DateTime.Now.ToUniversalTime();
	}
}
