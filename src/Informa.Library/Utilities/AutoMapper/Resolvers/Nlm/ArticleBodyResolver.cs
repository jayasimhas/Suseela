using AutoMapper;
using Informa.Library.Services.NlmExport.Models.Body;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;

namespace Informa.Library.Utilities.AutoMapper.Resolvers.Nlm
{
    public class ArticleBodyResolver : ValueResolver<ArticleItem, NlmArticleBodyModel>
    {
        // TODO: Convert to straight-up IValueResolver, and simply perform an AutoMap with the ResolutionContext in 'Resolve'
        protected override NlmArticleBodyModel ResolveCore(ArticleItem source)
        {
            return new NlmArticleBodyModel();
        }
    }
}
