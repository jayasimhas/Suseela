//using Informa.Library.Presentation;
//using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
//using Jabberwocky.Glass.Autofac.Attributes;
//using Jabberwocky.Glass.Autofac.Mvc.Models;

//namespace Informa.Web.ViewModels
//{
//    [AutowireService(LifetimeScope.SingleInstance)]
//    public class TaxonomyHierarchyViewModel : GlassViewModel<I___BaseTaxonomy>
//    {
//        public IRenderingItemContext ArticleRenderingContext { get; set; }

//        public TaxonomyHierarchyViewModel(IRenderingItemContext articleRenderingContext)
//        {                                 
//            ArticleRenderingContext = articleRenderingContext;
//        }

//        //public IEnumerable<IHierarchyLinks> Tags
//        //    => ArticleRenderingContext.Get<I___BaseTaxonomy>().Taxonomies.Select(x => GetHierarchyLinks(x));

//        //private HierarchyLinks GetHierarchyLinks(ITaxonomy_Item taxonomyItem)
//        //{
//        //    var taxonomyRootFolder = new Guid("{E8A37C2D-FFE3-42D4-B38E-164584743832}");

//        //    taxonomyItem._Parent 
//        //}
//    }
//}