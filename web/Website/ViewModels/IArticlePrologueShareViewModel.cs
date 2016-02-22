using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;

namespace Informa.Web.ViewModels
{
	public interface IArticlePrologueShareViewModel
	{
        string ArticleTitle { get; }
        string ArticleUrl { get; }
        string ShareText { get; }
    }
}
