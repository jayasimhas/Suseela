using Informa.Library.Advertisement;
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

        [AutowireService(IsAggregateService = true)]
        public interface IDependencies
        {
            ITextTranslator TextTranslator { get; }
            ICampaignQueryBuilder CampaignQueryBuilder { get; }
            IAdUrlBuilder AdUrlBuilder { get; }
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
                (_adClickLinkUrl = _dependencies.CampaignQueryBuilder.AddCampaignQuery(
                    _dependencies.AdUrlBuilder.GetLink(GlassModel, true)));

        public string AdImageUrl => _dependencies.AdUrlBuilder.GetImageSrc(GlassModel, true);

        public string CampaignQuery => _dependencies.CampaignQueryBuilder.GetCampaignQuery();
    }
}
