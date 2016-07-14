using Glass.Mapper.Sc;
using Informa.Library.Utilities.References;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Components;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Autofac.Attributes;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Jabberwocky.Glass.Models;

namespace Informa.Web.ViewModels.CompaniesAndDeals
{
    public class StratrgicTransactionViewModel : GlassViewModel<IGlassBase>
    {
        [AutowireService(true)]
        public interface IDependencies
        {
            ISitecoreService SitecoreService { get; set; }
        }

        public StratrgicTransactionViewModel(IDependencies dependencies)
        {
            var strategicTransactionComponent =
                dependencies.SitecoreService.GetItem<IStrategic_Transactions>(Constants.StrategicTransactionsComponent);
            if (strategicTransactionComponent == null) return;
            if (strategicTransactionComponent.Logo != null)
            {
                Logo = strategicTransactionComponent.Logo.Src;
            }
            Body = strategicTransactionComponent.Body;
            SubscribeButtonText = strategicTransactionComponent.Subscribe_Button_Text;
            SubscribeButtonURL = strategicTransactionComponent.Subscribe_Button_URL != null
                ? strategicTransactionComponent.Subscribe_Button_URL.Url
                : string.Empty;
        }

        public string Logo { get; set; }
        public string Body { get; set; }
        public string SubscribeButtonText { get; set; }
        public string SubscribeButtonURL { get; set; }
    }
}
