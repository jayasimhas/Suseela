using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;

namespace Informa.Library.User.Entitlement
{
    public class ScripEntitlement : Entitlement
    {
        public ScripEntitlement(IEntitledProductItem entitledItem)
        {
            ProductCode = "Scrip";
        }
    }

    public class Entitlement : IEntitlement
    {
        #region Implementation of IEntitlement

        public string ProductCode { get; set; }

        #endregion
    }
}