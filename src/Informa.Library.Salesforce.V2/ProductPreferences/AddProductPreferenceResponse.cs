using System.Collections.Generic;

namespace Informa.Library.Salesforce.V2.ProductPreferences
{
    public class AddProductPreferenceResponse
    {
        public bool hasErrors { get; set; }
        public List<AddProductPreferenceResultItem> results { get; set; }
    }
}
