using System.Linq.Expressions;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;

namespace Velir.Search.Core.Rules
{
	public interface ISearchCondition<TContext> where TContext : RuleContext
	{
		Expression GetQueryExpression(ConditionContext p_Context);

		void ValidateCondition(ErrorReport p_Report);

		RuleCondition<TContext> GetWrappedCondition();
	}
}
