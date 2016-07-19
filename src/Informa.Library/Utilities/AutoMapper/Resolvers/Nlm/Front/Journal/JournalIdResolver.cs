using System;
using AutoMapper;
using Informa.Library.Services.Global;
using Informa.Library.Services.NlmExport.Models.Front.Journal;
using Informa.Library.Utilities.Extensions;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
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
			var pubRoot = source.Crawl<ISite_Root>();
			if (pubRoot == null)
			{
				throw new ArgumentNullException("pubRoot", $"Journal ID could not be resolved for {source._Path} because the site root could not be located.");
			}

			var id = (pubRoot != null)
				? pubRoot.Journal_ID
				: string.Empty;

			if (string.IsNullOrEmpty(id))
			{
				throw new ArgumentNullException("journalId", $"Journal ID could not be resolved for {source._Path}");
			}

			return new NlmJournalIdModel
			{
				IdType = "publisher-id",
				Value = id
			};
		}
	}
}