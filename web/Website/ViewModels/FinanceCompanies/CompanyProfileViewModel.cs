using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Informa.Library.Globalization;
using Informa.Library.Site;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages.Company;
using Jabberwocky.Glass.Autofac.Mvc.Models;


namespace Informa.Web.ViewModels.FinanceCompanies
{
    public class CompanyProfileViewModel: GlassViewModel<ICompany_Detail_Page> 
    {
        protected readonly ISiteRootContext SiteRootContext;
        protected readonly ITextTranslator TextTranslator;

        public CompanyProfileViewModel( ISiteRootContext siteRootContext, ITextTranslator textTranslator)
        {
            SiteRootContext = siteRootContext;
            TextTranslator = textTranslator;
        }

        public string ProfileTitle => GlassModel.Profile_Title;

        public string ProfileText => GlassModel.Profile_Text;


        public IEnumerable<ICompany_Detail_Page> FinanceCompanies
        {
            get
            {
                var financeCompaniesFolder = GlassModel._Parent;
                return financeCompaniesFolder._ChildrenWithInferType.OfType<ICompany_Detail_Page>();
            }
        }

    }
}