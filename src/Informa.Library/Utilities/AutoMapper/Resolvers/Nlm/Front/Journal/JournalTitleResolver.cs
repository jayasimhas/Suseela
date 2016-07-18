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
			if (pubRoot == null)
			{
				throw new ArgumentNullException("pubRoot", $"Journal Title could not be resolved for {source._Path} because the site root could not be located.");
			}

			var title = (pubRoot != null)
				? pubRoot.Journal_Title
				: string.Empty;

			if (string.IsNullOrEmpty(title))
			{
				throw new ArgumentNullException("journalTitle", $"Journal Title could not be resolved for {source._Path}");
			}

			return new NlmJournalTitleModel
			{
				Title = title
			};
		}
	}
}