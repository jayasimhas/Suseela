using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using System.Collections.Generic;

namespace Informa.Library.User.Entitlement
{
    public interface IEntitledProductItemChannelCodesFactory
    {
        IList<string> Create(IArticle item);
    }
}
