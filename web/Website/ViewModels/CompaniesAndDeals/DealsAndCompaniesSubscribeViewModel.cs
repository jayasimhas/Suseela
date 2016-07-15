using Glass.Mapper.Sc;
using Informa.Library.Utilities.References;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Components;
using Jabberwocky.Autofac.Attributes;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Jabberwocky.Glass.Models;

namespace Informa.Web.ViewModels.CompaniesAndDeals
{
    public class DealsAndCompaniesSubscribeViewModel : GlassViewModel<IGlassBase>
    {
        [AutowireService(true)]
        public interface IDependencies
        {
            ISitecoreService SitecoreService { get; set; }
        }

        public DealsAndCompaniesSubscribeViewModel(IDependencies dependencies)
        {
            var dcdSubscribeComponent =
                dependencies.SitecoreService.GetItem<IDCDSubscribe>(Constants.DCDSubscribeComponent);
            if (dcdSubscribeComponent == null) return;

            PurchaseHeadline = dcdSubscribeComponent.Purchase_Headline;
            PurchaseSubHeading = dcdSubscribeComponent.Purchase_Subheading;
            PurchaseButtonText = dcdSubscribeComponent.Purchase_Button_Text;
            //PurchaseButtonLink = dcdSubscribeComponent.Purchase_Headline;
            SubscriberHeadline = dcdSubscribeComponent.Subscriber_Headline;
            SubscriberSubHeading = dcdSubscribeComponent.Subscriber_SubHeadline;
            SubscriberButtonText = dcdSubscribeComponent.Subscribe_Button_Text;
            SubscriberButtonLink = dcdSubscribeComponent.Subscribe_Button_Link != null
                ? dcdSubscribeComponent.Subscribe_Button_Link.Url
                : string.Empty;
            ContactHeadline = dcdSubscribeComponent.Contact_Headline;
            ContactInfo = dcdSubscribeComponent.Contact_Info;
        }

        public string PurchaseHeadline { get; set; }
        public string PurchaseSubHeading { get; set; }
        public string PurchaseButtonText { get; set; }
        public string PurchaseButtonLink { get; set; }
        public string SubscriberHeadline { get; set; }
        public string SubscriberSubHeading { get; set; }
        public string SubscriberButtonText { get; set; }
        public string SubscriberButtonLink { get; set; }
        public string ContactHeadline { get; set; }
        public string ContactInfo { get; set; }
    }
}
