
using Informa.Library.Utilities.Parsers;
using Informa.Library.Utilities.Settings;
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
    public interface IDealService {
        string GetDealTitle(string DealId);

        string GetDealSummary(string DealId);

        string GetDealUrl(string DealId);

        string GetPMBIDealUrl(string DealId);
    }
    
    [AutowireService]
    public class DealService : IDealService{

        private readonly IDependencies _;

        [AutowireService(IsAggregateService = true)]
        public interface IDependencies {
            IDCDReader DcdReader { get; set; }
            ICachedXmlParser CachedXmlParser { get; set; }
            IHttpContextProvider HttpContext { get; set; }
            ISiteSettings SiteSettings { get; set; }
        }

        public DealService(IDependencies dependencies) {
            _ = dependencies;
        }

        private Deal GetDeal(string DealId) {
            Deal d = _.DcdReader.GetDealByRecordNumber(DealId);
            return d;
        }

        private DealContent GetDealContent(string DealId) {
            Deal d = GetDeal(DealId);
            if (d == null)
                return null;

            DealContent dc = _.CachedXmlParser.ParseContent<DealContent>(d.Content, DealId);
            return dc;
        }

        public string GetDealTitle(string DealId) {
            Deal d = GetDeal(DealId);
            return d?.Title ?? string.Empty;
        }

        public string GetDealSummary(string DealId) {
            DealContent dc = GetDealContent(DealId);
            if (dc == null)
                return string.Empty;

            return (!string.IsNullOrEmpty(dc.DealSummary)) 
                ? DCDTokenMatchers.ReplaceDealNameTokens(dc.DealSummary) 
                : string.Empty;
        }

        public string GetDealUrl(string DealId) {
            return $"{_.HttpContext.Current.Request.Url.Scheme}://{_.HttpContext.Current.Request.Url.Host}/deals/{DealId}";
        }

        public string GetPMBIDealUrl(string DealId) {
            string s = string.Format(_.SiteSettings.OldDealsUrl, DealId);
            return $"{s}?r=0";
        }
    }
}
