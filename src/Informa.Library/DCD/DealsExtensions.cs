using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Models.DealsCompaniesDrugs
{
    public static class DealsExtensions
    {
        public static string GetUrl(this Deal deal)
        {
            return "https://www.google.com";
            //if (!string.IsNullOrEmpty(recordNumber))
            //{
                
            //    return ItemReference.Deals.URL + "/" + recordNumber;
            //}
            return string.Empty;
        }
    }
}
