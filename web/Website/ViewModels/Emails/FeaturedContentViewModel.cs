using System;
using System.Linq;
using Glass.Mapper;
using Glass.Mapper.Sc;
using Informa.Library.Mail.ExactTarget;
using Informa.Library.Services.Global;
using Informa.Library.Utilities.References;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Emails.Components;
using Jabberwocky.Autofac.Attributes;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Jabberwocky.Glass.Models;

namespace Informa.Web.ViewModels.Emails
{
    public class FeaturedContentViewModel : GlassViewModel<IFeatured_Content>
    {
        private readonly IDependencies _dependencies;

        [AutowireService(true)]
        public interface IDependencies
        {
            IGlobalSitecoreService GlobalService { get; }
            IItemReferences ItemReferences { get; }
            ICampaignQueryBuilder CampaignQueryBuilder { get; }
        }

        public FeaturedContentViewModel(IDependencies dependencies)
        {
            _dependencies = dependencies;
        }

        public string ReadMoreClass => GlassModel.Read_More_Styling.Equals("Button", StringComparison.InvariantCultureIgnoreCase)
                                            ? "featured__read-more--button"
                                            : string.Empty;

        public bool HasTitle => !string.IsNullOrEmpty(GlassModel.Title);

        public bool HasReadMoreLink => !string.IsNullOrEmpty(GlassModel.Read_More_Link?.Url);

        public bool HasImage => !string.IsNullOrEmpty(GlassModel.Image?.Src);

        public bool HasDownloadLink =>
            !string.IsNullOrEmpty(GlassModel.Download_Link.Url) && !string.IsNullOrEmpty(GlassModel.Download_Link_Text);

        public bool HasRightRail => HasDownloadLink || HasImage;

        private string _downloadTypeIconUrl;
        public string DownloadTypeIconUrl =>
            _downloadTypeIconUrl ?? (_downloadTypeIconUrl =
                _dependencies.GlobalService.GetItem<IGlassBase>(_dependencies.ItemReferences.DownloadTypes)?
                    ._ChildrenWithInferType.FirstOrDefault(option => option._Name.Equals(GlassModel.Download_Type))?
                    .CastTo<IOption>()?.Icon?.Src);

        private string _titleLinkUrl;
        public string TitleLinkUrl
            => _titleLinkUrl ??
                (_titleLinkUrl = _dependencies.CampaignQueryBuilder.AddCampaignQuery(GlassModel.Title_Link?.Url));

        private string _readMoreLinkUrl;
        public string ReadMoreLinkUrl
            => _readMoreLinkUrl ??
                (_readMoreLinkUrl = _dependencies.CampaignQueryBuilder.AddCampaignQuery(GlassModel.Read_More_Link?.Url));

        private string _downloadLinkUrl;
        public string DownloadLinkUrl
            => _downloadLinkUrl ??
                (_downloadLinkUrl = _dependencies.CampaignQueryBuilder.AddCampaignQuery(GlassModel.Download_Link?.Url));
    }
}
