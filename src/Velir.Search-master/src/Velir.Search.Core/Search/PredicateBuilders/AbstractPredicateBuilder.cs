using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data;
using Sitecore.Rules;
using Velir.Search.Core.Rules;
using Velir.Search.Core.Rules.Parser;

namespace Velir.Search.Core.Search.PredicateBuilders
{
	public abstract class AbstractPredicateBuilder<TResult> : IPredicateBuilder<TResult> where TResult : SearchResultItem
	{
		protected ISearchRuleParser RuleParser { get; set; }

		public IList<ID> ValidTemplates { get; protected set; }
		public IList<ID> ExcludedItems { get; protected set; }
		public string Location { get; set; }
		public IEnumerable<ISearchCondition<RuleContext>> Conditions { get; protected set; }

		protected AbstractPredicateBuilder(ISearchRuleParser parser)
		{
			RuleParser = parser;

			ValidTemplates = new List<ID>();
			ExcludedItems = new List<ID>();
			Location = string.Empty;
			Conditions = new List<ISearchCondition<RuleContext>>();
		}

		public void AddRules(string hiddenExpression)
		{
			Conditions = RuleParser.GetSearchConditions<RuleContext>(hiddenExpression);
		}

		public virtual Expression<Func<TResult, bool>> Build()
		{
			var ruleExpression = RuleParser.GetQueryExpression<TResult, RuleContext>(Conditions);

			var predicate = ruleExpression ?? PredicateBuilder.True<TResult>();
			
			if (ValidTemplates.Any())
			{
				var orPredicate = PredicateBuilder.True<TResult>();
				orPredicate = ValidTemplates.Aggregate(orPredicate, (current, templateId) => current.Or(item => item.TemplateId == templateId));

				predicate = predicate.And(orPredicate);
			}

			predicate = ExcludedItems.Aggregate(predicate, (current, excludedId) => current.And(item => item.ItemId != excludedId));

			if (!string.IsNullOrEmpty(Location))
			{
				predicate = predicate.And(item => item.Path.StartsWith(Location));
			}

			return predicate;
		}
	}
}
