using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Informa.Library.Services.NlmExport.Models.Front.Article;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;

namespace Informa.Library.Utilities.AutoMapper.Resolvers.Nlm.Front.Article
{
    public class ArticlePublisherIdResolver : BaseValueResolver<ArticleItem, List<NlmArticleIdModel>>
    {
        protected override List<NlmArticleIdModel> Resolve(ArticleItem source, ResolutionContext context)
        {
            var articleNumber = source.Article_Number;

            return new List<NlmArticleIdModel>
            {
                new NlmArticleIdModel
                {
                    IdType = "publisher-id",
                    Value = articleNumber
                },
                new NlmArticleIdModel
                {
                    IdType = "pii",
                    Value = articleNumber
                }
            };
        }
    }
}
