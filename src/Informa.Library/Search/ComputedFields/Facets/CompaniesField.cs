﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Data.Items;
using Velir.Search.Core.ComputedFields;

namespace Informa.Library.Search.ComputedFields.Facets
{
    public class CompaniesField : BaseContentComputedField
    {
        public override object GetFieldValue(Item indexItem)
        {
            return string.Empty;
        }
    }
}
