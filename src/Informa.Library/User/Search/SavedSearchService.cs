using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glass.Mapper.Sc;
using Informa.Library.User.Authentication;
using Informa.Library.User.Content;
using Informa.Library.Utilities.Extensions;
using Informa.Library.Utilities.Security;
using Informa.Library.Utilities.Settings;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Autofac.Attributes;
using Jabberwocky.Glass.Models;
using Jabberwocky.Glass.Services;
using Constants = Informa.Library.Utilities.References.Constants;
using SiteSettings = Velir.Search.Core.Reference.SiteSettings;
using Informa.Library.User.ProductPreferences;
using Informa.Library.SalesforceConfiguration;
using Informa.Library.Site;
using Informa.Library.Utilities.CMSHelpers;

namespace Informa.Library.User.Search
{
    [AutowireService(LifetimeScope.PerScope)]
    public class SavedSearchService : IUserContentService<ISavedSearchSaveable, ISavedSearchDisplayable>
    {
        [AutowireService(true)]
        public interface IDependencies
        {
            IUserContentRepository<ISavedSearchEntity> Repository { get; }
            IAddUserProductPreference AddUserProductPreference { get; }
            IGetUserProductPreferences GetUserProductPreferences { get; }
            IDeleteUserProductPreferences DeleteUserProductPreferences { get; }
            IUpdateUserProductPreference UpdateUserProductPreference { get; }
            ISalesforceConfigurationContext SalesforceConfigurationContext { get; }

            ISitecoreContext SitecoreContext { get; }
            ISiteContextService SiteContext { get; }
            ISiteRootContext SiteRootContext { get; }
            IVerticalRootContext VerticalRootContext { get; }
            IAuthenticatedUserContext UserContext { get; }
            IAuthenticatedUserSession UserSession { get; }
            ICrypto Crypto { get; }
            ISiteSettings SiteSettings { get; }
        }

        public static string SessionKey = nameof(SavedSearchService);
        private static readonly IEqualityComparer<ISavedSearchEntity> SavedSearchComparer = new SearchStringEqualityComparer();

        private readonly IDependencies _dependencies;

        private readonly Lazy<ISearch> _searchPage;
        protected ISearch SearchPage => _searchPage.Value;

        private readonly Lazy<bool> _isAuthenticated;
        protected bool IsAuthenticated => _isAuthenticated.Value;

        public SavedSearchService(IDependencies dependencies)
        {
            _dependencies = dependencies;
            _searchPage = new Lazy<ISearch>(() => _dependencies.SitecoreContext.GetHomeItem<IGlassBase>()._ChildrenWithInferType.OfType<ISearch>().FirstOrDefault());
            _isAuthenticated = new Lazy<bool>(() => _dependencies.UserContext.IsAuthenticated);
        }

        public bool Exists(ISavedSearchSaveable input)
        {
            if (!IsAuthenticated) return false;

            var entity = new SavedSearchEntity
            {
                SearchString = ExtractQueryString(input.Url),
                HasAlert = input.AlertEnabled
            };

            var results = GetContentFromSessionOrRepo();

            var entities = results != null ? results.Where(s => SavedSearchComparer.Equals(s, entity)) : null;

            return entities != null && entities.Any() && entity.HasAlert ? entities.Any(a => a.HasAlert) : entities != null && entities.Any();
        }

        public virtual IEnumerable<ISavedSearchDisplayable> GetContent()
        {
            if (!IsAuthenticated)
            {
                return Enumerable.Empty<ISavedSearchDisplayable>();
            }

            var results = GetContentFromSessionOrRepo();
            if (results != null)
            {
                return results.Select(doc => new SavedSearchDisplayModel
                {
                    Sources = ExtractSources(doc.SearchString),
                    Title = doc.Name,
                    Url = ConstructUrl(doc.SearchString),
                    DateSaved = doc.DateCreated,
                    AlertEnabled = doc.HasAlert,
                    Id = doc.Id
                }).OrderByDescending(r => r.DateSaved);
            }
            return Enumerable.Empty<ISavedSearchDisplayable>();
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
                Username = _dependencies.UserContext.User.Username,
                Name = input.Title
            };

