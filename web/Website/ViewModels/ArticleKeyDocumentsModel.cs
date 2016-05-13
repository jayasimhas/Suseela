using System.Collections.Generic;
using Glass.Mapper.Sc;
using Informa.Library.Globalization;
using Informa.Library.User.Entitlement;
using Informa.Models.Informa.Models.sitecore.templates.System.Media.Unversioned;
using Jabberwocky.Glass.Models;

namespace Informa.Web.ViewModels
{
	public class ArticleKeyDocumentsModel : ArticleEntitledViewModel
	{
		protected readonly ISitecoreContext SitecoreContext;
		protected readonly ITextTranslator TextTranslator;

		public ArticleKeyDocumentsModel(ISitecoreContext context, ITextTranslator textTranslator, IIsEntitledProducItemContext entitledProductContext) : base(entitledProductContext)
		{
			SitecoreContext = context;
			TextTranslator = textTranslator;
		}

		public string KeyDocumentHeader => TextTranslator.Translate("Article.KeyDocs");
		public IEnumerable<IGlassBase> KeyDocuments => GlassModel.Supporting_Documents;

		public string GetTitle(IGlassBase g)
		{
			IFile f = SitecoreContext.GetItem<IFile>(g._Id);
			return !string.IsNullOrEmpty(f?.Title)
					? f.Title
					: g._Name;
		}

		public string GetDocumentIcon(IGlassBase g)
		{
			IFile f = SitecoreContext.GetItem<IFile>(g._Id);
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