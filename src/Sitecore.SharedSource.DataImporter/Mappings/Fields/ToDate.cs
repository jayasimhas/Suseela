using System;
using System.Globalization;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.SharedSource.DataImporter.Extensions;
using Sitecore.SharedSource.DataImporter.Providers;
using Sitecore.SharedSource.DataImporter.Utility;

namespace Sitecore.SharedSource.DataImporter.Mappings.Fields {

    /// <summary>
    /// This field converts a date value to a sitecore date field value
    /// </summary>
    public class ToDate : ToText {

        #region Properties

        #endregion Properties

        #region Constructor

        //constructor
        public ToDate(Item i)
            : base(i) {

        }

        #endregion Constructor

        #region IBaseField

        public override void FillField(IDataMap map, ref Item newItem, string importValue, string id = null) {
            if (string.IsNullOrEmpty(importValue))
                return;

            //try to parse date value 
            DateTime date;
            if (!DateTimeUtil.ParseInformaDate(importValue, out date))  {
                map.Logger.Log(newItem.Paths.FullPath, "Date parse error", ProcessStatus.DateParseError, ItemName(), importValue);
                return;
            }
            
            Field f = newItem.Fields[NewItemField];
            if (f == null)
                return;

            f.Value = date.ToDateFieldValue();
        }

        #endregion IBaseField
    }
}
