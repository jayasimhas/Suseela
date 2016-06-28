﻿using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.SharedSource.DataImporter.Providers;

namespace Sitecore.SharedSource.DataImporter.Mappings.Fields {

    /// <summary>
    /// this field uses the url stored in the field and converts it to a LinkField value
    /// </summary>
    public class UrlToLink : ToText {
        
        #region Properties

        #endregion Properties

        #region Constructor

        //constructor
        public UrlToLink(Item i)
            : base(i) {

        }

        #endregion Constructor

        #region IBaseField

        public override void FillField(IDataMap map, ref Item newItem, string importValue, string id = null) {

            if (string.IsNullOrEmpty(importValue))
                return;

            //get the field as a link field and store the url
            LinkField lf = newItem.Fields[NewItemField];
            if (lf != null)
                lf.Url = importValue;
        }

        #endregion IBaseField
    }
}
