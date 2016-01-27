using System.Linq.Expressions;
using Sitecore.Diagnostics;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;

namespace Velir.Search.Core.Rules.Conditions
{
	public class ConditionWrapper<T> : ISearchCondition<T> where T : RuleContext
	{
		public RuleCondition<T> Condition { get; private set; }

		public ConditionWrapper(RuleCondition<T> p_Condition)
		{
			Assert.ArgumentNotNull((object)p_Condition, "p_Condition");
			this.Condition = p_Condition;
		}

		public virtual Expression GetQueryExpression(ConditionContext p_Context)
		{
			Assert.ArgumentNotNull((object)p_Context, "p_Context");
			return (Expression)null;
		}

		public virtual void ValidateCondition(ErrorReport p_Report)
		{
		}

		public RuleCondition<T> GetWrappedCondition()
		{
			return this.Condition;
		}
	}
}