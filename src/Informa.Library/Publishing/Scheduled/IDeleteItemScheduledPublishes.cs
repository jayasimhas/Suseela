using System;

namespace Informa.Library.Publishing.Scheduled
{
	public interface IDeleteItemScheduledPublishes
	{
		void Delete(Guid itemId);
	}
}
