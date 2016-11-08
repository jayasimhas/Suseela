using Glass.Mapper.Sc.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Library.JobsAndClassifieds
{
    public class JobTile : IJobTile
    {
        public Image JobLogo { get; set; }
        public string JobTitle { get; set; }
        public string JobShortDescription { get; set; }
        public DateTime JobPublishedDate { get; set; }
        public string JobDetailUrl { get; set; }
    }
}
