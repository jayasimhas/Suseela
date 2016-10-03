using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Components;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Glass.Mapper.Sc.Fields;

namespace Informa.Web.ViewModels.PopOuts
{
    public class VideoPopOutViewModel : GlassViewModel<IVideoControl>
    {
        #region VideoControl Parameters
        public Image Thumbnail => GlassModel?.Thumbnail;
        public string VideoLink => GlassModel?.VideoLink?.Url;
        public string AlternativeText => GlassModel?.AlternativeText;
        #endregion
    }
}