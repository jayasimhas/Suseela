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
using Informa.Library.Globalization;
using Informa.Library.Search.ComputedFields.Facets;
using Informa.Library.Utilities.References;

namespace Informa.Library.Authors
{
	[AutowireService]
	public class AuthorAlertOptions : IAuthorAlertOptions
	{

		private readonly IAuthorService AuthorService;
		private readonly ITextTranslator TextTranslator;

		public AuthorAlertOptions(IAuthorService authorService, ITextTranslator textTranslator)
		{
			AuthorService = authorService;
			TextTranslator = textTranslator;
		}

		public ITopic_Alert_Options SetOptions(ITopic_Alert_Options options)
		{

			var a = AuthorService?.GetCurrentAuthor() ?? null;
			if (a == null)
				return options;

			options.Search_Name = $"{TextTranslator.Translate("Author.TopicAlert.Prefix")} {a?.First_Name} {a?.Last_Name}" ?? string.Empty;
			options.Related_Search = $"?{Constants.QueryString.AuthorFullName}={AuthorNamesField.ToAuthorName(a)}";

			return options;
		}
	}
}
