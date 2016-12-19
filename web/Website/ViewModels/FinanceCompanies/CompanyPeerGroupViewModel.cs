using Elsevier.Library.Interfaces;
using Glass.Mapper.Sc;
using Informa.Library.Globalization;
using Informa.Models.FactoryInterface;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages.Company;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;

namespace Informa.Web.ViewModels.FinanceCompanies
{
    //IPMP-1274
    public class CompanyPeerGroupViewModel : GlassViewModel<ICompany_Detail_Page>
    {

        protected readonly ITextTranslator TextTranslator;
        public ISitecoreContext SitecoreContext;

        public CompanyPeerGroupViewModel(ITextTranslator textTranslator, ISitecoreContext sitecoreContext)
        {
            TextTranslator = textTranslator;
            SitecoreContext = sitecoreContext;
        }
        /// <summary>
        /// Peer Group Title
        /// </summary>
        public string PeerGroupTitle => GlassModel?.PeerGroup_Title;
        /// <summary>
        /// Peer Group section description text
        /// </summary>
        public string PeerGroupDescription => GlassModel?.PeerGroup_Text;
        /// <summary>
        /// Dictionary Item for Go Button text
        /// </summary>
        public string PeerGroupGoBtnText => TextTranslator.Translate("Company.PeerGroupButtonText");
        /// <summary>
        /// Dictionary Item for Peer group help text
        /// </summary>
        public string PeerGroupHelpText => TextTranslator.Translate("Company.CompaniesPeerGroupIncluded");
        /// <summary>
        /// Peer group go button link
        /// </summary>
        public string PeerGroupGoBtnLink => GlassModel?.PeerComanyDetailPage?.Url + "?Id=" + GlassModel?._Id;
        /// <summary>
        /// List of Peer Companlies
        /// </summary>
        public IEnumerable<LinkableModel> PeerCompanies => GetPeerCompanies();

        /// <summary>
        /// Method to get peer companies
        /// </summary>
        /// <returns></returns>
        private IEnumerable<LinkableModel> GetPeerCompanies()
        {
            List<LinkableModel> peerCompanies = new List<LinkableModel>();
            var peerComanies = GlassModel?.Company_PeerGroupList;
            if (peerComanies != null && peerComanies.Any())
            {
                foreach (var peer in peerComanies)
                {
                    LinkableModel link = new LinkableModel();
                    if(!string.Equals(peer._Name, SitecoreContext.GetCurrentItem<ICompany_Detail_Page>()._Name,StringComparison.OrdinalIgnoreCase))
                    {
                        link.LinkableText = peer._Name;
                        link.LinkableUrl = peer._Url;
                        peerCompanies.Add(link);
                    }
                    
                }
            }
            return peerCompanies;
        }
    }
}