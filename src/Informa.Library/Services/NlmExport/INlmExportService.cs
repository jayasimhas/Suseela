using System.IO;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;

namespace Informa.Library.Services.NlmExport
{
    public interface INlmExportService
    {
        Stream GenerateNlm(ArticleItem article, PublicationType? type = null);

        ExportResult ExportNlm(ArticleItem article, ExportType exportType, PublicationType? type = null);

        bool DeleteNlm(ArticleItem article);
    }

    public enum PublicationType
    {
        Epub,
        Ecorrected,
        Eretracted
    }

    public enum ExportType
    {
        Manual,
        Scheduled
    }
}
