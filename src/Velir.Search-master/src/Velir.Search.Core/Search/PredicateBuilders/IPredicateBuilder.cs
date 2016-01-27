using System;
using System.Linq.Expressions;
using Sitecore.ContentSearch.SearchTypes;

namespace Velir.Search.Core.Search.PredicateBuilders
{
    public interface IPredicateBuilder<T> where T : SearchResultItem
    {
        Expression<Func<T, bool>> Build();
    }
}
