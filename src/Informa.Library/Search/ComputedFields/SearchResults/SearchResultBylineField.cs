using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Sc;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Sitecore.Data.Items;
using Velir.Search.Core.ComputedFields;

namespace Informa.Library.Search.ComputedFields.SearchResults
{
    public class SearchResultBylineField : BaseContentComputedField
    {
        public override object GetFieldValue(Item indexItem)
        {
            if (indexItem.TemplateID != IArticleConstants.TemplateId)
            {
                return string.Empty;
            }

            IArticle article = indexItem.GlassCast<IArticle>(inferType: true);

            if (!article.Authors.Any())
            {
                return string.Empty;
            }

            //TamerM - 2016-04-03: comma separate except the last one and replace with 'and'
            return string.Join(", ", article.Authors?.Take(article.Authors.Count() - 1).Select(x => x.First_Name + " " + x.Last_Name)) + ((article.Authors.Count() > 1 ? " and " : "") + article.Authors.Select(x => x.First_Name + " " + x.Last_Name).LastOrDefault());

            //StringBuilder byLine = new StringBuilder();

            //string sep = "";

            //foreach (IStaff_Item author in article.Authors)
            //{
            //    byLine.Append(sep);
            //    byLine.Append(author.First_Name + " " + author.Last_Name);
            //    sep = ", ";
            //}

            //return byLine.ToString();
        }
    }
}
