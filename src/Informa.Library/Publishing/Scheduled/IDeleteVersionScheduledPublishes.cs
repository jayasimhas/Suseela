using System;

namespace Informa.Library.Publishing.Scheduled
{
	public interface IDeleteVersionScheduledPublishes
	{
		void Delete(Guid itemId, string language, string version);
	}
}
