using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glass.Mapper.Sc;
using Informa.Library.User.Authentication;
using Informa.Library.User.Content;
using Informa.Library.Utilities.Extensions;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Autofac.Attributes;
using Jabberwocky.Glass.Models;
using Jabberwocky.Glass.Services;
using Velir.Search.Core.Reference;

namespace Informa.Library.User.Search
{
	[AutowireService(LifetimeScope.PerScope)]
	public class SavedSearchService : IUserContentService<ISavedSearchSaveable, ISavedSearchDisplayable>
	{
		[AutowireService(true)]
		public interface IDependencies
		{
			IUserContentRepository<ISavedSearchEntity> Repository { get; }
			ISitecoreContext SitecoreContext { get; }
			ISiteContextService SiteContext { get; }
			IAuthenticatedUserContext UserContext { get; }
			IAuthenticatedUserSession UserSession { get; }
		}

		private const string SessionKey = nameof(SavedSearchService);
		private static IEqualityComparer<ISavedSearchEntity> _comparer = new SearchStringEqualityComparer();

		protected readonly IDependencies _;

		private readonly Lazy<ISearch> _searchPage;
		protected ISearch SearchPage => _searchPage.Value;

		private readonly Lazy<bool> _isAuthenticated;
		protected bool IsAuthenticated => _isAuthenticated.Value;

		public SavedSearchService(IDependencies dependencies)
		{
			_ = dependencies;
			_searchPage = new Lazy<ISearch>(() => _.SitecoreContext.GetHomeItem<IGlassBase>()._ChildrenWithInferType.OfType<ISearch>().FirstOrDefault());
			_isAuthenticated = new Lazy<bool>(() => _.UserContext.IsAuthenticated);
		}

		public bool Exists(ISavedSearchSaveable input)
		{
			var entity = new SavedSearchEntity
			{
				SearchString = ExtractQueryString(input.Url)
			};

			var results = GetContentFromSessionOrRepo();

			return results.Any(s => _comparer.Equals(s, entity));
		}

		public virtual IEnumerable<ISavedSearchDisplayable> GetContent()
		{
			if (!IsAuthenticated)
			{
				return Enumerable.Empty<ISavedSearchDisplayable>();
			}

			var results = GetContentFromSessionOrRepo();

			return results.Select(doc => new SavedSearchDisplayModel
			{
				Sources = ExtractSources(doc.SearchString),
				Title = doc.Name,
				Url = ConstructUrl(doc.SearchString),
				DateSaved = doc.DateCreated,
				AlertEnabled = doc.HasAlert
			}).OrderByDescending(r => r.DateSaved);
		}

		public virtual IContentResponse SaveContent(ISavedSearchSaveable input)
		{
			if (!IsAuthenticated)
			{
				return new ContentResponse
				{
					Success = false,
					Message = "User is not authenticated"
				};
			}
				
			var id = new SavedSearchEntity
			{
				Username = _.UserContext.User.Username,
				Name = input.Title
			};

			var entity = _.Repository.GetById(id);
			if (entity != null)
			{
				entity.HasAlert = input.AlertEnabled;

				var updateResponse = _.Repository.Update(entity);
				Clear();

				return updateResponse;
			}

			entity = id;
			entity.SearchString = ExtractQueryString(input.Url);
			entity.HasAlert = input.AlertEnabled;

			var addResponse = _.Repository.Add(entity);
			Clear();

			return addResponse;
		}

		public virtual IContentResponse DeleteContent(ISavedSearchSaveable input)
		{
			if (IsAuthenticated)
			{
				ISavedSearchEntity entity = new SavedSearchEntity
				{
					Username = _.UserContext.User.Username,
					Name = input.Title,
					SearchString = ExtractQueryString(input.Url)
				};

				if (string.IsNullOrWhiteSpace(entity.Name))
				{
					var results = GetContentFromSessionOrRepo();
					entity = results.FirstOrDefault(e => _comparer.Equals(e, entity));
				}

				var response = _.Repository.Delete(entity);
				Clear();

				return response;
			}

			return new ContentResponse
			{
				Success = false,
				Message = "User is not authenticated"
			};
		}

		public virtual void Clear()
		{
			_.UserSession.Clear(SessionKey);
		}

		protected virtual IEnumerable<string> ExtractSources(string url)
		{
			return url.ExtractParamValue("publication").Split(SiteSettings.ValueSeparator).Where(s => !string.IsNullOrEmpty(s)).Select(HttpUtility.UrlDecode);
		}

		protected virtual string ExtractQueryString(string url)
		{
			if (string.IsNullOrEmpty(url)) return string.Empty;

			string[] urlParts = url.Split('?');
			string querystring = urlParts.Length == 1 ? urlParts[0] : urlParts[1];

			var cleansedQuerystring = querystring.Contains("#") ? querystring.Split('#')[0] : querystring;
			var pairs = cleansedQuerystring.Split('&');

			return string.Join("&", pairs.Where(x => !x.StartsWith($"{SiteSettings.QueryString.PageKey}=")));
		}

		protected virtual string ConstructUrl(string searchString)
		{
			return $"{SearchPage._Url}#?{searchString}";
		}

		protected virtual IList<ISavedSearchEntity> GetContentFromSessionOrRepo()
		{
			var savedSearches = _.UserSession.Get<IList<ISavedSearchEntity>>(SessionKey);
			if (!savedSearches.HasValue)
			{
				IList<ISavedSearchEntity> results = _.Repository.GetMany(_.UserContext.User?.Username).ToList();

				_.UserSession.Set(SessionKey, results);

				return results;
			}

			return savedSearches.Value;
		}
	}
}