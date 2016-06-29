using Glass.Mapper.Sc;
using Informa.Library.Site;
using Informa.Library.Utilities.References;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Emails;
using Jabberwocky.Autofac.Attributes;

namespace Informa.Library.Mail.ExactTarget
{
    public interface ICampaignQueryBuilder
    {
        string AddCampaignQuery(string linkUrl);
        string GetCampaignQuery();
    }

    [AutowireService]
    public class CampaignQueryBuilder : ICampaignQueryBuilder
    {
        private readonly IDependencies _dependencies;

        [AutowireService(IsAggregateService = true)]
        public interface IDependencies
        {
            ISiteRootContext SiteRootContext { get; }
            ISitecoreContext SitecoreContext { get; }
        }

        public CampaignQueryBuilder(IDependencies dependencies)
        {
            _dependencies = dependencies;
        }

        private string _campaignQuery;
        public string GetCampaignQuery()
        {
            if(_campaignQuery != null) { return _campaignQuery;}

            var siteRoot = _dependencies.SiteRootContext.Item;
            var currentItem = _dependencies.SitecoreContext.GetCurrentItem<IExactTarget_Email>();

            _campaignQuery = $"{Constants.QueryString.UtmCampaign}={siteRoot.Publication_Name}"
                             + $"&{Constants.QueryString.UtmSource}={currentItem.Source}"
                             + $"&{Constants.QueryString.UtmMedium}={currentItem.Medium}";
            return _campaignQuery;
        }

        public string AddCampaignQuery(string linkUrl)
        {
            if(string.IsNullOrEmpty(linkUrl)) { return string.Empty; }
            
            var query = _campaignQuery ?? (_campaignQuery = GetCampaignQuery());
            var separator = linkUrl.Contains("?") ? "&" : "?";
            return linkUrl + separator + query;
        }
    }
}