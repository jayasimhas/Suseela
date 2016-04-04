using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using Glass.Mapper.Sc;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Sitecore.Data.Items;

namespace Informa.Library.Rss.Utils
{
    public class RssItemUtil
    {
        protected ISitecoreContext _sitecoreContext;

        public RssItemUtil(ISitecoreContext sitecoreContext)
        {
            _sitecoreContext = sitecoreContext;

        }

   
       

        

    }
}
