using Sitecore.Data;

namespace Informa.Models.DCD
{
	public class DCDConstants
	{
		public const string CompanyTokenFormat = "[C#{0}:{1}]";

		public const string DealTokenRegex = @"\[W#(.*?)\]";
		public const string CompanyTokenRegex = @"\[C#(.*?)\]";
		public const string SidebarTokenRegex = @"\[Sidebar#(.*?)\]";
		public const string ArticleTokenRegex = @"\(<a[^>]*?>\[A#(.*?)\]</a>\)";
		public const string LegacyArticleTokenRegex = @"\[A#(.*?)\]";
        public const string DealCompanyNameRegex = @"\[(.*?)\]";
        public const string DealProductNameRegext = @"\{(.*?)\}";

		public static readonly string EmailNoReplySenderAddress = Sitecore.Configuration.Settings.GetSetting("Mail.MailServerFromAddress");
		public static readonly string BusinessAcronym = "IBI";

		public static readonly ID DCDConfigurationItemID = new ID(Sitecore.Configuration.Settings.GetSetting("DCDConfiguration"));
	}
}
