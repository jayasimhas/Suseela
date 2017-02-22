using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Library.Logging
{
    public interface IInfoLogger
    {
        void Log(string message,string owner);

    }
}
