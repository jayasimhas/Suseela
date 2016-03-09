using System.Linq;
using AutoMapper;
using Informa.Library.Services.NlmExport.Models.Front.Article;
using Informa.Library.Services.NlmExport.Models.Front.Article.Contrib;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;

namespace Informa.Library.Utilities.AutoMapper.Resolvers.Nlm.Front.Article
{
    public class ArticleContributorResolver : BaseValueResolver<ArticleItem, NlmArticleAuthorModel[]>
    {
        protected override NlmArticleAuthorModel[] Resolve(ArticleItem source, ResolutionContext context)
        {
            var authors = source?.Authors?.ToArray();
            if (authors == null || !authors.Any()) return null;

            return authors.Select(author => new NlmArticleAuthorModel
            {
                Type = "author",
                Name = new NlmArticleAuthorNameModel
                {
                    Surname = author.Last_Name,
                    GivenNames = author.First_Name
                },
                Email = new NlmAuthorEmailModel
                {
                    Href = author.Email_Address
                }
            }).ToArray();
        }
    }
}
