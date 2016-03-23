using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;

namespace Informa.Library.User.Entitlement
{
    public class Entitlement : IEntitlement
    {
        #region Implementation of IEntitlement

        public string ProductCode { get; set; }

        #endregion
    }
}