using Glass.Mapper.Sc.Configuration.Attributes;

namespace Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates
{
	public partial interface I___BasePage
	{
		[SitecoreInfo(Type = Glass.Mapper.Sc.Configuration.SitecoreInfoType.Url, UrlOptions = Glass.Mapper.Sc.Configuration.SitecoreInfoUrlOptions.AlwaysIncludeServerUrl)]
		string _AbsoluteUrl { get; set; }
	}
}
