using AutoMapper;
using Informa.Library.Services.NlmExport.Models.Front.Article;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;

namespace Informa.Library.Utilities.AutoMapper.Resolvers.Nlm.Front.Article
{
    public class ArticleCustomMetaResolver : BaseValueResolver<ArticleItem, NlmArticleCustomMetaModel[]>
    {
        private const string DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss'.'fffffff";

        protected override NlmArticleCustomMetaModel[] Resolve(ArticleItem source, ResolutionContext context)
        {
            if (source == null) return null;

            var actualPublishDate = source.Actual_Publish_Date.ToString(DateTimeFormat);

            return new[]
            {
                new NlmArticleCustomMetaModel
                {
                    MetaName = "publish_datetime",
                    MetaValue = actualPublishDate
                }, 
                new NlmArticleCustomMetaModel
                {
                    MetaName = "lastupdated_datetime",
                    MetaValue = source.Updated.ToString(DateTimeFormat)
                },
                new NlmArticleCustomMetaModel
                {
                    MetaName = "created_datetime",
                    MetaValue = source.Created_Date.ToString(DateTimeFormat)
                }, 
                new NlmArticleCustomMetaModel
                {
                    MetaName = "end_datetime",
                    MetaValue = "2999-12-31 00:00:00.000"
                }, 
                new NlmArticleCustomMetaModel
                {
                    MetaName = "modified_datetime",
                    MetaValue = source.Modified_Date.ToString(DateTimeFormat)
                }, 
                new NlmArticleCustomMetaModel
                {
                    MetaName = "story_datetime",
                    MetaValue = actualPublishDate
                }, 
                new NlmArticleCustomMetaModel
                {
                    MetaName = "issue_date",
                    MetaValue = actualPublishDate
                }, 
                new NlmArticleCustomMetaModel
                {
                    MetaName = "article_size",
                    MetaValue = string.Empty
                },
                new NlmArticleCustomMetaModel
                {
                    MetaName = "content_type",
                    MetaValue = "Article"
                }
            };
        }
    }
}
