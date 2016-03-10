using AutoMapper;
using Informa.Library.Services.NlmExport.Models.Front.Article;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;

namespace Informa.Library.Utilities.AutoMapper.Resolvers.Nlm.Front.Article
{
    public class ArticlePubDateResolver : BaseValueResolver<ArticleItem, NlmArticlePubDateModel>
    {
        protected override NlmArticlePubDateModel Resolve(ArticleItem source, ResolutionContext context)
        {
            var actualPublishDate = source?.Actual_Publish_Date;

            var day = actualPublishDate?.Day.ToString();
            var month = actualPublishDate?.Month.ToString();
            var year = actualPublishDate?.Year.ToString();

            return new NlmArticlePubDateModel
            {
                DateType = "epub", // Apparently this value can be overridden manually
                Day = day,
                Month = month,
                Year = year
            };
        }
    }
}
