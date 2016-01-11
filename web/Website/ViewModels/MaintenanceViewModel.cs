using System;

namespace Informa.Web.ViewModels
{
	public class MaintenanceViewModel : IMaintenanceViewModel
	{
		public string Message { get; set; }
		public string DismissText { get; set; }
		public DateTime DisplayFrom { get; set; }
		public DateTime DisplayTo { get; set; }
	}
}