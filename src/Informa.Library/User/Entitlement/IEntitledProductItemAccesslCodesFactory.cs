using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Library.User.Entitlement
{
    public interface IEntitledProductItemAccesslCodesFactory
    {
        IList<string> Create(IArticle item);
    }
}
