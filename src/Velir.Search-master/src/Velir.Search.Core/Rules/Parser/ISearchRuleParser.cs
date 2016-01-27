using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Rules;

namespace Velir.Search.Core.Rules.Parser
{
	public interface ISearchRuleParser
	{
		IEnumerable<ISearchCondition<T>> GetSearchConditions<T>(string ruleSet) where T : RuleContext;

		Expression<Func<T, bool>> GetQueryExpression<T, TK>(IEnumerable<ISearchCondition<TK>> conditions) where T : SearchResultItem where TK : RuleContext;
	}
}
