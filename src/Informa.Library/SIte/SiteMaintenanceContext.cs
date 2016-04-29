﻿using Informa.Library.Globalization;
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

			_info = new Lazy<ISiteMaintenanceInfo>(GetSiteMaintenanceInfo);
		}

		private readonly Lazy<ISiteMaintenanceInfo> _info;
		public ISiteMaintenanceInfo Info => _info.Value;
		protected string DefaultMessageKey => "MaintenanceMessage";

		private ISiteMaintenanceInfo GetSiteMaintenanceInfo()
		{
			var siteRoot = SiteRootContext.Item;
			var from = siteRoot?.System_Maintenance_Start_Date ?? default(DateTime);
			var to = siteRoot?.System_Maintenance_End_Date ?? default(DateTime);
			var message = siteRoot?.System_Maintenance_Text ?? TextTranslator.Translate(DefaultMessageKey);
			var id = string.Concat(from.ToString("yyyyMMddHHmmss"), to.ToString("yyyyMMddHHmmss"));
			var show = (@from != default(DateTime) || to != default(DateTime)) && (@from <= DateTime.Now && to >= DateTime.Now);

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
}
