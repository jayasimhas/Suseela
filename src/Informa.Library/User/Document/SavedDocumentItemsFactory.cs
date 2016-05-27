using Glass.Mapper.Sc;
using Informa.Library.Site;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Autofac.Attributes;
using System;
using System.Collections.Generic;
using Informa.Library.Services.Global;

namespace Informa.Library.User.Document
{
	[AutowireService]
	public class SavedDocumentItemsFactory : ISavedDocumentItemsFactory
	{
		protected readonly IGlobalService GlobalService;
		protected readonly IIsUrlCurrentSite IsUrlCurrentSite;

		public SavedDocumentItemsFactory(
			IGlobalService globalService,
			IIsUrlCurrentSite isUrlCurrentSite)
		{
            GlobalService = globalService;
			IsUrlCurrentSite = isUrlCurrentSite;
		}

		public IEnumerable<ISavedDocumentItem> Create(IEnumerable<ISavedDocument> savedDocuments)
		{
			var savedDocumentItems = new List<ISavedDocumentItem>();

			foreach (var savedDocument in savedDocuments)
			{
				Guid itemId;

				if (!Guid.TryParse(savedDocument.DocumentId, out itemId))
				{
					continue;
				}

				var item = GlobalService.GetItem<IArticle>(itemId);

				if (item == null)
				{
					continue;
				}

				var url = item._Url ?? string.Empty;
				var isExternalUrl = !IsUrlCurrentSite.Check(url);

				savedDocumentItems.Add(new SavedDocumentItem
				{
					DocumentId = savedDocument.DocumentId,
					Publication = savedDocument.Description,
					PublishedOn = item.Actual_Publish_Date,
					SavedOn = savedDocument.SaveDate,
					Title = item.Title ?? savedDocument.Name,
					Url = url,
					IsExternalUrl = isExternalUrl
				});
			}

			return savedDocumentItems;
		}
	}
}
