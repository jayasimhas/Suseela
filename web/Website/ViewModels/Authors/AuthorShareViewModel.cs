using System.Linq;
using Informa.Library.Authors;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Informa.Library.Wrappers;
using Informa.Library.Globalization;
using Jabberwocky.Autofac.Attributes;

namespace Informa.Web.ViewModels.Authors
{
    public interface IAuthorShareViewModel {
        string PageTitle { get; set; }
        string PageUrl { get; }
        string ShareText { get; }
    }

    [AutowireService]
    public class AuthorShareViewModel : GlassViewModel<IStaff_Item>, IAuthorShareViewModel
	{
        protected readonly IAuthorIndexClient AuthorIndexClient;
        protected readonly IHttpContextProvider HttpContext;
        protected readonly ITextTranslator TextTranslator;

        public AuthorShareViewModel(
            IAuthorIndexClient authorIndexClient,
            IHttpContextProvider httpContext,
            ITextTranslator textTranslator)
		{
            AuthorIndexClient = authorIndexClient;
            HttpContext = httpContext;
            TextTranslator = textTranslator;

            PageTitle = string.Empty;
            var Author = AuthorIndexClient.GetAuthorByUrlName(HttpContext.Current.Request.Url.Segments.Last());
            if (Author == null) { HandleNullAuthor(); }

            PageTitle = $"{Author.First_Name} {Author.Last_Name}";
        }

        public string PageTitle { get; set; }
        public string PageUrl => $"{HttpContext.Current.Request.Url.Scheme}://{HttpContext.Current.Request.Url.Host}{HttpContext.Current.Request.Url.PathAndQuery}";

        private void HandleNullAuthor()
		{
			var curUrl = HttpContext.Current.Request.RawUrl;
			HttpContext.Current.Response.Redirect($"/404?url={curUrl}", true);
		}

        public string ShareText => TextTranslator.Translate("Author.Share");
    }
}