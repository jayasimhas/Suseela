using Sitecore;

namespace Informa.Library.Publishing
{
	public interface IPublishingStatus
	{
		Handle PublishHandle { get; }
		PublishStatus Status { get; set; }
		string Message { get; set; }
	}
}
