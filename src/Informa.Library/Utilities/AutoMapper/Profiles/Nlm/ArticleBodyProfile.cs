using AutoMapper;
using Informa.Library.Services.NlmExport.Models.Body;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;

namespace Informa.Library.Utilities.AutoMapper.Profiles.Nlm
{
    public class ArticleBodyProfile : Profile
    {
        public override string ProfileName => "NlmArticleBodyProfile";

        protected override void Configure()
        {
            // BODY mappings
            CreateMap<ArticleItem, NlmArticleBodyModel>();
        }
    }
}
