using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Maps;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using Glass.Mapper.Sc.Maps;
using Informa.Models.FactoryInterface;
using Informa.Models.GlassModels;
using Informa.Models.Informa.Models.sitecore.templates.Common;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Jabberwocky.Glass.Models;
using Sitecore.Buckets.Extensions;
using Sitecore.Globalization;

namespace Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages
{
	public partial interface IArticle : IPublicationChild
	{
		bool IsPublished { get; set; }
	}

	public class ArticleMap : SitecoreGlassMap<IPublicationChild>
	{
		public override void Configure()
		{
			// Map(x => x.AutoMap(),x => x.Delegate(y => y.Publication).GetValue(z => new Guid("{3818C47E-4B75-4305-8F01-AB994150A1B0}")));

			//Map(x => x.Delegate(y => y.Publication).GetValue(z => z.Item.Paths.LongID.Split('/')
			//	.Select(tag => z.Service.GetItem<IGlassBase>(tag, true, true)).OfType<ISite_Root>().FirstOrDefault()?._Id));		
		}
	}
    
	public class ArticleItem : GlassBase, IArticle__Raw
	{
	    public Guid Publication => new Guid("{3818C47E-4B75-4305-8F01-AB994150A1B0}");
		public Guid _Id { get; set; }
		public Language _Language { get; set; }
		public int _Version { get; set; }
		public string _Url { get; set; }
	    public Guid _TemplateId { get; set; } = IArticleConstants.TemplateId.Guid;
			//= new Guid(IArticleConstants.TemplateIdString);
		public string _Path { get; set; }
		public string _Name { get; set; }
		public IEnumerable<IGlassBase> _ChildrenWithInferType { get; set; }
		public IGlassBase _Parent { get; set; }
		public string _MediaUrl { get; set; }
		public IEnumerable<Guid> _BaseTemplates { get; set; }
		public string LinkableText { get; }
		public string LinkableUrl { get; }
		public IEnumerable<ILinkable> ListableAuthors { get; }
		public DateTime ListableDate { get; }
		public string ListableImage { get; }
		public string ListableSummary { get; }
		public string ListableTitle { get; }
		public string ListableByline { get; }
		public IEnumerable<ILinkable> ListableTopics { get; }
		public string ListableType { get; }
		public string Category { get; }
		public IEnumerable<ITaxonomy_Item> LongIds { get; }
		public IEnumerable<ITaxonomy_Item> Taxonomies { get; set; }
		public string Custom_Meta_Tags { get; set; }
		public string Meta_Description { get; set; }
		public string Meta_Keywords { get; set; }
		public string Meta_Title_Override { get; set; }
		public string Navigation_Title { get; set; }
		//[SitecoreField()]
		public string Body { get; set; }
		public string Sub_Title { get; set; }
		public string Title { get; set; }
		public IEnumerable<IGlassBase> FactoryListableAuthors { get; set; }
		public IEnumerable<IGlassBase> FactoryListableDates { get; set; }
		public IEnumerable<IGlassBase> FactoryListableImages { get; set; }
		public IEnumerable<IGlassBase> FactoryListableSummaries { get; set; }
		public IEnumerable<IGlassBase> FactoryListableTitles { get; set; }
		public IEnumerable<IGlassBase> FactoryListableTopics { get; set; }
		public IEnumerable<IGlassBase> FactoryListableUrls { get; set; }
		public bool IsPublished { get; set; }
		public float Sort_Order { get; set; }
		public DateTime Actual_Publish_Date { get; set; }
		public string Article_Number { get; set; }
		public ITaxonomy_Item Content_Type { get; set; }
		public DateTime Created_Date { get; set; }
		public string Editorial_Notes { get; set; }
		public bool Embargoed { get; set; }
		public bool Free_Article { get; set; }
		public bool Is_Sidebar_Article { get; set; }
		public Guid Label { get; set; }
		public ITaxonomy_Item Media_Type { get; set; }
		public DateTime Modified_Date { get; set; }
		public DateTime Planned_Publish_Date { get; set; }
		public Link Word_Document { get; set; }
		public string Article_Category { get; set; }
		public IEnumerable<IGlassBase> Child_Articles { get; set; }
		public string Escenic_ID { get; set; }
		public IEnumerable<IGlassBase> Legacy_Publications { get; set; }
		public Image Featured_Image_16_9 { get; set; }
		public string Featured_Image_Caption { get; set; }
		public string Featured_Image_Source { get; set; }
		public string Summary { get; set; }
		public IEnumerable<IAuthor> Authors { get; set; }
		public string Section_Review { get; set; }
		public string Start_Page { get; set; }
		public string Word_Count { get; set; }
		public IEnumerable<IArticle> Referenced_Articles { get; set; }
		public string Referenced_Companies { get; set; }
		public string Referenced_Deals { get; set; }
		public IEnumerable<IArticle> Related_Articles { get; set; }
		public IEnumerable<IGlassBase> Supporting_Documents { get; set; }
		public bool Scheduled_Publishing_Enabled { get; set; }
        public bool Include_In_Search { get; set; }
		public Link Canonical_Link { get; set; }
		public Guid Workflow { get; set; }
		public Guid State { get; set; }

		public string Notification_Text { get; set; }
		
	}

	public interface IPublicationChild
	{
		Guid Publication { get; }
	}
}
