using AutoMapper;
using Informa.Library.Services.NlmExport.Models.Front.Article;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;

namespace Informa.Library.Utilities.AutoMapper.Resolvers.Nlm.Front.Article
{
    public class ArticleContributorResolver : BaseValueResolver<ArticleItem, NlmArticleAuthorModel[]>
    {
        protected override NlmArticleAuthorModel[] Resolve(ArticleItem source, ResolutionContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}
