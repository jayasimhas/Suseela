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
    public class AMGraphsViewModel : GlassViewModel<ICompany_Detail_Page>
    {
        protected readonly ISiteRootContext SiteRootContext;
        protected readonly ITextTranslator TextTranslator;

    public AMGraphsViewModel(ISiteRootContext siteRootContext,
        ITextTranslator textTranslator)
    {
        SiteRootContext = siteRootContext;
        TextTranslator = textTranslator;
    }

        public string CompanyName => GlassModel.Companyname;

    }
}