using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Elsevier.Web.VWB.Report.Columns
{
    public class AuthorsColumn : IVwbColumn
    {
        public int Compare(ArticleItemWrapper x, ArticleItemWrapper y)
        {
            string a = string.Join("", x.Authors.ToArray());
            string b = string.Join("", y.Authors.ToArray());
            return a.CompareTo(b);
        }

        public string GetHeader()
        {
            return "Author(s)";
        }

        public string Key()
        {
            return "ath";
        }

        public Dictionary<string, string> GetDropDownValues(List<ArticleItemWrapper> results)
        {

            Dictionary<string, string> dictAuthorNames = new Dictionary<string, string>();
            if (results != null)
            {
                dictAuthorNames.Add("0", "Select");
                foreach (var result in results)
                {
                    if (result != null && result.Authors.Any())
                    {
                        AddAuthorNameToList(dictAuthorNames, result.Authors);
                    }
                }
            }
            return dictAuthorNames.OrderBy(k=>k.Key).ToDictionary(m=>m.Key,n=>n.Value);
        }

        private void AddAuthorNameToList(Dictionary<string, string> dictAuthorNames, IEnumerable<string> authors)
        {
            if (authors != null && authors.Count() > 1)
            {
                foreach (var author in authors)
                {
                    if (!dictAuthorNames.ContainsValue(author))
                        dictAuthorNames.Add(author, author);
                    else
                        continue;
                }
            }
            else
            {
                if (!dictAuthorNames.ContainsValue(authors.FirstOrDefault()))
                    dictAuthorNames.Add(authors.FirstOrDefault(), authors.FirstOrDefault());
            }
        }

        public TableCell GetCell(ArticleItemWrapper articleItemWrapper)
        {
            string authors = "";
            if (articleItemWrapper.Authors != null)
            {
                authors = string.Join("<br />", articleItemWrapper.Authors.ToArray());

            }

            return new TableCell { Text = authors };
        }
    }
}