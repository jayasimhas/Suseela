using System.Web;
using AutoMapper;
using Informa.Library.Services.NlmExport.Models.Front.Article;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;

namespace Informa.Library.Utilities.AutoMapper.Resolvers.Nlm.Front.Article
{
    public class ArticleTitleResolver : BaseValueResolver<ArticleItem, NlmArticleTitleModel>
    {
        protected override NlmArticleTitleModel Resolve(ArticleItem source, ResolutionContext context)
        {
            var title = HttpUtility.HtmlDecode(source?.Title ?? string.Empty);
            var subTitle = HttpUtility.HtmlDecode(source?.Sub_Title ?? string.Empty);

            return new NlmArticleTitleModel
            {
                Title = title,
                SubTitle = subTitle
            };
        }
    }
}
