using Informa.Library.DCD;
using Informa.Library.Globalization;
using Informa.Library.Utilities.Extensions;
using Informa.Library.Utilities.WebUtils;
using Informa.Library.Wrappers;
using Informa.Model.DCD;
using Informa.Models.DCD;
using Jabberwocky.Autofac.Attributes;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Jabberwocky.Glass.Models;

namespace Informa.Web.ViewModels.CompaniesAndDeals
{
    public class CompanyViewModel : GlassViewModel<IGlassBase>
    {
        private readonly IDependencies _dependencies;

        [AutowireService(IsAggregateService = true)]
        public interface IDependencies
        {
            IDCDReader DcdReader { get; }
            IHttpContextProvider HttpContextProvider { get; }
            ICompanyContentXmlParser CompanyContentXmlParser { get; }
            ITextTranslator TextTranslator { get; }
        }

        public CompanyViewModel(IDependencies dependencies)
        {
            _dependencies = dependencies;
            RecordNumber = UrlUtils.GetLastUrlSement(_dependencies.HttpContextProvider.Current);
            Company = _dependencies.DcdReader.GetCompanyByRecordNumber(RecordNumber);
            Content = _dependencies.CompanyContentXmlParser.ParseContent(Company.Content, RecordNumber);

            if (Company == null || Content == null)
            {
                _dependencies.HttpContextProvider.Current.Response.Redirect($"/404?url=/companies/{RecordNumber}");
            }
        }

        public string RecordNumber { get; set; }
        public Company Company { get; set; }
        public CompanyContent Content { get; set; }

        private string _divisionOfText;
        public string DivisionOfText
        {
            get
            {
                if (!_divisionOfText.HasContent())
                {
                    var parent = _dependencies.CompanyContentXmlParser.GetParentCompany(Content);
                    var text = _dependencies.TextTranslator.Translate("DCD.DivisionOf");
                    _divisionOfText = parent.HasContent() ? $"{text} {parent}" : null;
                }
                return _divisionOfText;
            }
        }
    }
}