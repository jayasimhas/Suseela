using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Maps;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Maps;
using Informa.Models.Informa.Models.sitecore.templates.Common;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Jabberwocky.Glass.Models;
using Sitecore.Buckets.Extensions;

namespace Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages
{
	public partial interface IArticle : IPublicationChild
	{   
		//TODO
		bool IsPublished { get ; set; }

		/*[SitecoreField(I___BaseTaxonomyConstants.TaxonomiesFieldName)]
		IEnumerable<Guid> TaxonomyIDs { get; set; } */
	}

	public class ArticleMap : SitecoreGlassMap<IPublicationChild>
	{
		public override void Configure()
		{
			Map(x => x.Delegate(y => y.Publication).GetValue(z => new Guid("{3818C47E-4B75-4305-8F01-AB994150A1B0}")));

			//Map(x => x.Delegate(y => y.Publication).GetValue(z => z.Item.Paths.LongID.Split('/')
			//	.Select(tag => z.Service.GetItem<IGlassBase>(tag, true, true)).OfType<ISite_Root>().FirstOrDefault()?._Id));		
		}
	}

	public interface IPublicationChild
	{
		Guid Publication { get; }
	}
}
