using System;
using System.Collections.Generic;
using System.Linq;
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
		private const string SessionKey = nameof(SavedSearchService);
		
		protected readonly IDependencies _;

		private readonly Lazy<ISearch> _searchPage;
		protected ISearch SearchPage => _searchPage.Value;

		public SavedSearchService(IDependencies dependencies)
		{
			_ = dependencies;
			_searchPage = new Lazy<ISearch>(() => _.SitecoreContext.GetHomeItem<IGlassBase>()._ChildrenWithInferType.OfType<ISearch>().FirstOrDefault());
		}

		public virtual IEnumerable<ISavedSearchDisplayable> GetContent()
		{
			if (!_.UserContext.IsAuthenticated)
			{
				return Enumerable.Empty<ISavedSearchDisplayable>();
			}

			var savedSearches = _.UserSession.Get<IEnumerable<ISavedSearchEntity>>(SessionKey);
			if (!savedSearches.HasValue)
			{
				_.UserSession.Set(SessionKey, _.Repository.GetMany(_.UserContext.User.Username));
				savedSearches = _.UserSession.Get<IEnumerable<ISavedSearchEntity>>(SessionKey);
			}

			return savedSearches.Value.Select(doc => new SavedSearchDisplayModel
			{
				Sources = ExtractSources(doc.SearchString),
				Title = doc.Name,
				Url = ConstructUrl(doc.SearchString),
				DateSaved = doc.DateCreated,
				AlertEnabled = doc.HasAlert
			});
		}

		public virtual IContentResponse SaveContent(ISavedSearchSaveable input)
		{
			if (_.UserContext.IsAuthenticated)
			{
				var id = new SavedSearchEntity
				{
					Username = _.UserContext.User.Name,
					Name = input.Title
				};

				var entity = _.Repository.GetById(id);
				if (entity != null)
				{
					return null;
				}

				entity = id;
				entity.SearchString = input.Url;
				entity.HasAlert = input.AlertEnabled;

				_.Repository.Add(entity);
				Clear();
			}

			return null;
		}

		public virtual IContentResponse DeleteContent(ISavedSearchSaveable input)
		{
			if (_.UserContext.IsAuthenticated)
			{
				var entity = new SavedSearchEntity
				{
					Username = _.UserContext.User.Name,
					Name = input.Title,
					SearchString = input.Url
				};

				_.Repository.Delete(entity);
				Clear();
			}

			return null;
		}

		public virtual void Clear()
		{
			_.UserSession.Clear(SessionKey);
		}

		protected virtual IEnumerable<string> ExtractSources(string url)
		{
			return url.ExtractParamValue("publication").Split(SiteSettings.ValueSeparator);
		}

		protected virtual string ConstructUrl(string searchString)
		{
			return $"{SearchPage._Url}?{searchString}";
		}
	}

	[AutowireService(true)]
	public interface IDependencies
	{
		IUserContentRepository<ISavedSearchEntity> Repository { get; }
		ISitecoreContext SitecoreContext { get; }
		ISiteContextService SiteContext { get; }
		IAuthenticatedUserContext UserContext { get; }
		IAuthenticatedUserSession UserSession { get; }
	}
}