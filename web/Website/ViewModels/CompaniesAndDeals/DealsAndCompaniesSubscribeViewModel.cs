using Glass.Mapper.Sc;
using Informa.Library.Site;
using Informa.Library.Utilities.References;
using Informa.Library.Utilities.WebUtils;
using Informa.Library.Wrappers;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Components;
using Jabberwocky.Autofac.Attributes;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Jabberwocky.Glass.Models;
using Informa.Library.Utilities.Settings;

namespace Informa.Web.ViewModels.CompaniesAndDeals
{
    public class DealsAndCompaniesSubscribeViewModel : GlassViewModel<IGlassBase>
    {
        [AutowireService(true)]
        public interface IDependencies
        {
            ISitecoreService SitecoreService { get; set; }
            IHttpContextProvider HttpContextProvider { get; }
            ISiteRootContext SiteRootContext { get; }
            ISiteSettings SiteSettingsContext { get; set; }
        }

        public DealsAndCompaniesSubscribeViewModel(IDependencies dependencies)
        {
            var dcdSubscribeComponent =
                dependencies.SitecoreService.GetItem<IDCDSubscribe>(Constants.DCDSubscribeComponent);
            if (dcdSubscribeComponent == null) return;

			PurchaseHeadline = dcdSubscribeComponent.Subscriber_Headline;
            PurchaseSubHeading = dcdSubscribeComponent.Subscriber_Subheading;
            PurchaseButtonText = dcdSubscribeComponent.Subscriber_Button_Text;

            var recordNumber = UrlUtils.GetLastUrlSement(dependencies.HttpContextProvider.Current);

            PurchaseButtonLink = string.Format(dependencies.SiteSettingsContext.OldDealsUrl, recordNumber);
            SubscriberHeadline = dcdSubscribeComponent.Promotional_Headline;
            SubscriberSubHeading = dcdSubscribeComponent.Promotional_Subheadline;
            SubscriberButtonText = dcdSubscribeComponent.Promotional_Button_Text;
            SubscriberButtonLink = dcdSubscribeComponent.Promotional_Button_Link != null
                ? dcdSubscribeComponent.Promotional_Button_Link.Url
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
