using Informa.Library.Authors;
using System;
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
        }

        public string Output => _dependencies.AuthorIndexClient.GetAuthorIdByUrlName("moose").ToString();
	    public string FirstName => GlassModel.First_Name ?? string.Empty;
		public string LastName => GlassModel.Last_Name ?? string.Empty;
		public string Credentials => GlassModel.Credentials ?? string.Empty;
	    public string Location => GlassModel.Location ?? string.Empty;

	    public bool HasTwitter => GlassModel.Twitter?.Length > 0;
		public string TwitterHandle => HasTwitter ? 
			( "@" + GlassModel.Twitter.Replace("@","")) : string.Empty;
		public string TwitterUrl => HasTwitter ?
			("https://twitter.com/" + GlassModel.Twitter.Replace("@", "")) : string.Empty;
		
		public bool HasEmail => GlassModel.Email_Address?.Length > 0;
		public string Email => GlassModel.Email_Address ?? string.Empty;

	    public bool HasImage => GlassModel.Image != null;
		public string ImageUrl => HasImage ? GlassModel.Image.Src : string.Empty;

	    public bool HasLinkedIn => GlassModel.LinkedIn != null;
	    public string LinkedInUrl => HasLinkedIn ? (GlassModel.LinkedIn.Url) : string.Empty;

	    public string Bio => GlassModel.Bio;
    }
}