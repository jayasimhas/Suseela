using System.Linq.Expressions;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;

namespace Velir.Search.Core.Rules.Conditions
{
	public class OrConditionWrapper<T> : ConditionWrapper<T> where T : RuleContext
	{
		private ISearchCondition<T> LeftOperand
		{
			get
			{
				return SearchConditionFactory.GetCondition<T>(((BinaryCondition<T>)(this.Condition as OrCondition<T>)).LeftOperand);
			}
		}

		private ISearchCondition<T> RightOperand
		{
			get
			{
				return SearchConditionFactory.GetCondition<T>(((BinaryCondition<T>)(this.Condition as OrCondition<T>)).RightOperand);
			}
		}

		public OrConditionWrapper(OrCondition<T> p_Condition)
			: base((RuleCondition<T>)p_Condition)
		{
		}

		public OrConditionWrapper(RuleCondition<T> p_LeftOperand, RuleCondition<T> p_RightOperand)
			: base((RuleCondition<T>)new OrCondition<T>(p_LeftOperand, p_RightOperand))
		{
		}

		public override Expression GetQueryExpression(ConditionContext p_Context)
		{
			return (Expression)Expression.OrElse(this.LeftOperand.GetQueryExpression(p_Context), this.RightOperand.GetQueryExpression(p_Context));
		}

		public override void ValidateCondition(ErrorReport p_Report)
		{
			this.LeftOperand.ValidateCondition(p_Report);
			this.RightOperand.ValidateCondition(p_Report);
		}
	}
}