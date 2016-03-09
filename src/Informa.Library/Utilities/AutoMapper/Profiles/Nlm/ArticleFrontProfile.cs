using AutoMapper;
using Informa.Library.Services.NlmExport.Models.Front;
using Informa.Library.Utilities.AutoMapper.Resolvers;
using Informa.Library.Utilities.AutoMapper.Resolvers.Nlm.Front.Article;
using Informa.Library.Utilities.AutoMapper.Resolvers.Nlm.Front.Journal;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;

namespace Informa.Library.Utilities.AutoMapper.Profiles.Nlm
{
    public class ArticleFrontProfile : Profile
    {
        public override string ProfileName => "NlmArticleFrontProfile";

        protected override void Configure()
        {
            // FRONT mappings
            CreateMap<ArticleItem, NlmArticleFrontModel>()
                .ForMember(m => m.ArticleMeta, opt => opt.ResolveUsing<AutoMapResolver<ArticleItem, NlmArticleMetaModel>>())
                .ForMember(m => m.JournalMeta, opt => opt.ResolveUsing<AutoMapResolver<ArticleItem, NlmJournalMetaModel>>());

            // Article Meta
            CreateMap<ArticleItem, NlmArticleMetaModel>()
                .ForMember(m => m.PublisherId, opt => opt.ResolveUsing<ArticlePublisherIdResolver>())
                .ForMember(m => m.Categories, opt => opt.ResolveUsing<ArticleCategoriesResolver>())
                .ForMember(m => m.TitleGroup, opt => opt.ResolveUsing<ArticleTitleResolver>())
                .ForMember(m => m.Contributors, opt => opt.ResolveUsing<ArticleContributorResolver>())
                .ForMember(m => m.PubDate, opt => opt.ResolveUsing<ArticlePubDateResolver>())
                .ForMember(m => m.History, opt => opt.ResolveUsing<ArticleHistoryResolver>())
                .ForMember(m => m.Permissions, opt => opt.ResolveUsing<ArticlePermissionsResolver>())
                .ForMember(m => m.Volume, opt => opt.Ignore())
                .ForMember(m => m.Issue, opt => opt.Ignore())
                ;

            // Journal Meta
            CreateMap<ArticleItem, NlmJournalMetaModel>()
                .ForMember(m => m.Id, opt => opt.ResolveUsing<JournalIdResolver>())
                .ForMember(m => m.TitleGroup, opt => opt.ResolveUsing<JournalTitleResolver>())
                .ForMember(m => m.IssueNumber, opt => opt.ResolveUsing<IssueNumberResolver>())
                .ForMember(m => m.Publisher, opt => opt.ResolveUsing<PublisherResolver>());
        }
    }
}
