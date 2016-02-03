using Jabberwocky.Glass.Autofac.Attributes;
using System;

namespace Informa.Library.Publishing.Scheduled
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class ProcessScheduledPublish : IProcessScheduledPublish
	{
		public void Process(IScheduledPublish scheduledPublish)
		{
			// TODO: Sitecore publish

			throw new NotImplementedException();
		}
	}
}
