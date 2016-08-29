using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Web.Mvc;
using Informa.Library.Search.ComputedFields.SearchResults.Converter;
using Informa.Library.Search.ComputedFields.SearchResults.Converter.DisplayTaxonomy;
using Informa.Library.User.Document;
using Informa.Library.Utilities.TokenMatcher;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Links;

namespace Informa.Library.Search.Results
{
	public class InformaSearchResultItem : SearchResultItem
	{
		/// <summary>
		/// This property is used for boosting newer articles
		/// See http://www.sitecoreblogger.com/2014/09/publication-date-boosting-in-sitecore-7.html
		/// </summary>
		[IndexField("_val_")]
		public string Val { get; set; }

		[IndexField("issearchable_b")]
		public bool IsSearchable { get; set; }

		[IndexField("facetarticleinprogress_b")]
		public bool InProgress { get; set; }

		[IndexField("_latestversion")]
		public bool IsLatestVersion { get; set; }

		[IndexField("sort_order_tf")]
		public float SortOrder { get; set; }

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

        [IndexField("searchpublicationcode_s")]
        [DataMember]
        public string PublicationCode { get; set; }

        [IndexField("searchbyline_t")]
		[DataMember]
		public string Byline { get; set; }

		[IndexField("facetcontenttype_s")]
		[DataMember]
		public string ContentType { get; set; }

		[IndexField("searchmediaicon_s")]
		[DataMember]
		public string MediaType { get; set; }

        [IndexField("searchmediatooltip_s")]
        [DataMember]
        public string MediaTooltip { get; set; }

        [DataMember]
		public string Summary => DCDTokenMatchers.ProcessDCDTokens(RawSummary);

		[IndexField("searchsummary_s")]
		public string RawSummary { get; set; }

		[TypeConverter(typeof(HtmlLinkListTypeConverter))]
		[IndexField("searchdisplaytaxonomy_s")]
		[DataMember]
		public HtmlLinkList SearchDisplayTaxonomy { get; set; }

		[IndexField("facetcompanies")]
		public List<string> CompaniesFacet { get; set; }

		[IndexField("authors_sm")]
		public List<string> Authors { get; set; }

        [IndexField("referenced_companies_t")]
        public string ReferencedCompany { get; set; }
        [DataMember]
		public bool IsArticleBookmarked => DocumentContext.IsSaved(ItemId.Guid);
		
		[DataMember]
		public new string Url
		{
			get
			{
				var options = LinkManager.GetDefaultUrlOptions();
				options.SiteResolving = true;
				options.AlwaysIncludeServerUrl = true;

				var item = GetItem();
				return item != null ? LinkManager.GetItemUrl(item, options) : base.Url;
			}
		}

		protected IIsSavedDocumentContext DocumentContext => DependencyResolver.Current.GetService<IIsSavedDocumentContext>();
	}
}
