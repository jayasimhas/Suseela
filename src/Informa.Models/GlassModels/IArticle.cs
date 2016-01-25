using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Maps;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Maps;
using Informa.Models.FactoryInterface;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Jabberwocky.Glass.Models;
using Sitecore.Buckets.Extensions;
using Sitecore.Data.Items;

namespace Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages
{
	public partial interface IArticle
    {

        [SitecoreQuery("./ancestor-or-self::*[@@templateid='{110D559F-DEA5-42EA-9C1C-8A5DF7E70EF9}']", IsRelative = true)]
        Guid Publication { get; set; }
		
		//TODO
		bool IsPublished { get ; set; }

		/*[SitecoreField(I___BaseTaxonomyConstants.TaxonomiesFieldName)]
		IEnumerable<Guid> TaxonomyIDs { get; set; } */
	}

	public class ArticleMap : SitecoreGlassMap<IArticle>
	{
		public override void Configure()
		{
			Map(x => x.AutoMap(), x => x.Delegate(y => y.IsPublished).GetValue(z =>
			{
				return z.Item.IsPublished(z.Service.Database);
			}));
		}
	}
}
