using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;

namespace Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects
{
	public partial interface IPage_Assets
	{
		[SitecoreField("__Style", Setting = SitecoreFieldSettings.InferType)]
		string Style { get; set; }
	}
}
