using System;

namespace Informa.Web.ViewModels
{
	public interface IMaintenanceViewModel
	{
		string Message { get; }
		string DismissText { get; }
		DateTime DisplayFrom { get; }
		DateTime DisplayTo { get; }
		bool Display { get; }
		string Id { get; }
	}
}
