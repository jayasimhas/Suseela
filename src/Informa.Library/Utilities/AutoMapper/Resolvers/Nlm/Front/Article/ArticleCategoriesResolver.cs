using System;
using System.Linq;
using AutoMapper;
using Glass.Mapper.Sc;
using Informa.Library.Services.NlmExport.Models.Common;
using Informa.Library.Utilities.References;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Models;

namespace Informa.Library.Utilities.AutoMapper.Resolvers.Nlm.Front.Article
{
    public class ArticleCategoriesResolver : BaseValueResolver<ArticleItem, NlmCategoryModel[]>
    {
        private static readonly string[] EmptySubjects = { string.Empty };

        private readonly IItemReferences _itemReferences;
        private readonly ISitecoreService _service;

        public ArticleCategoriesResolver(ISitecoreService service, IItemReferences itemReferences)
        {
            if (service == null) throw new ArgumentNullException(nameof(service));
            if (itemReferences == null) throw new ArgumentNullException(nameof(itemReferences));
            _service = service;
            _itemReferences = itemReferences;
        }

        protected override NlmCategoryModel[] Resolve(ArticleItem source, ResolutionContext context)
        {
            if (source == null) return new NlmCategoryModel[0];

            return new[]
            {
                // This category is obsolete; always pass empty values
                new NlmCategoryModel
                {
                    GroupType = "Category",
                    Subjects = EmptySubjects
                },
                new NlmCategoryModel
                {
                    GroupType = "Subjects",
                    Subjects = GetTaxonomyItems(source, _itemReferences.SubjectsTaxonomyFolder)
                },
                new NlmCategoryModel
                {
                    GroupType = "Geography",
                    Subjects = GetTaxonomyItems(source, _itemReferences.RegionsTaxonomyFolder)
                },
                new NlmCategoryModel
                {
                    GroupType = "Therapy Areas",
                    Subjects = GetTaxonomyItems(source, _itemReferences.TherapyAreasTaxonomyFolder)
                }
            };
        }

        private string[] GetTaxonomyItems(ArticleItem source, Guid taxonomyFolder)
        {
            var taxonomyFolderPath = _service.GetItem<IGlassBase>(taxonomyFolder)?._Path;
            if (taxonomyFolderPath == null) return new string[0];

            var items = (source?.Taxonomies ?? new ITaxonomy_Item[0])
                .Where(item => item._Path.StartsWith(taxonomyFolderPath, StringComparison.InvariantCultureIgnoreCase))
                .Select(item => getItemNameWithParent(item, taxonomyFolder))
                .ToArray();

            return items.Any()
                ? items
                : EmptySubjects;
        }

        private string getItemNameWithParent(ITaxonomy_Item item, Guid taxonomyFolder)
        {
            if (item._Parent._Id.Equals(taxonomyFolder))
            {
                return item.Item_Name;
            }
            else
            {
                return getItemNameWithParent(item._Parent as ITaxonomy_Item, taxonomyFolder) + @"\" + item.Item_Name;
            }
        }
    }
}
