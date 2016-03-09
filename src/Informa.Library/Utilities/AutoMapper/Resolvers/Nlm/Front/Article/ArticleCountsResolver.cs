using AutoMapper;
using Informa.Library.Services.NlmExport.Models.Front.Article;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;

namespace Informa.Library.Utilities.AutoMapper.Resolvers.Nlm.Front.Article
{
    public class ArticleCountsResolver : BaseValueResolver<ArticleItem, NlmArticleWordCountModel[]>
    {
        protected override NlmArticleWordCountModel[] Resolve(ArticleItem source, ResolutionContext context)
        {
            if (source == null) return null;

            return new[]
            {
                new NlmArticleWordCountModel
                {
                    Count = source.Word_Count ?? "0"
                }
            };
        }
    }
}
