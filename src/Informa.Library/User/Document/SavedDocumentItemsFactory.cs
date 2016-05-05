using Glass.Mapper.Sc;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Autofac.Attributes;
using System;
using System.Collections.Generic;

namespace Informa.Library.User.Document
{
	[AutowireService]
	public class SavedDocumentItemsFactory : ISavedDocumentItemsFactory
	{
		protected readonly ISitecoreService SitecoreService;

		public SavedDocumentItemsFactory(
			ISitecoreService sitecoreService)
		{
			SitecoreService = sitecoreService;
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

				var item = SitecoreService.GetItem<IArticle>(itemId);

				if (item == null)
				{
					continue;
				}

				savedDocumentItems.Add(new SavedDocumentItem
				{
					DocumentId = savedDocument.DocumentId,
					Publication = savedDocument.Description,
					Published = item?.Actual_Publish_Date ?? default(DateTime),
					Title = item?.Title ?? savedDocument.Name,
					Url = item?._Url ?? "#"
				});
			}

			return savedDocumentItems;
		}
	}
}
