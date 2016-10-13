using System;
using System.Linq;
using AutoMapper;
using Glass.Mapper.Sc;
using Informa.Library.Services.NlmExport.Models.Common;
using Informa.Library.Utilities.References;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Models;
using Sitecore.Data.Items;
using System.Collections.Generic;
using Glass;

namespace Informa.Library.Utilities.AutoMapper.Resolvers.Nlm.Front.Article
{
	public class ArticleCategoriesResolver : BaseValueResolver<ArticleItem, NlmCategoryModel[]>
	{
		private static readonly string[] EmptySubjects = { string.Empty };

		private readonly ITaxonomyFolders _taxonomyFolder;
		private readonly ISitecoreService _service;

		public ArticleCategoriesResolver(ISitecoreService service, ITaxonomyFolders taxonomyFolder)
		{
			if (service == null) throw new ArgumentNullException(nameof(service));
			if (taxonomyFolder == null) throw new ArgumentNullException(nameof(taxonomyFolder));
			_service = service;
			_taxonomyFolder = taxonomyFolder;
		}

		protected override NlmCategoryModel[] Resolve(ArticleItem source, ResolutionContext context)
		{
			if (source == null) return new NlmCategoryModel[0];

			List<NlmCategoryModel> categories = new List<NlmCategoryModel>();
			// This category is obsolete; always pass empty values
			categories.Add(new NlmCategoryModel { GroupType = "Category", Subjects = EmptySubjects });

			//Fill categories with all global taxonomy/category type
			foreach (var taxoFolder in _taxonomyFolder.TaxonomyFolders)
			{
				categories.Add(new NlmCategoryModel { GroupType = taxoFolder.Name, Subjects = GetTaxonomyItems(source, taxoFolder.ID.Guid) });
			}

			return categories.ToArray();
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
