namespace Informa.Library.Publishing
{
	public interface IPublishingStatusCheck
	{
		IPublishingStatus Update(IPublishingStatus status);
	}
}
