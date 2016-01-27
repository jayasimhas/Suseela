using System.Linq.Expressions;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;

namespace Velir.Search.Core.Rules.Conditions
{
	public class NotSupportedConditionWrapper<T> : ConditionWrapper<T> where T : RuleContext
	{
		public NotSupportedConditionWrapper(RuleCondition<T> p_Condition)
			: base(p_Condition)
		{
		}

		public override Expression GetQueryExpression(ConditionContext p_Context)
		{
			return (Expression)Expression.Constant((object)false);
		}

		public override void ValidateCondition(ErrorReport p_Report)
		{
			p_Report.AddError("This condition is not supported.");
		}
	}
}