using Sitecore;

namespace Informa.Library.Publishing
{
	public class PublishProcessStatus : IPublishProcessStatus
	{
		public Handle PublishHandle { get; set; }
		public PublishStatus Status { get; set; }
		public string Message { get; set; }
	}
}
