using System.Web;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;

namespace Informa.Library.Site
{
	public interface ISiteRootContext
	{
		ISite_Root Item { get; }
        string GetBodyCssClass();
        HtmlString GetPrintHeaderMessage();
    }
}
