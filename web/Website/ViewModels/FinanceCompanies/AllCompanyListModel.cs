using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Informa.Library.Globalization;
using Informa.Library.Site;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages.Company;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Jabberwocky.Glass.Models;

namespace Informa.Web.ViewModels.FinanceCompanies
{
    public class AllCompanyListModel:GlassViewModel<IGlassBase>  
    {
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