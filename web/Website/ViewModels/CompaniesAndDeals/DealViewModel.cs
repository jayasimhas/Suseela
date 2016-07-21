using System.Linq;
using System.Web;
using Glass.Mapper.Sc;
using Informa.Library.Globalization;
using Informa.Library.Utilities.Parsers;
using Informa.Library.Utilities.References;
using Informa.Library.Utilities.WebUtils;
using Informa.Library.Wrappers;
using Informa.Model.DCD;
using Informa.Models.DCD;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Components;
using Jabberwocky.Autofac.Attributes;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Jabberwocky.Glass.Models;

namespace Informa.Web.ViewModels.CompaniesAndDeals
{
    public class DealViewModel : GlassViewModel<IGlassBase>
    {
        private readonly IDependencies _dependencies;

        [AutowireService(IsAggregateService = true)]
        public interface IDependencies
        {
            IDCDReader DcdReader { get; }
            IHttpContextProvider HttpContextProvider { get; }
            ICachedXmlParser CachedXmlParser { get; set; }
            ITextTranslator TextTranslator { get; }
            ISitecoreService SitecoreService { get; set; }
        }

        public DealViewModel(IDependencies dependencies)
        {
            _dependencies = dependencies;

            RecordNumber = UrlUtils.GetLastUrlSement(_dependencies.HttpContextProvider.Current);

            Deal = _dependencies.DcdReader.GetDealByRecordNumber(RecordNumber);
            if (Deal == null) RedirectTo404();

            Content = _dependencies.CachedXmlParser.ParseContent<DealContent>(Deal.Content, RecordNumber);
            if (Content == null) RedirectTo404();

        }

        private void RedirectTo404()
        {
            _dependencies.HttpContextProvider.Current.Response.Redirect($"/404?url=/deals/{RecordNumber}");
        }

        public string RecordNumber { get; set; }
        public Deal Deal { get; set; }
        public DealContent Content { get; set; }

        public string DealsSummaryText =>_dependencies.TextTranslator.Translate("DCD.Summary");
        public string BroughtToYouByText => _dependencies.TextTranslator.Translate("DCD.BroughToYouBy");
        public string DealIndustryHeader => _dependencies.TextTranslator.Translate("DCD.DealIndustry");
        public string DealStatusHeader => _dependencies.TextTranslator.Translate("DCD.DealStatus");
        public string DealTypeHeader => _dependencies.TextTranslator.Translate("DCD.DealType");
        public string RelatedCompaniesHeader => _dependencies.TextTranslator.Translate("DCD.RelatedCompanies");

        public string LogoUrl
        {
            get
            {
                var strategicComponent =
                    _dependencies.SitecoreService.GetItem<IStrategic_Transactions>(
                        Constants.StrategicTransactionsComponent);
                return strategicComponent?.Logo != null ? strategicComponent.Logo.Src : string.Empty;
            }
        }

        public Coding[] Industries => Content.CodingSets?.FirstOrDefault(x => x.Type.Equals("indstry"))?.Codings;
        public InnerCompany[] RelatedCompanies => Content.DealCompanies.Select(x => x.Company).ToArray();

    }
}