namespace Informa.Library.Publishing.Scheduled
{
	public interface IProcessScheduledPublish
	{
		void Process(IScheduledPublish scheduledPublish);
	}
}
