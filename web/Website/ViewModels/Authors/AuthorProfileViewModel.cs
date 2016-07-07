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

            if( Author == null ) { HandleNullAuthor(); }
        }

        private void HandleNullAuthor()
        {
            var curUrl = HttpContext.Current.Request.RawUrl;
            HttpContext.Current.Response.Redirect($"/404?url={curUrl}", true);
        }

        public IStaff_Item Author { get; }
        private string NameFromUrl => HttpContext.Current.Request.Url.Segments.Last();
        
	    public string FirstName => Author.First_Name ?? string.Empty;
		public string LastName => Author.Last_Name ?? string.Empty;
		public string Credentials => Author.Credentials ?? string.Empty;
	    public string Location => Author.Location ?? string.Empty;

	    public bool HasTwitter => Author.Twitter?.Length > 0;
		public string TwitterHandle => HasTwitter ? 
			( "@" + Author.Twitter.Replace("@","")) : string.Empty;
		public string TwitterUrl => HasTwitter ?
			("https://twitter.com/" + Author.Twitter.Replace("@", "")) : string.Empty;
		
		public bool HasEmail => Author.Email_Address?.Length > 0;
		public string Email => Author.Email_Address ?? string.Empty;

	    public bool HasImage => Author.Image != null;
		public string ImageUrl => HasImage ? Author.Image.Src : string.Empty;

	    public bool HasLinkedIn => Author.LinkedIn != null;
	    public string LinkedInUrl => HasLinkedIn ? (Author.LinkedIn.Url) : string.Empty;

	    public string Bio => Author.Bio;
    }
}