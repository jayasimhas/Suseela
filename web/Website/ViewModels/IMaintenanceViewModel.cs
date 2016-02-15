using System;
using System.Web;

namespace Informa.Web.ViewModels
{
	public interface IMaintenanceViewModel
	{
		IHtmlString Message { get; }
		string DismissText { get; }
		DateTime DisplayFrom { get; }
		DateTime DisplayTo { get; }
		bool Display { get; }
		string Id { get; }
	}
}
