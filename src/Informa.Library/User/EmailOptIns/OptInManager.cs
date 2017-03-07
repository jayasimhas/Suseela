using System;
using System.Linq;
using Glass.Mapper.Sc;
using Informa.Library.Globalization;
using Informa.Library.Publication;
using Informa.Library.User.Authentication;
using Informa.Library.User.Content;
using Informa.Library.User.Newsletter;
using Informa.Library.User.Offer;
using Informa.Library.User.Search;
using Informa.Library.Utilities.References;
using Informa.Library.Utilities.Security;
using Informa.Library.ViewModels.Account;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages.Account;
using Jabberwocky.Autofac.Attributes;

namespace Informa.Library.User.EmailOptIns
{
    public interface IOptInManager
    {
        OptInResponseModel OptIn(string publicationName);
        OptInResponseModel OptOut(string userName, string type, string publicationName);
        OptInResponseModel AnnonymousOptOut(string token);
    }

    [AutowireService]
    public class OptInManager : IOptInManager
    {
        private readonly IDependencies _dependencies;

        [AutowireService(IsAggregateService = true)]
        public interface IDependencies
        {
            ISitecoreContext SitecoreContext { get; }
            IAuthenticatedUserContext AuthenticatedUserContext { get; }
            IUpdateNewsletterUserOptInsContext UpdateNewsletterUserOptInsContext { get; }
            ISignInViewModel SignInViewModel { get; }
            ISitePublicationNameContext SitePublicationNameContext { get; }
            INewsletterUserOptInFactory NewsletterUserOptInFactory { get; }
            IUpdateSiteNewsletterUserOptIn UpdateSiteNewsletterUserOptIn { get; }
            IUpdateOfferUserOptIn UpdateOfferUserOptIn { get; }
            IUpdateOfferUserOptInContext UpdateOfferUserOptInContext { get; }
            ICrypto Crypto { get; }
            IUserContentRepository<ISavedSearchEntity> SavedSearchEntityRepository { get; }
            IAuthenticatedUserSession AuthenticatedUserSession { get; }
            ITextTranslator TextTranslator { get; }
        }

        public OptInManager(IDependencies dependencies)
        {
            _dependencies = dependencies;
        }

        public OptInResponseModel OptIn(string publicationName)
        {
            var page = _dependencies.SitecoreContext.GetCurrentItem<ISubscribe_Page>();

            var responseModel = new OptInResponseModel
            {
                BodyText = page.Body.Replace("#USER_EMAIL#", _dependencies.AuthenticatedUserContext?.User?.Username ?? string.Empty),
                IsAuthenticated = _dependencies.AuthenticatedUserContext?.IsAuthenticated ?? false,
                SignInViewModel = _dependencies.SignInViewModel
            };

            if (!responseModel.IsAuthenticated)
            {
                //redirect to email preferences
                var url = page._Parent?._Url ?? string.Empty;
                if (!string.IsNullOrEmpty(url))
                {
                    responseModel.RedirectUrl = url;
                }
                return responseModel;
            }

            var isCurrentPublication = !string.IsNullOrEmpty(publicationName)
                                       && (string.Equals(publicationName, _dependencies.SitePublicationNameContext.Name,
                                           StringComparison.CurrentCultureIgnoreCase));
            if (isCurrentPublication)
            {
                var userOptIn = _dependencies.NewsletterUserOptInFactory.Create(_dependencies.SitePublicationNameContext.Name, true);
                _dependencies.UpdateNewsletterUserOptInsContext.Update(new[] { userOptIn });
            }

            return responseModel;
        }

        public OptInResponseModel OptOut(string userName, string type, string publicationName)
        {
            var page = _dependencies.SitecoreContext.GetCurrentItem<IUnsubscribe_Page>();

            var responseModel = new OptInResponseModel
            {
                BodyText = page.Body,
                SignInViewModel = _dependencies.SignInViewModel
            };

            if (string.IsNullOrEmpty(type))
            {
                return responseModel;
            }

            //process unsubscribe
            var newsletterType = _dependencies.SitePublicationNameContext.Name;

            if (type.ToLower() == "newsletter" && !string.IsNullOrEmpty(publicationName) && (publicationName.ToLower() == newsletterType.ToLower()))
            {
                if (_dependencies.AuthenticatedUserContext.IsAuthenticated)
                {
                    var userOptIn = _dependencies.NewsletterUserOptInFactory.Create(_dependencies.SitePublicationNameContext.Name, false);
                    _dependencies.UpdateNewsletterUserOptInsContext.Update(new[] { userOptIn });
                }
                else if (!string.IsNullOrWhiteSpace(userName))
                {
                    _dependencies.UpdateSiteNewsletterUserOptIn.Update(userName, false);
                }
            }
            else if (type.ToLower() == "promotions")
            {
                if (responseModel.IsAuthenticated)
                {
                    _dependencies.UpdateOfferUserOptInContext.Update(false, NewsletterPreference.Update);
                }
                else if (!string.IsNullOrWhiteSpace(userName))
                {
                    _dependencies.UpdateOfferUserOptIn.Update(userName, false);
                }
            }

            return responseModel;
        }

        public OptInResponseModel AnnonymousOptOut(string token)
        {
            var entity = ParseToken(token);
            var response = new OptInResponseModel {IsAuthenticated = false};

            response.IsSuccessful = entity != null 
                && _dependencies.SavedSearchEntityRepository.Delete(entity).Success;

            if (response.IsSuccessful)
            {
                response.BodyText = entity.Name;
            }

            if (_dependencies.AuthenticatedUserContext.IsAuthenticated)
            {
                _dependencies.AuthenticatedUserSession.Clear(SavedSearchService.SessionKey);
            }

            return response;
        }

        public SavedSearchEntity ParseToken(string token)
        {
            string decrypted;
            try
            {
                decrypted = _dependencies.Crypto.DecryptStringAes(token, Constants.CryptoKey);
            }
            catch
            {
                return null;
            }

            if (string.IsNullOrEmpty(decrypted)) { return null; }

            var split = decrypted.Split('|');
            if (split.Length < 2 || split.Any(string.IsNullOrEmpty))
            { return null; }

            return new SavedSearchEntity
            {
                Username = split[0],
                Name = split[1]
            };
        }
    }
}