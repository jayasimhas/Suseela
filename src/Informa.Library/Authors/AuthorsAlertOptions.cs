using Glass.Mapper.Sc;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.View_Templates;
using Jabberwocky.Autofac.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Informa.Library.Authors {
    [AutowireService]
    public class AuthorAlertOptions : IAuthorAlertOptions {
        
        private readonly IAuthorService AuthorService;

        public AuthorAlertOptions(IAuthorService authorService) {
            AuthorService = authorService;
        }

        public ITopic_Alert_Options SetOptions(ITopic_Alert_Options options) {

            var a = AuthorService?.GetCurrentAuthor() ?? null;
            if (a == null)
                return options;

            options.Search_Name = $"{a?.First_Name} {a?.Last_Name}" ?? string.Empty;
            options.Related_Search = $"?author={a?._Id.ToString("N")}";

            return options;
        }
    }
}
