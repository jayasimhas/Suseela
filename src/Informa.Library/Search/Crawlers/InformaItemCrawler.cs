using System.Collections.Generic;
using System.Linq;
using Sitecore.ContentSearch;
using Sitecore.Diagnostics;

namespace Informa.Library.Search.Crawlers {
    public class InformaItemCrawler : SitecoreItemCrawler {

        private readonly List<string> excludeItemsList = new List<string>();
        public List<string> ExcludeItemsList
        {
            get
            {
                return excludeItemsList;
            }
        }

        protected override bool IsExcludedFromIndex(SitecoreIndexableItem indexable, bool checkLocation = false) {
            Assert.ArgumentNotNull(indexable, "item");
            if (ExcludeItemsList.Any(path => indexable.AbsolutePath.ToLower().StartsWith(path.ToLower()))) {
                return true;
            }
            return base.IsExcludedFromIndex(indexable, checkLocation);
        }
    }
}