using Informa.Library.Globalization;
using Jabberwocky.Glass.Autofac.Attributes;
using System;

namespace Informa.Library.Site
{
	[AutowireService(LifetimeScope.PerScope)]
	public class SiteMaintenanceContext : ISiteMaintenanceContext
	{
		protected readonly ISiteRootContext SiteRootContext;
		protected readonly ITextTranslator TextTranslator;

		public SiteMaintenanceContext(
			ISiteRootContext siteRootContext,
			ITextTranslator textTranslator)
		{
			SiteRootContext = siteRootContext;
			TextTranslator = textTranslator;
		}

		public ISiteMaintenanceInfo Info
		{
			get
			{
				var siteRoot = SiteRootContext.Item;
				var from = siteRoot?.System_Maintenance_Start_Date ?? default(DateTime);
				var to = siteRoot?.System_Maintenance_End_Date ?? default(DateTime);
				var message = siteRoot.System_Maintenance_Text ?? TextTranslator.Translate(DefaultMessageKey);
				var id = string.Concat(from.ToString("yyyyMMddHHmmss"), to.ToString("yyyyMMddHHmmss"));
				var show = from == default(DateTime) && to == default(DateTime) ? false : from <= DateTime.Now && to >= DateTime.Now;

				return new SiteMaintenanceInfo
				{
					From = from,
					To = to,
					Message = message,
					Id = id,
					Show = show
				};
			}
		}

		protected string DefaultMessageKey => "MaintenanceMessage";
	}
}
