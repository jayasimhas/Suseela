using System;
using System.Collections.Generic;
using System.Linq;
using Informa.Library.Globalization;
using Informa.Library.Search.Utilities;
using Informa.Library.Services.Global;
using Informa.Library.Taxonomy;
using Informa.Models.Informa.Models.sitecore.templates.Common;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Jabberwocky.Glass.Autofac.Mvc.Models;

namespace Informa.Web.ViewModels
{
	public class HierarchyLinksViewModel : GlassViewModel<I___BaseTaxonomy>, IHierarchyLinks
	{
		private HierarchyLinks model;
		protected readonly ITextTranslator TextTranslator;
	    protected readonly ITaxonomyService TaxonomyService;

		public HierarchyLinksViewModel(
				I___BaseTaxonomy glassModel,
				ITextTranslator textTranslator,
                ITaxonomyService taxonomyService)
		{
			TextTranslator = textTranslator;
		    TaxonomyService = taxonomyService;

            model = new HierarchyLinks();

			model.Text = "Related Topics";
			model.Url = string.Empty;
            model.Children = TaxonomyService.GetHeirarchyChildLinks(glassModel);
		}

		public IEnumerable<IHierarchyLinks> Children => model.Children;
		public string Path { get; set; }

		public string Text => model.Text;

		public string RelatedTaxonomyHeader => TextTranslator.Translate("Article.RelTaxHeader");

		public string Url => model.Url;
	}
}