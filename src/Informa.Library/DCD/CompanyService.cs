using Informa.Library.Utilities.Parsers;
using Informa.Library.Utilities.TokenMatcher;
using Informa.Library.Wrappers;
using Informa.Model.DCD;
using Informa.Models.DCD;
using Jabberwocky.Autofac.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Library.DCD {
    public interface ICompanyService {
        string GetCompanyName(string CompanyId);
        string GetCompanyDescription(string CompanyId);
        string GetCompanyUrl(string CompanyId);
    }

    [AutowireService]
    public class CompanyService : ICompanyService {

        private readonly IDependencies _;

        [AutowireService(IsAggregateService = true)]
        public interface IDependencies {
            IDCDReader DcdReader { get; set; }
            ICachedXmlParser CachedXmlParser { get; set; }
            IHttpContextProvider HttpContext { get; set; }
        }

        public CompanyService(IDependencies dependencies) {
            _ = dependencies;
        }

        private Informa.Models.DCD.Company GetCompany(string CompanyId) {
            Informa.Models.DCD.Company d = _.DcdReader.GetCompanyByRecordNumber(CompanyId);
            return d;
        }

        private CompanyContent GetCompanyContent(string CompanyId) {
            Informa.Models.DCD.Company c = GetCompany(CompanyId);
            if (c == null)
                return null;

            CompanyContent cc = _.CachedXmlParser.ParseContent<CompanyContent>(c.Content, CompanyId);
            return cc;
        }

        public string GetCompanyName(string CompanyId) {
            Informa.Models.DCD.Company c = GetCompany(CompanyId);
            return c?.Title ?? string.Empty;
        }

        public string GetCompanyDescription(string CompanyId) {
            CompanyContent cc = GetCompanyContent(CompanyId);
            return cc?.CompanyInfo?.Description ?? string.Empty;
        }

        public string GetCompanyUrl(string CompanyId) {
            return $"{_.HttpContext.Current.Request.Url.Scheme}://{_.HttpContext.Current.Request.Url.Host}/companies/{CompanyId}";
        }
    }
}
