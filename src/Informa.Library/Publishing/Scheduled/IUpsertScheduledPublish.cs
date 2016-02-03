namespace Informa.Library.Publishing.Scheduled
{
	public interface IUpsertScheduledPublish
	{
		void Upsert(IScheduledPublish scheduledPublish);
	}
}
