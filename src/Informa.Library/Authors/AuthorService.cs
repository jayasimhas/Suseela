using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Linq;
using Glass.Mapper.Sc;
using Informa.Library.Utilities.Attributes;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Jabberwocky.Autofac.Attributes;
using Informa.Library.Wrappers;

namespace Informa.Library.Authors
{
    public interface IAuthorService
    {
        string ConvertAuthorNameToUrlName(string authorName);
        string GetUrlName(Guid authorId);
        string GetUrlName([NotNull] IStaff_Item authorItem);
        string ConvertAuthorToUrl(IStaff_Item s);
        string ConvertUrlNameToLink(string authorUrlName);
        IStaff_Item GetCurrentAuthor();
    }

    [AutowireService]
    public class AuthorService : IAuthorService
    {
        private readonly IDependencies _dependencies;

        [AutowireService(IsAggregateService = true)]
        public interface IDependencies
        {
            ISitecoreService SitecoreService { get; set; }
            IAuthorIndexClient AuthorIndexClient { get; set; }
            IHttpContextProvider HttpContextProvider { get; set; }
        }

        public AuthorService(IDependencies dependencies)
        {
            _dependencies = dependencies;
        }

        public string GetUrlName(Guid authorId)
            => GetUrlName(_dependencies.SitecoreService.GetItem<IStaff_Item>(authorId));
        public string GetUrlName([NotNull] IStaff_Item authorItem)
        {
            return ConvertAuthorNameToUrlName(authorItem._Name);
        }

        public string ConvertAuthorToUrl(IStaff_Item s) {
            if (s == null)
                return string.Empty;
            
            return ConvertUrlNameToLink(GetUrlName(s._Id));
        }

        public string ConvertAuthorNameToUrlName(string authorName)
        {
            var nonCharsRegex = new Regex(@"[^a-z]+");
            var name = authorName.ToLower();
            name = nonCharsRegex.Replace(name, "-");
            name = name.Trim('-');

            return name;
        }

        public string ConvertUrlNameToLink(string authorUrlName)
        {
            StringBuilder authorUrl = new StringBuilder();
            authorUrl.Append($"{_dependencies.HttpContextProvider.Current.Request.Url.Scheme}://{_dependencies.HttpContextProvider.Current.Request.Url.Authority}{_dependencies.HttpContextProvider.Current.Request.ApplicationPath.TrimEnd('/')}/");
            authorUrl.Append("authors/" + authorUrlName);
            return authorUrl.ToString();
        }

        private IStaff_Item _CurrentAuthor;
        public IStaff_Item GetCurrentAuthor() 
        {
            if (_CurrentAuthor != null)
                return _CurrentAuthor;

            var nameFromUrl = _dependencies.HttpContextProvider.Current.Request.Url.Segments.Last();
            Guid? author = _dependencies.AuthorIndexClient.GetAuthorIdByUrlName(nameFromUrl);
            if (author == null)
                return _CurrentAuthor;

            _CurrentAuthor = _dependencies.SitecoreService.GetItem<IStaff_Item>(author.Value);
            return _CurrentAuthor;
        }
    }
}