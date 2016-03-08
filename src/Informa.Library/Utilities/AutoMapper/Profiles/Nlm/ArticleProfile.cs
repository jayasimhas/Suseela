using AutoMapper;
using Informa.Library.Services.NlmExport.Models;
using Informa.Library.Utilities.AutoMapper.Resolvers.Nlm;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;

namespace Informa.Library.Utilities.AutoMapper.Profiles.Nlm
{
    public class ArticleProfile : Profile
    {
        public override string ProfileName => "NlmArticleProfile";

        protected override void Configure()
        {
            CreateMap<ArticleItem, NlmArticleModel>()
                .ForMember(m => m.ArticleType, opt => opt.ResolveUsing<ArticleTypeResolver>())
                .ForMember(m => m.Body, opt => opt.ResolveUsing<ArticleBodyResolver>());
        }
    }
}
