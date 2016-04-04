using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace Informa.Library.Rss.Utils
{
    public class RssConstants
    {
        public static readonly XNamespace AtomNamespace = "http://www.w3.org/2005/Atom";

        //RSS Field Names
        public static readonly string FieldEmailPriority = "e-mail_priority";
        public static readonly string FieldMediaIcon = "media_icon";
        public static readonly string FieldId = "id";
        public static readonly string FieldPubDate = "pubDate";
        public static string FieldTaxonomyItems = "taxonomy_items";
        public static string FieldTaxonomyItem = "taxonomy_item";

        //Date Format
        public static readonly string DateFormat = "r";
        
    }
}
