using Sitecore;

namespace Informa.Library.Publishing
{
	public interface IPublishProcessStatus
	{
		Handle PublishHandle { get; }
		PublishStatus Status { get; set; }
		string Message { get; set; }
	}
}
