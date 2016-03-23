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
            //Resolve needed dependencies
            using (var scope = AutofacConfig.ServiceLocator.BeginLifetimeScope())
            {
                _nlmExportService = scope.Resolve<INlmExportService>();
                _searcher = scope.Resolve<IArticleSearchService>();
            }
        }

        public string ReExport(string articleNumber)
        {
            try
            {
                //Use the NlmExportService to reExport the NLM feed
                ExportResult result = _nlmExportService.ExportNlm(_searcher.GetArticleByNumber(articleNumber), ExportType.Manual, PublicationType.Ecorrected);

                if (result.ExportSuccessful)
                    return string.Empty;
                else
                    return result.Exception.ToString();
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error while ReExporting Article Nlm: " + ex.ToString(), this.GetType());
                return ex.ToString();
            }
        }
    }
}
