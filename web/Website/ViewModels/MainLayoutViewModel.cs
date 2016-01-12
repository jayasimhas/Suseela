using Glass.Mapper.Sc.Fields;
using Informa.Library.Globalization;
using Informa.Library.Site;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Jabberwocky.Glass.Models;

namespace Informa.Web.ViewModels
{
	public class MainLayoutViewModel : GlassViewModel<IGlassBase>
	{
		protected readonly ISiteRootContext SiteRootContext;
		protected readonly ISiteMaintenanceContext SiteMaintenanceContext;
		protected readonly ITextTranslator TextTranslator;

		public MainLayoutViewModel(
			ISiteRootContext siteRootContext,
			ISiteMaintenanceContext siteMaintenanceContext,
			ITextTranslator textTranslator)
		{
			SiteRootContext = siteRootContext;
			SiteMaintenanceContext = siteMaintenanceContext;
			TextTranslator = textTranslator;
		}

		public IMaintenanceViewModel MaintenanceMessage
		{
			get
			{
				var siteMaintenanceInfo = SiteMaintenanceContext.Info;

				return new MaintenanceViewModel
				{
					DismissText = TextTranslator.Translate("MaintenanceDismiss"),
					DisplayFrom = siteMaintenanceInfo.From,
					DisplayTo = siteMaintenanceInfo.To,
					Message = siteMaintenanceInfo.Message
				};
			}
		}

		public string FooterLogoUrl => SiteRootContext.Item == null ? string.Empty : SiteRootContext.Item.Footer_Logo.Src;

		public string CopyrightText => SiteRootContext.Item == null ? string.Empty : SiteRootContext.Item.Copyright_Text;

		// Subscribe Link - business logic

		// Local footer links

		public string FollowText => TextTranslator.Translate("FooterFollow");

		public Link LinkedInLink => SiteRootContext.Item == null ? null : SiteRootContext.Item.LinkedIn_Link;

		public Link FacebookLink => SiteRootContext.Item == null ? null : SiteRootContext.Item.Facebook_Link;

		public Link TwitterLink => SiteRootContext.Item == null ? null : SiteRootContext.Item.Twitter_Link;

		public string MenuOneHeader => SiteRootContext.Item == null ? string.Empty : SiteRootContext.Item.Menu_One_Header;

		// Menu one links

		public string MenuTwoHeader => SiteRootContext.Item == null ? string.Empty : SiteRootContext.Item.Menu_Two_Header;
	}
}