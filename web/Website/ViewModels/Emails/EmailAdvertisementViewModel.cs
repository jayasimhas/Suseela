using Informa.Library.Globalization;
using Informa.Library.Mail.ExactTarget;
using Informa.Library.Utilities.References;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Components;
using Jabberwocky.Autofac.Attributes;
using Jabberwocky.Glass.Autofac.Mvc.Models;

namespace Informa.Web.ViewModels.Emails
{
    public class EmailAdvertisementViewModel : GlassViewModel<IAdvertisement>
    {
        private readonly IDependencies _dependencies;

        private const string AdClickLink =
            "http://oasc-eu1.247realmedia.com/RealMedia/ads/click_nx.ads/www.scripdailyemail.com/dailyemail/11001%40";

        private const string AdImage =
            "http://oasc-eu1.247realmedia.com/RealMedia/ads/adstream_nx.ads/www.scripdailyemail.com/dailyemail/11001%40";

        [AutowireService(IsAggregateService = true)]
        public interface IDependencies
        {
            ITextTranslator TextTranslator { get; }
            ICampaignQueryBuilder CampaignQueryBuilder { get; }
        }

        public EmailAdvertisementViewModel(IDependencies dependencies)
        {
            _dependencies = dependencies;
        }

        private string _adHeader;
        public string AdHeader => _adHeader ?? (_adHeader = _dependencies.TextTranslator?.Translate(DictionaryKeys.AdvertisementHeader));

        private string _adClickLinkUrl;
        public string AdClickLinkUrl
            => _adClickLinkUrl ??
                (_adClickLinkUrl = _dependencies.CampaignQueryBuilder.AddCampaignQuery(AdClickLink + GlassModel.Slot_ID));

        public string AdImageUrl => AdImage + GlassModel.Slot_ID;

        public string CampaignQuery => _dependencies.CampaignQueryBuilder.GetCampaignQuery();
    }
}
