using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Library.DCD
{
    public partial class DealsCompaniesDrugsConfigurationItem : CustomItem
    {

        public static readonly string TemplateId = "{D71E4DC1-1F72-4846-AADB-832F160088E8}";


        #region Boilerplate CustomItem Code

        public DealsCompaniesDrugsConfigurationItem(Item innerItem) : base(innerItem)
        {

        }

        public static implicit operator DealsCompaniesDrugsConfigurationItem(Item innerItem)
        {
            return innerItem != null ? new DealsCompaniesDrugsConfigurationItem(innerItem) : null;
        }

        public static implicit operator Item(DealsCompaniesDrugsConfigurationItem customItem)
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
                //return new CustomTextField(InnerItem, InnerItem.Fields["Error Distribution List"]);
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
    }
}
