using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Glass.Mapper.Sc;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Rules;

namespace Velir.Search.Core.Rules.Parser
{
	public class SearchRuleParser : ISearchRuleParser
	{
		private readonly ISitecoreService _service;

		public SearchRuleParser(ISitecoreService service)
		{
			_service = service;
		}

		public IEnumerable<ISearchCondition<T>> GetSearchConditions<T>(string ruleSet) where T : RuleContext
		{
			return _service != null && ruleSet != null ? SearchRuleFactory<T>.ParseRules(_service, ruleSet).Select(r => r.Condition) : new List<ISearchCondition<T>>();
		}

		public Expression<Func<T, bool>> GetQueryExpression<T, TK>(IEnumerable<ISearchCondition<TK>> conditions) where T : SearchResultItem where TK : RuleContext
		{
			Expression<Func<T, bool>> predicate = null;

			if (conditions.Any())
			{
				ParameterExpression expression = Expression.Parameter(typeof(T), "item");
				ConditionContext context = new ConditionContext(expression);
				foreach (ISearchCondition<TK> condition in conditions)
				{
					Expression ruleExpression = condition.GetQueryExpression(context);
					predicate = Expression.Lambda<Func<T, bool>>(ruleExpression, new[] { expression });
				}
			}

			return predicate;
		}
	}
}
