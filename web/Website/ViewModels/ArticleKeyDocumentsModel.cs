using System.Collections.Generic;
using Glass.Mapper.Sc;
using Informa.Library.Globalization;
using Informa.Library.Services.Article;
using Informa.Library.User.Entitlement;
using Informa.Models.Informa.Models.sitecore.templates.System.Media.Unversioned;
using Jabberwocky.Glass.Models;
using Informa.Library.User;

namespace Informa.Web.ViewModels
{
	public class ArticleKeyDocumentsModel : ArticleEntitledViewModel
	{
		protected readonly ITextTranslator TextTranslator;
	    protected readonly IArticleService ArticleService;

		public ArticleKeyDocumentsModel(
            ITextTranslator textTranslator, 
            IIsEntitledProducItemContext entitledProductContext,
            ISitecoreUserContext sitecoreUserContext,
            IArticleService articleService) : base(entitledProductContext, sitecoreUserContext)
		{
			TextTranslator = textTranslator;
		    ArticleService = articleService;

		}

		public string KeyDocumentHeader => TextTranslator.Translate("Article.KeyDocs");
		public IEnumerable<IFile> KeyDocuments => ArticleService.GetSupportingDocuments(GlassModel);

		public string GetTitle(IFile f) => !string.IsNullOrEmpty(f?.Title)
            ? f.Title
			: f._Name;

		public string GetDocumentIcon(IFile f)
		{
			if (string.IsNullOrEmpty(f?.Extension) || f.Extension.Equals("pdf"))
				return "pdf";
			if (f.Extension.Equals("doc") || f.Extension.Equals("docx"))
				return "doc";
			if (f.Extension.Equals("xls") || f.Extension.Equals("xlsx"))
				return "xls";
			return "pdf";
		}
	}
}