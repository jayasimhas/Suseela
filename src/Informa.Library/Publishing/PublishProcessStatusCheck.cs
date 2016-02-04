using Jabberwocky.Glass.Autofac.Attributes;
using Sitecore.Publishing;

namespace Informa.Library.Publishing
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class ScheduledPublishedStatusCheck : IPublishProcessStatusCheck
	{
		public IPublishProcessStatus Update(IPublishProcessStatus publishProcessStatus)
		{
			var publishStatus = PublishManager.GetStatus(publishProcessStatus.PublishHandle);

			if (publishStatus.Failed || publishStatus.Expired)
			{
				publishProcessStatus.Status = PublishStatus.Failed;
			}
			else if (publishStatus.IsDone)
			{
				publishProcessStatus.Status = PublishStatus.Done;
			}

			return publishProcessStatus;
		}
	}
}
