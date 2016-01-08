using System;

namespace Informa.Library.Site
{
	public interface ISiteMaintenanceInfo
	{
		string Message { get; }
		DateTime From { get; }
		DateTime To { get; }
	}
}
