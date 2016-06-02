using Informa.Library.Globalization;
using Jabberwocky.Glass.Autofac.Attributes;
using System;
using Jabberwocky.Core.Caching;

namespace Informa.Library.Site
{
	[AutowireService(LifetimeScope.PerScope)]
	public class SiteMaintenanceContext : ISiteMaintenanceContext
	{
		protected readonly ISiteRootContext SiteRootContext;
		protected readonly ITextTranslator TextTranslator;
        protected readonly ICacheProvider CacheProvider;

        public SiteMaintenanceContext(
			ISiteRootContext siteRootContext,
			ITextTranslator textTranslator,
            ICacheProvider cacheProvider)
		{
			SiteRootContext = siteRootContext;
			TextTranslator = textTranslator;
            CacheProvider = cacheProvider;

            Info = GetSiteMaintenanceInfo();
		}

		public ISiteMaintenanceInfo Info { get; set; }
		protected string DefaultMessageKey => "MaintenanceMessage";

	    private ISiteMaintenanceInfo GetSiteMaintenanceInfo()
	    {
            string cacheKey = $"{nameof(SiteMaintenanceContext)}-Info-{SiteRootContext.Item._Id}";
            return CacheProvider.GetFromCache(cacheKey, BuildSiteMaintenanceInfo);
        }
        
        private ISiteMaintenanceInfo BuildSiteMaintenanceInfo()
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
