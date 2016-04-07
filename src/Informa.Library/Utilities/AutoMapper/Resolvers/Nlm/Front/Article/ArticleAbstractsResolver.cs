using System.Collections.Generic;
using AutoMapper;
using Informa.Library.Services.NlmExport.Models.Front.Article;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;

namespace Informa.Library.Utilities.AutoMapper.Resolvers.Nlm.Front.Article
{
    public class ArticleAbstractsResolver : BaseValueResolver<ArticleItem, List<NlmArticleAbstractModel>>
    {
        protected override List<NlmArticleAbstractModel> Resolve(ArticleItem source, ResolutionContext context)
        {
            if (source == null) return null;

            //Load the summary into an HTMLAgilityPack's HtmlDoc to render and remove html tags
            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.LoadHtml(source.Summary);

            return new List<NlmArticleAbstractModel>
            {
                new NlmArticleAbstractModel
                {
                    AbstractType = "short",
                    Paragraph = htmlDoc.DocumentNode.InnerText
                }
            };
        }
    }
}
