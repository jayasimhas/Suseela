using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Sc.Configuration.Attributes;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Virtual_Whiteboard;
using Jabberwocky.Glass.Models;

namespace Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages
{
	public partial interface IArticle
	{
		[SitecoreField("__Source")]
		string SourceId { get; set; }
	}

	public partial interface IArticle__Raw
	{
		[SitecoreField("__Sortorder")]
		float Sortorder { get; set; }
	}
}
