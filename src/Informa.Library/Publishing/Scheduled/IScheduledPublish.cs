using System;

namespace Informa.Library.Publishing.Scheduled
{
	public interface IScheduledPublish
	{
		Guid ItemId { get; set; }
		string Language { get; set; }
		string Version { get; set; }
		DateTime PublishOn { get; set; }
		bool Published { get; set; }
	}
}
