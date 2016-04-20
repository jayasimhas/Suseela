using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using Informa.Library.Search.ComputedFields.SearchResults.Converter;
using Informa.Library.Utilities.Extensions;
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

		[IndexField("facetcompanies")]
		public List<string> CompaniesFacet { get; set; }
			
		[DataMember]
		public bool IsArticleBookmarked { get; set; }

		[DataMember]
		public bool IsUserAuthenticated { get; set; }

		[DataMember]
		public new string Url
		{
			get { return $"{SiteUrl}{LinkManager.GetItemUrl(GetItem())}"; }
		}

		[DataMember]
		public string SiteUrl
		{
			get
			{
				var options = LinkManager.GetDefaultUrlOptions();
				options.SiteResolving = true;
				options.AlwaysIncludeServerUrl = true;

				var item = GetItem();
				var site = item.GetSite();
				var home = item.Database.GetItem($"{site.RootPath}{site.StartItem}");
				return LinkManager.GetItemUrl(home, options).TrimEnd('/');
			}
		}
	}
}
