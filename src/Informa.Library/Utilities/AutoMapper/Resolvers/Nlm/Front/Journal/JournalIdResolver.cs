using System;
using AutoMapper;
using Informa.Library.Services.Global;
using Informa.Library.Services.NlmExport.Models.Front.Journal;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;

namespace Informa.Library.Utilities.AutoMapper.Resolvers.Nlm.Front.Journal
{
    public class JournalIdResolver : BaseValueResolver<ArticleItem, NlmJournalIdModel>
    {
        private readonly IGlobalSitecoreService _globalService;

        public JournalIdResolver(IGlobalSitecoreService globalService)
        {
            if (globalService == null) throw new ArgumentNullException(nameof(globalService));
            _globalService = globalService;
        }

        protected override NlmJournalIdModel Resolve(ArticleItem source, ResolutionContext context)
        {
            var pubRoot = _globalService.GetSiteRootAncestor(source._Id);
            var id = (pubRoot != null) 
                ? pubRoot.Journal_ID
                : string.Empty;

            return new NlmJournalIdModel
            {
                IdType = "publisher-id",
                Value = id
            };
        }
    }
}
