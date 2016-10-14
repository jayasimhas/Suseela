using Glass.Mapper.Sc;
using Informa.Library.DCD;
using Informa.Library.User;
using Informa.Library.User.Authentication;
using Informa.Library.User.Entitlement;
using Informa.Library.Utilities.References;
using Informa.Library.Utilities.WebUtils;
using Informa.Library.Wrappers;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Components;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Autofac.Attributes;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Jabberwocky.Glass.Models;
using System.Collections.Generic;
using System.Linq;

namespace Informa.Web.ViewModels.CompaniesAndDeals
{
    public class StrategicTransactionViewModel : GlassViewModel<IGlassBase>
    {
        [AutowireService(true)]
        public interface IDependencies
        {
            ISitecoreService SitecoreService { get; set; }
            IStrategicEntitlementContext StrategicEntitlementContext { get; set; }
            IDealService DealService { get; set; }
            IHttpContextProvider HttpContext { get; set; }
        }

        public StrategicTransactionViewModel(IDependencies dependencies)
        {
            var strategicTransactionComponent =
                dependencies.SitecoreService.GetItem<IStrategic_Transactions>(Constants.StrategicTransactionsComponent);

            if (strategicTransactionComponent == null) return;
            if (strategicTransactionComponent.Logo != null)
                Logo = strategicTransactionComponent.Logo.Src;
            
            bool IsEntitled = dependencies.StrategicEntitlementContext.AuthenticatedUserHasStrategicEntitlements();
            if (IsEntitled) {
                Body = strategicTransactionComponent.STAT_Entitled_Body;
                SubscribeButtonText = strategicTransactionComponent.STAT_Entitled_Subscribe_Button_Text;
                string DealId = UrlUtils.GetLastUrlSement(dependencies.HttpContext.Current);
                SubscribeButtonURL = dependencies.DealService.GetPMBIDealUrl(DealId);
            } else {
                Body = strategicTransactionComponent.Body;
                SubscribeButtonText = strategicTransactionComponent.Subscribe_Button_Text;
                SubscribeButtonURL = strategicTransactionComponent.Subscribe_Button_URL != null
                    ? strategicTransactionComponent.Subscribe_Button_URL.Url
                    : string.Empty;
            }
        }

        public string Logo { get; set; }
        public string Body { get; set; }
        public string SubscribeButtonText { get; set; }
        public string SubscribeButtonURL { get; set; }
    }
}
