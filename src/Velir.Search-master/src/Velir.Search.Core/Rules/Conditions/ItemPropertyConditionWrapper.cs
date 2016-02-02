using System.Linq.Expressions;
using System.Reflection;
using Sitecore.Diagnostics;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;

namespace Velir.Search.Core.Rules.Conditions
{
	public abstract class ItemPropertyConditionWrapper<TContext, TProperty> : ConditionWrapper<TContext> where TContext : RuleContext
	{
		public string PropertyName { get; set; }

		public TProperty Value { get; set; }

		public ItemPropertyConditionWrapper(RuleCondition<TContext> p_Condition)
			: base(p_Condition)
		{
		}

		public override Expression GetQueryExpression(ConditionContext p_Context)
		{
			return (Expression)Expression.Equal(this.GetPropertyExpression(p_Context.ParameterExpression, this.PropertyName), (Expression)Expression.Constant((object)this.Value));
		}

		protected virtual Expression GetPropertyExpression(ParameterExpression p_Instance, string p_PropertyName)
		{
			Assert.ArgumentNotNull((object)p_Instance, "p_Instance");
			Assert.ArgumentNotNullOrEmpty(p_PropertyName, "p_PropertyName");
			PropertyInfo property = p_Instance.Type.GetProperty(this.PropertyName);
			return (Expression)Expression.Property((Expression)p_Instance, property);
		}
	}
}