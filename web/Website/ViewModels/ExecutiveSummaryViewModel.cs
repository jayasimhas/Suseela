using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Informa.Library.Globalization;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Informa.Library.Services.AccountManagement;
using Informa.Library.Utilities.References;

namespace Informa.Web.ViewModels {
    public class ExecutiveSummaryViewModel : GlassViewModel<IGeneral_Content_Page> {

        private readonly ITextTranslator TextTranslator;
        private readonly IAccountManagementService AccountService;
        private readonly IItemReferences ItemReferences;

        public ExecutiveSummaryViewModel(
            ITextTranslator textTranslator,
            IAccountManagementService accountService,
            IItemReferences itemReferences)
        {
            TextTranslator = textTranslator;
            AccountService = accountService;
            ItemReferences = itemReferences;
        }

        public string ExecutiveSummary => TextTranslator.Translate("SharedContent.ExecutiveSummary");

        public bool ShowSummary => AccountService.IsRestricted(GlassModel) || 
            (GlassModel.Restrict_Access.Equals(ItemReferences.FreeWithEntitlement) && GlassModel.Show_Summary_When_Entitled && AccountService.IsEntitled(GlassModel));
    }
}