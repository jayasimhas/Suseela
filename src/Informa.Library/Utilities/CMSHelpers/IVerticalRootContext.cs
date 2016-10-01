using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Library.Utilities.CMSHelpers
{
   public interface IVerticalRootContext
    {
        IVertical_Root Item { get; }
    }
}
