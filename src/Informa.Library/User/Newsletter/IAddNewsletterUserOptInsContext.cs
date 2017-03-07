using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Library.User.Newsletter
{
    public interface IAddNewsletterUserOptInsContext
    {
        bool Add(IEnumerable<INewsletterUserOptIn> optIns);
    }
}
