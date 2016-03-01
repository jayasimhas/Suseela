using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Informa.Library.Search.ComputedFields.SearchResults;
using Informa.Library.Search.ComputedFields.SearchResults.Converter;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Converters;
using Sitecore.ContentSearch.SearchTypes;

namespace Informa.Library.Search.Results
{
    public class InformaSearchResultItem : SearchResultItem
    {
        [IndexField("issearchable_b")]
        public bool IsSearchable { get; set; }

        [IndexField("_latestversion")]
        public bool IsLatestVersion { get; set; }

        [IndexField("searchdate")]
        [DataMember]
        public DateTime SearchDate { get; set; }

        [IndexField("searchurl_s")]
        [DataMember]
        public new string Url { get; set; }

        [IndexField("searchtitle_s")]
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
    }


}
