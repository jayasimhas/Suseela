using Informa.Library.Globalization;
using Informa.Library.Site;
using Jabberwocky.Glass.Autofac.Attributes;
using System;

namespace Informa.Web.ViewModels
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class MaintenanceViewModel : IMaintenanceViewModel
	{
		protected readonly ISiteMaintenanceContext SiteMaintenanceContext;
		protected readonly ITextTranslator TextTranslator;

		public MaintenanceViewModel(
			ISiteMaintenanceContext siteMaintenanceContext,
			ITextTranslator textTranslator)
		{
			SiteMaintenanceContext = siteMaintenanceContext;
			TextTranslator = textTranslator;
		}

		public string Message => SiteMaintenanceContext.Info.Message;
		public string DismissText => TextTranslator.Translate("Maintenance.MaintenanceDismiss");
		public DateTime DisplayFrom => SiteMaintenanceContext.Info.From;
		public DateTime DisplayTo => SiteMaintenanceContext.Info.To;
		public bool Display => DisplayFrom <= DateTime.Now && DisplayTo >= DateTime.Now;
	}
}