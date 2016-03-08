﻿using System;
using AutoMapper;
using Glass.Mapper.Sc;
using Informa.Library.Services.NlmExport.Models.Front.Journal;
using Informa.Library.Utilities.References;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;

namespace Informa.Library.Utilities.AutoMapper.Resolvers.Nlm.Front.Journal
{
    public class JournalTitleResolver : BaseValueResolver<ArticleItem, NlmJournalTitleModel>
    {
        private readonly IItemReferences _itemReferences;
        private readonly ISitecoreService _service;

        public JournalTitleResolver(IItemReferences itemReferences, ISitecoreService service)
        {
            if (itemReferences == null) throw new ArgumentNullException(nameof(itemReferences));
            if (service == null) throw new ArgumentNullException(nameof(service));
            _itemReferences = itemReferences;
            _service = service;
        }

        protected override NlmJournalTitleModel Resolve(ArticleItem source, ResolutionContext context)
        {
            var title = _service.GetItem<INLM_Config>(_itemReferences.NlmConfiguration)?.Journal_Title;

            return new NlmJournalTitleModel
            {
                Title = title
            };
        }
    }
}
