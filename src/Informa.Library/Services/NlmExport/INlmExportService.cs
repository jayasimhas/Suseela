using System.IO;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;

namespace Informa.Library.Services.NlmExport
{
    public interface INlmExportService
    {
        Stream ExportNlm(ArticleItem article);
    }
}
