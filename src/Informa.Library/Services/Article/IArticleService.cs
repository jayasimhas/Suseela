﻿using System.Collections.Generic;
using Informa.Library.Search.ComputedFields.SearchResults.Converter.MediaTypeIcon;
using Informa.Models.FactoryInterface;
using Informa.Models.Informa.Models.sitecore.templates.System.Media.Unversioned;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;

namespace Informa.Library.Services.Article
{
    public interface IArticleService
    {
        IEnumerable<string> GetLegacyPublicationNames(IArticle article);
        IEnumerable<ILinkable> GetLinkableTaxonomies(IArticle article);
        MediaTypeIconData GetMediaTypeIconData(IArticle article);
        string GetArticleSummary(IArticle article);
        string GetArticleBody(IArticle article);
        string GetLegacyPublicationText(IArticle glassModel);
        IEnumerable<IFile> GetSupportingDocuments(IArticle glassModel);
	    string GetArticlePublicationName(IArticle article);
        string GetDownloadUrl(IArticle article);
        string GetPreviewUrl(IArticle article);
    }
}
