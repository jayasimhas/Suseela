using System;

namespace Informa.Library.Publishing.Scheduled
{
	public interface IScheduledPublish
	{
		Guid ItemId { get; }
		string Language { get; }
		string Version { get; }
		DateTime PublishOn { get; }
		bool Published { get; }
	}
}
