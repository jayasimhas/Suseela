using System;
using AutoMapper;
using Glass.Mapper.Sc;
using Informa.Library.Services.NlmExport.Models.Front.Article;
using Informa.Library.Utilities.References;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Velir.Core.Extensions.System;

namespace Informa.Library.Utilities.AutoMapper.Resolvers.Nlm.Front.Article
{
    public class ArticlePermissionsResolver : BaseValueResolver<ArticleItem, NlmArticlePermissionsModel>
    {
        private const string YearToken = "##YEAR##";

        private readonly ISitecoreService _service;
        private readonly IItemReferences _itemReferences;

        public ArticlePermissionsResolver(ISitecoreService service, IItemReferences itemReferences)
        {
            if (service == null) throw new ArgumentNullException(nameof(service));
            if (itemReferences == null) throw new ArgumentNullException(nameof(itemReferences));
            _service = service;
            _itemReferences = itemReferences;
        }

        protected override NlmArticlePermissionsModel Resolve(ArticleItem source, ResolutionContext context)
        {
            var year = DateTime.UtcNow.Year.ToString();

            var copyright = _service.GetItem<INLM_Copyright_Statement>(_itemReferences.NlmCopyrightStatement)?.Copyright_Statement ?? string.Empty;
            copyright = copyright.Replace(YearToken, year, StringComparison.InvariantCultureIgnoreCase);

            return new NlmArticlePermissionsModel
            {
                CopyrightStatement = copyright,
                CopyrightYear = year // maybe override this later? I think we can rely on just setting this during mapping
            };
        }
    }
}
