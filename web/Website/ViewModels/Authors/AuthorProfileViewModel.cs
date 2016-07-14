using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glass.Mapper.Sc;
using Informa.Library.Authors;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Jabberwocky.Autofac.Attributes;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Jabberwocky.Glass.Models;
using Informa.Library.Utilities.References;

namespace Informa.Web.ViewModels.Authors
{
	public class AuthorProfileViewModel : GlassViewModel<IStaff_Item>
	{
		private readonly IDependencies _dependencies;

		[AutowireService(IsAggregateService = true)]
		public interface IDependencies
		{
			IAuthorIndexClient AuthorIndexClient { get; set; }
			ISitecoreContext SitecoreContext { get; }
            IItemReferences ItemReferences { get; }
		}
		public AuthorProfileViewModel(IDependencies dependencies)
		{
			_dependencies = dependencies;

			Author = _dependencies.AuthorIndexClient.GetAuthorByUrlName(NameFromUrl);

			if (Author == null) { HandleNullAuthor(); }
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
			("@" + Author.Twitter.Replace("@", "")) : string.Empty;
		public string TwitterUrl => HasTwitter ?
			("https://twitter.com/" + Author.Twitter.Replace("@", "")) : string.Empty;

		public bool HasEmail => Author.Email_Address?.Length > 0;
		public string Email => Author.Email_Address ?? string.Empty;

		public bool HasImage => Author.Image != null;
		public string ImageUrl => HasImage ? Author.Image.Src : string.Empty;

		public bool HasLinkedIn => Author.LinkedIn != null;
		public string LinkedInUrl => HasLinkedIn ? (Author.LinkedIn.Url) : string.Empty;

		public string Bio => Author.Bio;

		public TaxonomyLinkViewModel[] AreasOfExpertise {
			get
			{
				return Author.Areas_Of_Expertises != null
					? Author.Areas_Of_Expertises?.Cast<ITaxonomy_Item>().Select(i => ConvertToTaxonomyLink(i, Author, GetKeyName(i))).ToArray()
					: new List<TaxonomyLinkViewModel>().ToArray();
			}
		}
	
		public bool PersonHasAreasOfExpertise => AreasOfExpertise.Any();

		public TaxonomyLinkViewModel[] IndustryExpertise
		{

			get
			{
				return Author.Industry_Expertises != null
					? Author.Industry_Expertises?.Cast<ITaxonomy_Item>().Select(i => ConvertToTaxonomyLink(i, Author, "industries")).ToArray()
					: new List<TaxonomyLinkViewModel>().ToArray();
			}
		}

		public bool PersonHasIndustryExpertise => IndustryExpertise.Any();

		public TaxonomyLinkViewModel[] WriteForExpertise
		{
			get
			{
				return Author.Writes_Fors != null
					? Author.Writes_Fors.Cast<ISite_Root>().Select(i => ConvertToTaxonomyLink(i, Author)).ToArray()
					: new List<TaxonomyLinkViewModel>().ToArray();
			}
		}

		public bool PersonWritesFor => WriteForExpertise.Any();

		private TaxonomyLinkViewModel ConvertToTaxonomyLink(ITaxonomy_Item item, IGlassBase glassItem, string type)
		{
			return new TaxonomyLinkViewModel
			{
				LinkText = item.Item_Name,
				LinkUrl = $"/search#?author={glassItem._Id.ToString("N")}&{type}={HttpUtility.UrlEncode(item.Item_Name)}"
			};
		}
		private TaxonomyLinkViewModel ConvertToTaxonomyLink(ISite_Root item, IGlassBase glassItem)
		{
			return new TaxonomyLinkViewModel
			{
				LinkText = item.Publication_Name,
				LinkUrl = $"/search#?author={glassItem._Id.ToString("N")}&publication={HttpUtility.UrlEncode(item.Publication_Name)}"
			};
		}

		private string GetKeyName(ITaxonomy_Item item){
            
            IGlassBase parent = item._Parent;
            while (!parent._TemplateId.Equals(_dependencies.ItemReferences.FolderTemplate) || parent._Name.ToLower().Equals("content")) {
                parent = parent._Parent;
            }

            if (parent._Id.Equals(_dependencies.ItemReferences.DeviceAreasTaxonomyFolder))
                return "deviceareas";
            else if (parent._Id.Equals(_dependencies.ItemReferences.TherapyAreasTaxonomyFolder))
                return "areas";
            else if (parent._Id.Equals(_dependencies.ItemReferences.SubjectsTaxonomyFolder))
                return "subjects";
            else if (parent._Id.Equals(_dependencies.ItemReferences.RegionsTaxonomyFolder))
                return "regions";
            else
                return "value";
		}
	}

	public class TaxonomyLinkViewModel
	{
		public string LinkText { get; set; }
		public string LinkUrl { get; set; }
	}
}