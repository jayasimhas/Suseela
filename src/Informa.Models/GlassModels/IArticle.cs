using System;
using System.Collections.Generic;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using Glass.Mapper.Sc.Maps;
using Informa.Models.FactoryInterface;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Jabberwocky.Glass.Models;
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
        }
    }

    public class ArticleItem : GlassBase, IArticle__Raw
    {
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
        public bool IsPublished { get { return this.Actual_Publish_Date != DateTime.MinValue && this.Actual_Publish_Date < DateTime.Now; } }
        public float Sort_Order { get; set; }
        public DateTime Actual_Publish_Date { get; set; }
        public string Article_Number { get; set; }
        public ITaxonomy_Item Content_Type { get; set; }
        public DateTime Created_Date { get; set; }
        public string Editorial_Notes { get; set; }
        public bool Embargoed { get; set; }
        public bool Free { get; set; }
        public bool Free_With_Registration { get; set; }
        public bool Is_Sidebar_Article { get; set; }
        public Guid Label { get; set; }
        public ITaxonomy_Item Media_Type { get; set; }
        public DateTime Modified_Date { get; set; }
        public DateTime Planned_Publish_Date { get; set; }
        public Link Word_Document { get; set; }
        public string Article_Category { get; set; }
        public IEnumerable<IGlassBase> Child_Articles { get; set; }
        public string Escenic_ID { get; set; }
        public string Legacy_Article_Number { get; set; }
        public string Legacy_Article_Url { get; set; }
        public IEnumerable<IGlassBase> Legacy_Publications { get; set; }
        public string Legacy_Sitecore_ID { get; set; }
        public Image Featured_Image_16_9 { get; set; }
        public string Featured_Image_Caption { get; set; }
        public string Featured_Image_Source { get; set; }
        public string Summary { get; set; }
        public IEnumerable<IStaff_Item> Authors { get; set; }
        public string Section_Review { get; set; }
        public string Start_Page { get; set; }
        public string Word_Count { get; set; }
        public IEnumerable<IGlassBase> Referenced_Articles { get; set; }
        public string Referenced_Companies { get; set; }
        public string Referenced_Deals { get; set; }
        public IEnumerable<IGlassBase> Related_Articles { get; set; }
        public IEnumerable<IGlassBase> Editors_Picks { get; set; }
        public IEnumerable<IGlassBase> Supporting_Documents { get; set; }
        public bool Scheduled_Publishing_Enabled { get; set; }
        public bool Include_In_Search { get; set; }
        public Link Canonical_Link { get; set; }
        public Guid Workflow { get; set; }
        public Guid State { get; set; }

        public string Notification_Text { get; set; }
        public string Leaderboard_Slot_ID { get; set; }
        public string Article_Medium_Slot_ID { get; set; }
		public float Sortorder { get; set; }
		public string Article_Filmstrip_Slot_ID { get; set; }
        public string Article_Rectangular_Slot_ID2 { get; set; }
        public string Article_Rectangular_Slot_ID3 { get; set; }
        public string Leaderboard_Slot_ID2 { get; set; }
        [SitecoreField("__updated")]
        public virtual DateTime Updated { get; set; }
    }

    public interface IPublicationChild
    {
        Guid Publication { get; }
    }
}
