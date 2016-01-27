using System;
using System.Linq.Expressions;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data.Fields;
using Sitecore.Rules;

namespace Velir.Search.Core.Rules.Conditions
{
	public class MultilistFieldContainsCondition<T> : FieldOperatorCondition<T, Guid> where T : RuleContext
	{
		protected override bool Execute(T ruleContext)
		{
			MultilistField field = ruleContext.Item.Fields[FieldName];

			return field.Contains(Value.ToString());
		}

		public override Expression GetQueryExpression(ConditionContext p_Context)
		{
			return Expression.Call(this.GetTypedFieldValueExpression(p_Context), "Contains", (Type[])null, new Expression[1]
			{
				this.GetTypedValueExpression(p_Context)
			});
		}

		protected override Expression GetTypedFieldValueExpression(ConditionContext p_Context)
		{
			return Expression.Property(p_Context.ParameterExpression, typeof(SearchResultItem), FieldName);
		}
	}
}
