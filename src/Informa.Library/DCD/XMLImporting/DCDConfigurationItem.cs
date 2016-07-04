using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Library.DCD.XMLImporting
{
    public class DCDConfigurationItem : CustomItem
    {
        #region Boilerplate CustomItem Code

        public DCDConfigurationItem(Item innerItem) : base(innerItem)
        {

        }

        public static implicit operator DCDConfigurationItem(Item innerItem)
        {
            return innerItem != null ? new DCDConfigurationItem(innerItem) : null;
        }

        public static implicit operator Item(DCDConfigurationItem customItem)
        {
            return customItem != null ? customItem.InnerItem : null;
        }

        #endregion //Boilerplate CustomItem Code

        #region Field Instance Methods

        public TextField ErrorDistributionList
        {
            get
            {
                return InnerItem.Fields["Error Distribution List"];
            }
        }

        public static string FieldName_ErrorDistributionList
        {
            get
            {
                return "Error Distribution List";
            }
        }

        #endregion //Field Instance Methods

        public List<string> GetEmailDistributionList()
        {
            //This is called in a weird context, so the FieldRenderer throws a null exception
            //Use the raw value instead
            return InnerItem.Fields["Error Distribution List"].GetValue(true).Split(',').Select(s => s.Trim()).ToList();
        }
    }
}
