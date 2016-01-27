using System;
using System.Linq.Expressions;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data.Fields;
using Sitecore.Rules;

namespace Velir.Search.Core.Rules.Conditions
{
	public class FieldEqualsCondition<T>: FieldOperatorCondition<T, Guid> where T : RuleContext
	{
		protected override bool Execute(T ruleContext)
		{
			Field field = ruleContext.Item.Fields[FieldName];

			return field.Value.Equals(Value.ToString());
		}

		protected override Expression GetTypedFieldValueExpression(ConditionContext p_Context)
		{
			return Expression.Property(p_Context.ParameterExpression, typeof(SearchResultItem), FieldName);
		}
	}
}
