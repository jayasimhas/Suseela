using System.Linq;
using Glass.Mapper;
using Glass.Mapper.Sc;
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
            ISitecoreService SitecoreService { get; }
        }

        public FeaturedContentViewModel(IDependencies dependencies)
        {
            _dependencies = dependencies;
        }

        public string ReadMoreClass =>
            string.IsNullOrEmpty(GlassModel.Read_More_Styling) ? null : $"ReadMore-{GlassModel.Read_More_Styling}";

        public bool HasTitle =>
            !string.IsNullOrEmpty(GlassModel.Title);

        public bool HasReadMoreLink =>
            !string.IsNullOrEmpty(GlassModel.Read_More_Link?.Url);

        public bool HasDownloadLink =>
            !string.IsNullOrEmpty(GlassModel.Download_Link.Url);

        public string DownloadTypeIconUrl =>
            _dependencies.SitecoreService.GetItem<IGlassBase>(Constants.DownloadTypes)?
                ._ChildrenWithInferType.FirstOrDefault(option => option._Name.Equals(GlassModel.Download_Type))?
                .CastTo<IOption>()?.Icon.Src;
    }
}