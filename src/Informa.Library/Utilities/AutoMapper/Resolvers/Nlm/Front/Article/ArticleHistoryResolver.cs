using System;
using AutoMapper;
using Informa.Library.Services.NlmExport.Models.Front.Article.History;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;

namespace Informa.Library.Utilities.AutoMapper.Resolvers.Nlm.Front.Article
{
    public class ArticleHistoryResolver : BaseValueResolver<ArticleItem, NlmHistoryDateModel[]>
    {
        private const string HistoryDateType = "accepted";

        protected override NlmHistoryDateModel[] Resolve(ArticleItem source, ResolutionContext context)
        {
            var updated = source?.Modified_Date;

            var day = updated?.Day.ToString();
            var month = updated?.Month.ToString();
            var year = updated?.Year.ToString();

            return new[]
            {
                new NlmHistoryDateModel
                {
                    DateType = HistoryDateType,
                    Year = year,
                    Month = month,
                    Day = day
                },
            };
        }
    }
}
