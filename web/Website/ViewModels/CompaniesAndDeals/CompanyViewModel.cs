using System;
using System.Linq;
using Informa.Library.DCD;
using Informa.Library.Globalization;
using Informa.Library.Utilities.DataModels;
using Informa.Library.Utilities.Extensions;
using Informa.Library.Utilities.Parsers;
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
            IDcdContentAnalyzer DcdContentAnalyzer { get; }
            ICachedXmlParser CachedXmlParser { get; set; }
            ITextTranslator TextTranslator { get; }
        }

        public CompanyViewModel(IDependencies dependencies)
        {
            _dependencies = dependencies;

            RecordNumber = UrlUtils.GetLastUrlSement(_dependencies.HttpContextProvider.Current);
            RedirectIfRecordId(RecordNumber);

            Company = _dependencies.DcdReader.GetCompanyByRecordNumber(RecordNumber);
            if (Company == null) RedirectTo404();

            Content = _dependencies.CachedXmlParser.ParseContent<CompanyContent>(Company.Content, RecordNumber);
            if (Content == null) RedirectTo404();
        }

        private void RedirectTo404()
        {
            _dependencies.HttpContextProvider.Current.Response.Redirect($"/404?url=/companies/{RecordNumber}");
        }

        private void RedirectIfRecordId(string segment)
        {
            if(!segment.StartsWith("_id")) { return; }  //Not a record id, yay!

            int id;
            if (!int.TryParse(segment.Substring(3), out id)) return;

            var company = _dependencies.DcdReader.GetCompanyByRecordId(id);
            if ((company?.RecordNumber).HasContent())
            {
                _dependencies.HttpContextProvider.Current.Response.Redirect($"/companies/{company.RecordNumber}");
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
                    var parent = _dependencies.DcdContentAnalyzer.GetParentCompany(Content);
                    var text = _dependencies.TextTranslator.Translate("DCD.DivisionOf");
                    _divisionOfText = parent.HasContent() ? $"{text} {parent}" : null;
                }
                return _divisionOfText;
            }
        }
        public string WebsiteHref => ("http://" + Content.ContactInfo.Website).Replace("http://http", "http");
        public string RegionAddressLine
        {
            get
            {
                var text = Content.ContactInfo.PoBox.HasContent() ? $"P.O. Box {Content.ContactInfo.PoBox} " : string.Empty;
                text += $"{Content.ContactInfo.City}, {Content.ContactInfo.State} {Content.ContactInfo.Zip}";
                return text;
            }
        }

        public TreeNode<string,string>[] Industries
            => _dependencies.DcdContentAnalyzer.GetCodingSetTrees(
                    Content.CodingSets?.FirstOrDefault(x => x.Type.Equals("indstry"))?.Codings, "&gt;")
            ?? new TreeNode<string, string>[0];
        public Coding[] TherapyAreas => Content.CodingSets?.FirstOrDefault(x => x.Type.Equals("theracat"))?.Codings;
        public string[] LocationPath => Content.CompanyInfo.LocationPath.Split('/');
        public TreeNode<string,string>[] SubsidiariesTree
            => _dependencies.DcdContentAnalyzer.GetCompanySubsidiaryTree(Content.ParentsAndDivisions)
            ?? new TreeNode<string, string>[0];

        public string CompanyInformationHeader => _dependencies.TextTranslator.Translate("DCD.CompanyInformation");
        public string IndustryHeader => _dependencies.TextTranslator.Translate("DCD.Industry");
        public string TherapyAreasHeader => _dependencies.TextTranslator.Translate("DCD.TherapeuticAreas");
        public string OwnershipHeader => _dependencies.TextTranslator.Translate("DCD.Ownership");
        public string HeadquartersHeader => _dependencies.TextTranslator.Translate("DCD.Headquarters");
        public string CompanyTypeHeader => _dependencies.TextTranslator.Translate("DCD.CompanyType");
        public string AliasHeader => _dependencies.TextTranslator.Translate("DCD.Alias");
        public string ParentSubHeader => _dependencies.TextTranslator.Translate("DCD.ParentAndSubsidiaries");
        public string SeniorManagementHeader => _dependencies.TextTranslator.Translate("DCD.SeniorManagement");
        public string ContactHeader => _dependencies.TextTranslator.Translate("DCD.ContactInfo");

        public bool ShowCompanyType => Content?.CompanyInfo?.Description.HasContent() ?? false;
    }
}