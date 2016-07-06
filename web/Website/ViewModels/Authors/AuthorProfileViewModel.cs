using System.Linq;
using System.Web;
using Informa.Library.Authors;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Jabberwocky.Autofac.Attributes;
using Jabberwocky.Glass.Autofac.Mvc.Models;

namespace Informa.Web.ViewModels.Authors
{
    public class AuthorProfileViewModel : GlassViewModel<IStaff_Item>
    {
        private readonly IDependencies _dependencies;

        [AutowireService(IsAggregateService = true)]
        public interface IDependencies
        {
            IAuthorIndexClient AuthorIndexClient { get; set; }
        }
        public AuthorProfileViewModel(IDependencies dependencies)
        {
            _dependencies = dependencies;
            Author = _dependencies.AuthorIndexClient.GetAuthorByUrlName(NameFromUrl);
            if (Author == null)
            {
                var curUrl = HttpContext.Current.Request.RawUrl;
                HttpContext.Current.Response.Redirect($"/404?url={curUrl}", true);
            }
        }

        public IStaff_Item Author { get; }
        private string NameFromUrl => HttpContext.Current.Request.Url.Segments.Last();
        
    }
}