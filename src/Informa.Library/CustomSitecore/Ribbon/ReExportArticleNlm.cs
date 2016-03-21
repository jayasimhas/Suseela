using Informa.Library.Article.Service;
using Informa.Library.Services.NlmExport;
using Jabberwocky.Glass.Autofac.Util;
using System;
using Autofac;

namespace Informa.Library.CustomSitecore.Ribbon
{
    public class ReExportArticleNlm
    {
        INlmExportService _nlmExportService;
        IArticleSearchService _searcher;

        public ReExportArticleNlm()
        {
            using (var scope = AutofacConfig.ServiceLocator.BeginLifetimeScope())
            {
                _nlmExportService = scope.Resolve<INlmExportService>();
                _searcher = scope.Resolve<IArticleSearchService>();
            }
        }

        public bool ReExport(string articleNumber)
        {
            try
            {
                ExportResult result = _nlmExportService.ExportNlm(_searcher.GetArticleByNumber(articleNumber), ExportType.Manual, PublicationType.Ecorrected);

                return result.ExportSuccessful;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error while ReExporting Article Nlm: " + ex.ToString(), this.GetType());
                return false;
            }
        }
    }
}
