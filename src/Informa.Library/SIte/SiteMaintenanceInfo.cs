using System;

namespace Informa.Library.Site
{
	public class SiteMaintenanceInfo : ISiteMaintenanceInfo
	{
		public string Message { get; set; }
		public DateTime From { get; set; }
		public DateTime To { get; set; }
	}
}
