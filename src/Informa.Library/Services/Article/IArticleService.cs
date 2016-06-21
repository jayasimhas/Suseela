using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Informa.Models.FactoryInterface;
using Informa.Models.Informa.Models.sitecore.templates.System.Media.Unversioned;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;

namespace Informa.Library.Services.Article {
    public interface IArticleService
    {
        IEnumerable<string> GetLegacyPublicationNames(IArticle article);
        IEnumerable<ILinkable> GetLinkableTaxonomies(IArticle article);
        string GetMediaTypeName(IArticle article);
        string GetArticleSummary(IArticle article);
        string GetArticleBody(IArticle article);
        string GetLegacyPublicationText(IArticle glassModel);
        IEnumerable<IFile> GetSupportingDocuments(IArticle glassModel);
	    string GetArticlePublicationName(IArticle article);
    }
}
