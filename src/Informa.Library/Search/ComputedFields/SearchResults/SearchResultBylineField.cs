using System;
using System.Linq;
using System.Text;
using Autofac;
using FuelSDK;
using Informa.Library.Authors;
using Informa.Library.Services.Search.Fields.Base;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Autofac.Attributes;
using Jabberwocky.Glass.Autofac.Util;

namespace Informa.Library.Search.ComputedFields.SearchResults
{
    public class SearchResultBylineField : BaseGlassComputedField<IArticle>
    {
        public override object GetFieldValue(IArticle indexItem)
        {
            if (indexItem.Authors == null)
            {
                return string.Empty;
            }

            var authorCount = indexItem.Authors.Count();
            if (authorCount == 0)
            {
                return string.Empty;
            }

            var authorList = new StringBuilder();

            if (!indexItem.Authors.Any()) return string.Empty;
            var currentCount = 1;
            using (var scope = AutofacConfig.ServiceLocator.BeginLifetimeScope())
            {
                var authorClient = scope.Resolve<IAuthorService>();
                foreach (var eachAuthor in indexItem.Authors)
                {
                    if (eachAuthor.Inactive)
                    {
                        authorList.Append(eachAuthor.First_Name + eachAuthor.Last_Name);
                    }
                    else
                    {
	                    var urlName = authorClient.GetUrlName(eachAuthor._Id);
                        authorList.Append($"<a href='/authors/{urlName}'>{eachAuthor.First_Name} {eachAuthor.Last_Name}</a>");
                    }

                    if (currentCount < authorCount - 1)
                    {
                        authorList.Append(", ");
                    }
                    else if (currentCount == authorCount - 1)
                    {
                        authorList.Append(" and ");
                    }
                    currentCount++;
                }
            }

            return authorList;
        }
    }
}
