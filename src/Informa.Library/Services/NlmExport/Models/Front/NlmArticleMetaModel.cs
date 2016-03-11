using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Informa.Library.Services.NlmExport.Models.Common;
using Informa.Library.Services.NlmExport.Models.Front.Article;
using Informa.Library.Services.NlmExport.Models.Front.Article.History;

namespace Informa.Library.Services.NlmExport.Models.Front
{
    [XmlRoot("article-meta")]
    [Serializable]
    public class NlmArticleMetaModel
    {
        [XmlElement("article-id")]
        public List<NlmArticleIdModel> PublisherId { get; set; }

        [XmlArray("article-categories")]
        [XmlArrayItem("subj-group", typeof(NlmCategoryModel))]
        public NlmCategoryModel[] Categories { get; set; }

        [XmlElement("title-group")]
        public NlmArticleTitleModel TitleGroup { get; set; }

        [XmlArray("contrib-group")]
        [XmlArrayItem("contrib", typeof(NlmArticleAuthorModel))]
        public NlmArticleAuthorModel[] Contributors { get; set; }

        [XmlElement("pub-date")]
        public NlmArticlePubDateModel PubDate { get; set; }

        [XmlElement("volume")]
        public string Volume { get; set; }

        [XmlElement("issue")]
        public string Issue { get; set; }

        [XmlArray("history")]
        [XmlArrayItem("date", typeof(NlmHistoryDateModel))]
        public NlmHistoryDateModel[] History { get; set; }

        [XmlElement("permissions")]
        public NlmArticlePermissionsModel Permissions { get; set; }

        [XmlElement("related-article")]
        public List<NlmRelatedArticleModel> RelatedArticles { get; set; }

        [XmlElement("abstract")]
        public List<NlmArticleAbstractModel> Abstracts { get; set; }

        //[XmlElement("abstract")]
        //public NlmArticleAbstractModel LongAbstract { get; set; }

        //[XmlElement("abstract")]
        //public NlmArticleAbstractModel DeckAbstract { get; set; }

        [XmlArray("counts")]
        [XmlArrayItem("word-count", typeof(NlmArticleWordCountModel))]
        public NlmArticleWordCountModel[] Counts { get; set; }

        [XmlArray("custom-meta-group")]
        [XmlArrayItem("custom-meta", typeof(NlmArticleCustomMetaModel))]
        public NlmArticleCustomMetaModel[] CustomMetaGroup { get; set; }
    }
}
