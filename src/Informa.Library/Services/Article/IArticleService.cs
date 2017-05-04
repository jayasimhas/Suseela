﻿using System;
using System.Collections.Generic;
using Informa.Library.Search.ComputedFields.SearchResults.Converter.MediaTypeIcon;
using Informa.Models.FactoryInterface;
using Informa.Models.Informa.Models.sitecore.templates.System.Media.Unversioned;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Components;

namespace Informa.Library.Services.Article
{
    public interface IArticleService
    {
        IEnumerable<string> GetLegacyPublicationNames(IArticle article,bool isLegacyBrandSelected = false);
        IEnumerable<ILinkable> GetLinkableTaxonomies(IArticle article);
        MediaTypeIconData GetMediaTypeIconData(IArticle article);
        string GetArticleSummary(IArticle article);
        string GetArticleBody(IArticle article);
        string GetLegacyPublicationText(IArticle glassModel, bool isLegacyBrandSelected = false,string Escenic_ID = null,string Legacy_Article_Number = null);
        IEnumerable<IFile> GetSupportingDocuments(IArticle glassModel);
	    string GetArticlePublicationName(IArticle article);
        string GetDownloadUrl(IArticle article);
        string GetPreviewUrl(IArticle article);
        string GetCmsUrl(IArticle article);
        IEnumerable<Informa.Library.Article.Search.ILinkable> GetPersonalizedLinkableTaxonomies(IArticle article);
        ISponsored_Content GetSponsoredContent(IArticle article);
        string GetArticleBody(string _body, Guid _ArticleId);
    }
}
