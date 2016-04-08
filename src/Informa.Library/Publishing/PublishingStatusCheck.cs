using Jabberwocky.Glass.Autofac.Attributes;
using Sitecore.Publishing;

namespace Informa.Library.Publishing
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class PublishingStatusCheck : IPublishingStatusCheck
	{
		public IPublishingStatus Update(IPublishingStatus publishingStatus)
		{
			if (publishingStatus.PublishHandle == null)
			{
				publishingStatus.Status = PublishStatus.Failed;

				return publishingStatus;
			}

			var publishStatus = PublishManager.GetStatus(publishingStatus.PublishHandle);

			if (publishStatus.Failed || publishStatus.Expired)
			{
				publishingStatus.Status = PublishStatus.Failed;
			}
			else if (publishStatus.IsDone)
			{
				publishingStatus.Status = PublishStatus.Done;
			}

			return publishingStatus;
		}
	}
}
