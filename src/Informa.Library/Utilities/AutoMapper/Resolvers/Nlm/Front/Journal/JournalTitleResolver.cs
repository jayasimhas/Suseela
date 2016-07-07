using System;
using AutoMapper;
using Informa.Library.Services.Global;
using Informa.Library.Services.NlmExport.Models.Front.Journal;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;

namespace Informa.Library.Utilities.AutoMapper.Resolvers.Nlm.Front.Journal
{
    public class JournalTitleResolver : BaseValueResolver<ArticleItem, NlmJournalTitleModel>
    {
        private readonly IGlobalSitecoreService _globalService;

        public JournalTitleResolver(IGlobalSitecoreService globalService)
        {
            if (globalService == null) throw new ArgumentNullException(nameof(globalService));
            _globalService = globalService;
        }

        protected override NlmJournalTitleModel Resolve(ArticleItem source, ResolutionContext context)
        {
            var pubRoot = _globalService.GetSiteRootAncestor(source._Id);
            var title = (pubRoot != null)
                ? pubRoot.Journal_Title
                : string.Empty;
            
            return new NlmJournalTitleModel
            {
                Title = title
            };
        }
    }
}
