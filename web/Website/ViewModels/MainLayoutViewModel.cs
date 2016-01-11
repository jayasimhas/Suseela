using Glass.Mapper.Sc;
using Informa.Library.Globalization;
using Informa.Library.Site;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Jabberwocky.Glass.Models;

namespace Informa.Web.ViewModels
{
	public class MainLayoutViewModel : GlassViewModel<IGlassBase>
	{
		//protected readonly ISiteMaintenanceContext SiteMaintenanceContext;
		//protected readonly ITextTranslator TextTranslator;

		public MainLayoutViewModel(
			//ISiteMaintenanceContext siteMaintenanceContext,
			/*ITextTranslator textTranslator*/)
		{
			//SiteMaintenanceContext = siteMaintenanceContext;
			//TextTranslator = textTranslator;
		}

		public string TestString => "Test String";

		//public IMaintenanceViewModel MaintenanceMessage
		//{
		//	get
		//	{
		//		var siteMaintenanceInfo = SiteMaintenanceContext.Info;

		//		return new MaintenanceViewModel
		//		{
		//			DismissText = TextTranslator.Translate("MaintenanceDismiss"),
		//			DisplayFrom = siteMaintenanceInfo.From,
		//			DisplayTo = siteMaintenanceInfo.To,
		//			Message = siteMaintenanceInfo.Message
		//		};
		//	}
		//}
	}
}