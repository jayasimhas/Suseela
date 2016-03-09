using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Informa.Library.Services.NlmExport.Models.Front.Article;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;

namespace Informa.Library.Utilities.AutoMapper.Resolvers.Nlm.Front.Article
{
    public class ArticlePermissionsResolver : BaseValueResolver<ArticleItem, NlmArticlePermissionsModel>
    {
        protected override NlmArticlePermissionsModel Resolve(ArticleItem source, ResolutionContext context)
        {
            var year = DateTime.UtcNow.Year.ToString();

            return new NlmArticlePermissionsModel
            {
                CopyrightStatement = "",
                CopyrightYear = year // maybe override this later? I think we can rely on just setting this during mapping
            };
        }
    }
}
