using System;
using System.IO;
using System.Text;
using System.Web;
using System.Xml;
using Glass.Mapper.Sc;
using Informa.Library.Services.NlmExport;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Jabberwocky.Glass.Models;

namespace Informa.Web.sitecore.shell.client.Applications.View_NLM.model
{
    public class ViewNlmViewModel : GlassViewModel<IGlassBase>
    {
        private readonly INlmExportService _exportService;
        private readonly ISitecoreService _service;

        private readonly Lazy<string> _lazyArticleNlm;
        public string ArticleNlm => _lazyArticleNlm.Value;

        protected string ArticleId => HttpContext.Current.Request.QueryString["id"];

        public ViewNlmViewModel(INlmExportService exportService, ISitecoreService service)
        {
            if (exportService == null) throw new ArgumentNullException(nameof(exportService));
            if (service == null) throw new ArgumentNullException(nameof(service));
            _exportService = exportService;
            _service = service;

            _lazyArticleNlm = new Lazy<string>(GetArticleNlm);
        }

        private string GetArticleNlm()
        {
            Guid id;
            if (!Guid.TryParse(ArticleId, out id))
            {
                return string.Empty;
            }

            var article = _service.GetItem<ArticleItem>(id);

            return BeautifyXml(_exportService.GenerateNlm(article));
        }

        private string BeautifyXml(Stream stream)
        {
            XmlDocument doc = new XmlDocument();
            using (var xmlReader = new XmlTextReader(stream) { DtdProcessing = DtdProcessing.Ignore })
            {
                doc.Load(xmlReader);
                using (var memStream = new MemoryStream())
                {
                    var settings = new XmlWriterSettings { CloseOutput = false, Encoding = Encoding.UTF8, Indent = true };
                    using (var writer = XmlWriter.Create(memStream, settings))
                    {
                        doc.Save(writer);
                    }

                    memStream.Seek(0, SeekOrigin.Begin);
                    using (var reader = new StreamReader(memStream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }

    }
}