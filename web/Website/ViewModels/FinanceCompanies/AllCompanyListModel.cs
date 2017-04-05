﻿using System;
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
    public class AllCompanyListModel : GlassViewModel<IGlassBase>
    {
        protected readonly ITextTranslator TextTranslator;

        public AllCompanyListModel(ITextTranslator textTranslator)
        {
            TextTranslator = textTranslator;
        }
        public string ComponentTitle => TextTranslator.Translate("Company.AllCompaniesComponentTitle");
        public string financeCompaniesFolder => GlassModel?._Parent._Url;
        public IEnumerable<ICompany_Detail_Page> FinanceCompanies
        {
            get
            {
                return GlassModel?._Parent._ChildrenWithInferType.OfType<ICompany_Detail_Page>();
            }
        }
    }
}