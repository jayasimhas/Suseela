using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using Informa.Library.Search.ComputedFields.SearchResults.Converter;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Links;

namespace Informa.Library.Search.Results
{
	public class InformaSearchResultItem : SearchResultItem
	{
		[IndexField("_val_")]
		public string Val { get; set; }

		[IndexField("issearchable_b")]
		public bool IsSearchable { get; set; }

		[IndexField("facetarticleinprogress_b")]
		public bool InProgress { get; set; }

		[IndexField("_latestversion")]
		public bool IsLatestVersion { get; set; }

		[IndexField("searchdate")]
		[DataMember]
		public DateTime SearchDate { get; set; }

		[IndexField("plannedpublishdate")]
		[DataMember]
		public DateTime PlannedPublishDate { get; set; }
		
		[IndexField("searchtitle")]
		[DataMember]
		public string Title { get; set; }

		[IndexField("sub_title_t")]
		[DataMember]
		public string SubTitle { get; set; }

		[IndexField("searchpublicationtitle_s")]
		[DataMember]
		public string PublicationTitle { get; set; }

		[IndexField("searchbyline_s")]
		[DataMember]
		public string Byline { get; set; }

		[IndexField("facetcontenttype_s")]
		[DataMember]
		public string ContentType { get; set; }

		[IndexField("searchmediaicon_s")]
		[DataMember]
		public string MediaType { get; set; }

		[IndexField("searchsummary_s")]
		[DataMember]
		public string Summary { get; set; }

		[TypeConverter(typeof(HtmlLinkListTypeConverter))]
		[IndexField("searchdisplaytaxonomy_s")]
		[DataMember]
		public HtmlLinkList SearchDisplayTaxonomy { get; set; }

		[DataMember]
		public bool IsArticleBookmarked { get; set; }

		[DataMember]
		public bool IsUserAuthenticated { get; set; }

		[DataMember]
		public new string Url
		{
			get
			{
				var options = LinkManager.GetDefaultUrlOptions();
				options.SiteResolving = true;

				return LinkManager.GetItemUrl(GetItem(), options);
			}
		}
	}
}
