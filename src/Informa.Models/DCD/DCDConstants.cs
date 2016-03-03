using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Models.DCD
{
    public class DCDConstants
    {
        public const string CompanyTokenFormat = "[C#{0}:{1}]";

        public const string DealTokenRegex = @"\[W#(.*?)\]";
        public const string CompanyTokenRegex = @"\[C#(.*?)\]";
    }
}
