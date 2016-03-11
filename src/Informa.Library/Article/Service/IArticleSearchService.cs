using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;

namespace Informa.Library.Article.Service
{
    public interface IArticleSearchService
    {
        ArticleItem GetArticleByNumber(string number);
    }
}
