using System.Collections.Generic;
using System.Linq;
using Informa.Library.Presentation;
using Informa.Library.Search.Utilities;
using Informa.Models.FactoryInterface;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Jabberwocky.Glass.Autofac.Attributes;
using Informa.Library.Globalization;

namespace Informa.Web.ViewModels.DataTools
{
    [AutowireService(LifetimeScope.PerScope)]
    public class DataToolTagsViewModel : IDataToolTagsViewModel
    {
        protected readonly ITextTranslator TextTranslator;
        public IRenderingItemContext DataToolRenderingContext;

        public DataToolTagsViewModel(
            IRenderingItemContext dataToolRenderingContext,
             ITextTranslator textTranslator)
        {
            DataToolRenderingContext = dataToolRenderingContext;

            Tags = DataToolRenderingContext.Get<I___BaseTaxonomy>().Taxonomies.Take(3).Select(x => new LinkableModel
            {
                LinkableText = x.Item_Name,
                LinkableUrl = SearchTaxonomyUtil.GetSearchUrl(x)
            });
            TextTranslator = textTranslator;
        }

        public IEnumerable<ILinkable> Tags { get; set; }

        public string TagsLableText => TextTranslator.Translate("DataTools.TagsLable");
    }
}