using Sitecore;

namespace Informa.Library.Publishing
{
	public class PublishingStatus : IPublishingStatus
	{
		public Handle PublishHandle { get; set; }
		public PublishStatus Status { get; set; }
		public string Message { get; set; }
	}
}
