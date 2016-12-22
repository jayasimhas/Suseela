using Glass.Mapper.Sc;
using Informa.Library.Globalization;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages.Company;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Informa.Web.ViewModels.FinanceCompanies
{
    public class PeerGroupDetailViewModel : GlassViewModel<ICompany_Peer_Group_Detail_Page>
    {
        protected readonly ITextTranslator TextTranslator;
        public ISitecoreContext SitecoreContext;

        public PeerGroupDetailViewModel(ITextTranslator textTranslator, ISitecoreContext sitecoreContext)
        {
            TextTranslator = textTranslator;
            SitecoreContext = sitecoreContext;
        }

        /// <summary>
        /// Page Title
        /// </summary>
        public string PeerGroupPageTitle => GetPageTitle();
        /// <summary>
        /// Dictionary Item for Page Title
        /// </summary>
        public string PeerGroupPageTitleTemplate => TextTranslator.Translate("PeerGroup.DetailPage.Title");
        /// <summary>
        /// Method for fetching Peer Page Title
        /// </summary>
        /// <returns></returns>
        private string GetPageTitle()
        {
            string id = HttpContext.Current.Request.QueryString["Id"];
            if (!string.IsNullOrEmpty(id))
            {
                var compnayPage = SitecoreContext.GetItem<ICompany_Detail_Page>(id);
                return compnayPage.Companyname;
            }
            else
            {
                return string.Empty;
            }
            
        }

    }
}