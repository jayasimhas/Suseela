using Informa.Library.Globalization;
using Jabberwocky.Glass.Autofac.Attributes;
using System;

namespace Informa.Library.Site
{
	[AutowireService(LifetimeScope.SingleInstance)]
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

				return new SiteMaintenanceInfo
				{
					From = siteRoot == null ? default(DateTime) : siteRoot.System_Maintenance_Start_Date,
					To = siteRoot == null ? default(DateTime) : siteRoot.System_Maintenance_End_Date,
					Message = siteRoot == null ? TextTranslator.Translate(DefaultMessageKey) : siteRoot.System_Maintenance_Text
				};
			}
		}

		protected string DefaultMessageKey => "MaintenanceMessage";
	}
}
