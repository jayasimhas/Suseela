﻿using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Sitecore.ContentSearch;
using System;
using System.Collections.Generic;

namespace Informa.Library.Search.Results
{
    public interface ITaxonomySearchResults
    {
        //[IndexField(I___BaseTaxonomyConstants.TaxonomiesFieldName)]
        [IndexField("taxonomylist_sm")]
        List<Guid> Taxonomies { get; set; }

        [IndexField("content_type_s")]
        Guid ContentTypeTaxonomy { get; set; }

        [IndexField("media_type_s")]
        Guid MediaTypeTaxonomy { get; set; }
    }
}
