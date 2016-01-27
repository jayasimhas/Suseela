using System.Linq.Expressions;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;

namespace Velir.Search.Core.Rules.Conditions
{
	public class NotConditionWrapper<T> : ConditionWrapper<T> where T : RuleContext
	{
		private ISearchCondition<T> Operand
		{
			get
			{
				return SearchConditionFactory.GetCondition<T>(((UnaryCondition<T>)(this.Condition as NotCondition<T>)).Operand);
			}
		}

		public NotConditionWrapper(NotCondition<T> p_Condition)
			: base((RuleCondition<T>)p_Condition)
		{
		}

		public override Expression GetQueryExpression(ConditionContext p_Context)
		{
			return (Expression)Expression.Not(this.Operand.GetQueryExpression(p_Context));
		}

		public override void ValidateCondition(ErrorReport p_Report)
		{
			this.Operand.ValidateCondition(p_Report);
		}
	}
}