            var entity = _dependencies.SalesforceConfigurationContext.IsNewSalesforceEnabled ?
                GetById(id) :
                _dependencies.Repository.GetById(id);
            if (entity != null)
            {
                entity.HasAlert = input.AlertEnabled;
                entity.SearchString = ExtractQueryString(input.Url);

                var updateResponse = _dependencies.SalesforceConfigurationContext.IsNewSalesforceEnabled ?
                    _dependencies.UpdateUserProductPreference.
                    UpdateUserSavedSearch(_dependencies.UserContext.User.AccessToken, entity) :
                    _dependencies.Repository.Update(entity);
                Clear();

                return updateResponse;
            }

            entity = id;
            entity.SearchString = ExtractQueryString(input.Url);
            entity.UnsubscribeToken = GetUnsubscribeToken(entity);
            entity.Publication = _dependencies.SiteSettings.GetCurrentSiteInfo()?.HostName;
            entity.HasAlert = input.AlertEnabled;
            entity.PublicationCode = _dependencies.SiteRootContext?.Item?.Publication_Code;
            entity.PublicationName = _dependencies.SiteRootContext?.Item?.Publication_Name;
            entity.VerticalName = _dependencies.VerticalRootContext?.Item?._Name;
            var addResponse = _dependencies.SalesforceConfigurationContext.IsNewSalesforceEnabled ?
                _dependencies.AddUserProductPreference.AddUserSavedSearch(_dependencies.UserContext.User.AccessToken, entity) :
                _dependencies.Repository.Add(entity);
            Clear();

            return addResponse;
        }

        public virtual IContentResponse DeleteContent(ISavedSearchSaveable input)
        {
            if (!IsAuthenticated)
            {
                return new ContentResponse
                {
                    Success = false,
                    Message = "User is not authenticated"
                };
            }

            ISavedSearchEntity entity = new SavedSearchEntity
            {
                Username = _dependencies.UserContext.User.Username,
                Name = input.Title,
                SearchString = ExtractQueryString(input.Url),
                Id = input.Id
            };

            IContentResponse response = null;
            if (string.IsNullOrWhiteSpace(entity.Name))
            {
                var results = GetContentFromSessionOrRepo();
                if (results != null)
                {
                    var entities = results.Where(e => SavedSearchComparer.Equals(e, entity));
                    foreach (ISavedSearchEntity en in entities)
                    {
                        response = _dependencies.SalesforceConfigurationContext.IsNewSalesforceEnabled ?
                            _dependencies.DeleteUserProductPreferences.DeleteUserProductPreference(
                                _dependencies.UserContext.User.AccessToken, en.Id) :
                            _dependencies.Repository.Delete(en);
                    }
                }
            }
            else
            {
                response = _dependencies.SalesforceConfigurationContext.IsNewSalesforceEnabled ?
                        _dependencies.DeleteUserProductPreferences.DeleteUserProductPreference(
                            _dependencies.UserContext.User.AccessToken, entity.Id) :
                    _dependencies.Repository.Delete(entity);
            }

            Clear();

            return response;
        }

        public virtual void Clear()
        {
            _dependencies.UserSession.Clear(SessionKey);
        }

        private string GetUnsubscribeToken(ISavedSearchEntity entity)
        {
            var toEncrypt = string.Concat(entity.Username, "|", entity.Name);
            var encrypted = _dependencies.Crypto.EncryptStringAes(toEncrypt, Constants.CryptoKey);
            return HttpUtility.UrlEncode(encrypted);
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
            var savedSearches = _dependencies.UserSession.Get<IList<ISavedSearchEntity>>(SessionKey);
            if (!savedSearches.HasValue)
            {
                var results = _dependencies.SalesforceConfigurationContext.IsNewSalesforceEnabled ?
                    _dependencies.GetUserProductPreferences.GetProductPreferences<IList<ISavedSearchEntity>>
                    (_dependencies.UserContext.User, _dependencies.SiteRootContext?.Item?.Publication_Code,
                    ProductPreferenceType.SavedSearches) :
                    _dependencies.Repository.GetMany(_dependencies.UserContext.User?.Username).ToList();

                _dependencies.UserSession.Set(SessionKey, results);

                return results;
            }

            return savedSearches.Value;
        }

        protected virtual ISavedSearchEntity GetById(object item)
        {
            var itemId = item as ISavedSearchItemId;

            if (string.IsNullOrEmpty(itemId?.Username) || string.IsNullOrEmpty(itemId.Name)) return default(ISavedSearchEntity);
            var results = GetContentFromSessionOrRepo();
            return results != null ?
                results.Where(s => s.Name == itemId.Name && s.Username == itemId.Username).FirstOrDefault() : null;
        }
    }
}