using Informa.Library.Globalization;
using Informa.Library.Site;
using Jabberwocky.Glass.Autofac.Attributes;
using System;
using System.Web;
using System.Web.Mvc;

namespace Informa.Web.ViewModels
{
	[AutowireService(LifetimeScope.PerScope)]
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

		public IHtmlString Message => new MvcHtmlString(SiteMaintenanceContext.Info.Message);
		public string DismissText => TextTranslator.Translate("Maintenance.MaintenanceDismiss");
		public DateTime DisplayFrom => SiteMaintenanceContext.Info.From;
		public DateTime DisplayTo => SiteMaintenanceContext.Info.To;
		public bool Display => SiteMaintenanceContext.Info.Show;
		public string Id => SiteMaintenanceContext.Info.Id;
	}
}