using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Data.Items;
using Jabberwocky.Autofac.Attributes;
using Glass.Mapper.Sc;

namespace Informa.Library.Utilities.References
{
    [AutowireService(LifetimeScope.PerScope)]
    public class TaxonomyFolders : ITaxonomyFolders
    {
        ISitecoreService _sitecoreService;
        public TaxonomyFolders(ISitecoreService sitecoreService)
        {
            _sitecoreService = sitecoreService;

            var mainTaxonomyFolder = _sitecoreService.GetItem<Item>(ItemReferences.Instance.PharmaTaxonomyRootFolder);
            _taxonomyFolders = mainTaxonomyFolder.Children;
        }

        IEnumerable<Item> _taxonomyFolders;
        IEnumerable<Item> ITaxonomyFolders.TaxonomyFolders
        {
            get
            {
                return _taxonomyFolders;
            }
        }
    }
}
