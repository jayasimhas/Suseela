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
    public class CompanyGraphViewModel : GlassViewModel<ICompany_Graph_Detail_Page>
    {
        protected readonly ITextTranslator TextTranslator;
        public ISitecoreContext SitecoreContext;

        public CompanyGraphViewModel(ITextTranslator textTranslator, ISitecoreContext sitecoreContext)
        {
            TextTranslator = textTranslator;
            SitecoreContext = sitecoreContext;
        }
        /// <summary>
        /// Page Title
        /// </summary>
        public string PageTitle => GetPageTitle();
        /// <summary>
        /// Method for fetching Page Title
        /// </summary>
        /// <returns>Page Title</returns>
        private string GetPageTitle()
        {
            string id = HttpContext.Current.Request.QueryString["Id"];
            if (!string.IsNullOrEmpty(id))
            {
                var compnayPage = SitecoreContext.GetItem<ICompany_Detail_Page>(id);
                if (compnayPage != null)
                    return compnayPage.Companyname;
                else
                    return string.Empty;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}