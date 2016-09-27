using System;
using System.Web;
using Informa.Library.Globalization;
using Informa.Library.Presentation;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Web.ViewModels.DataTools
{
	[AutowireService(LifetimeScope.PerScope)]
	public class DataToolPrologueShareViewModel : IDataToolPrologueShareViewModel
    {
		private readonly Lazy<IGeneral_Content_Page> _dataToolPage; 

		protected readonly ITextTranslator TextTranslator;
		
		public DataToolPrologueShareViewModel(
				ITextTranslator textTranslator,
				IRenderingItemContext articleRenderingContext)
		{
			TextTranslator = textTranslator;

            _dataToolPage = new Lazy<IGeneral_Content_Page>(articleRenderingContext.Get<IGeneral_Content_Page>);
		}

		public string Title => _dataToolPage.Value.Title;
		public string Url => $"{HttpContext.Current.Request.Url.Scheme}://{HttpContext.Current.Request.Url.Host}{_dataToolPage.Value._Url}";
		public string ShareText => TextTranslator.Translate("Article.Share");
	}
}