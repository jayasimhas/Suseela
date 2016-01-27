using System;
using System.Linq.Expressions;
using System.Reflection;
using log4net;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;

namespace Velir.Search.Core.Rules.Conditions
{
	public abstract class FieldOperatorCondition<TContext, TValue> : WhenCondition<TContext>, ISearchCondition<TContext> where TContext : RuleContext
	{
		private static readonly ILog s_Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public string FieldName { get; set; }

		public string OperatorId { get; set; }

		public TValue Value { get; set; }

		protected FieldOperatorCondition()
		{
      
		}

		public virtual Expression GetQueryExpression(ConditionContext p_Context)
		{
			Expression expression;
			switch (this.OperatorId)
			{
				case "{066602E2-ED1D-44C2-A698-7ED27FD3A2CC}":
					expression = this.BuildBinaryExpression(new Func<Expression, Expression, Expression>(Expression.Equal), p_Context);
					break;
				case "{B88CD556-082E-4385-BB76-E4D1B565F290}":
					expression = this.BuildBinaryExpression(new Func<Expression, Expression, Expression>(Expression.GreaterThan), p_Context);
					break;
				case "{814EF7D0-1639-44FD-AEEF-735B5AC14425}":
					expression = this.BuildBinaryExpression(new Func<Expression, Expression, Expression>(Expression.GreaterThanOrEqual), p_Context);
					break;
				case "{E362A3A4-E230-4A40-A7C4-FC42767E908F}":
					expression = this.BuildBinaryExpression(new Func<Expression, Expression, Expression>(Expression.LessThan), p_Context);
					break;
				case "{2E1FC840-5919-4C66-8182-A33A1039EDBF}":
					expression = this.BuildBinaryExpression(new Func<Expression, Expression, Expression>(Expression.LessThanOrEqual), p_Context);
					break;
				case "{3627ED99-F454-4B83-841A-A0194F0FB8B4}":
					expression = this.BuildBinaryExpression(new Func<Expression, Expression, Expression>(Expression.NotEqual), p_Context);
					break;
				default:
					FieldOperatorCondition<TContext, TValue>.s_Logger.Debug(string.Format("Unknown operator id \"{0}\". Returning true expression.", (object) this.OperatorId));
					expression = (Expression) Expression.Constant((object) true);
					break;
			}
			return expression;
		}

		public void ValidateCondition(ErrorReport p_Report)
		{
		}

		public RuleCondition<TContext> GetWrappedCondition()
		{
			return (RuleCondition<TContext>) this;
		}

		protected abstract Expression GetTypedFieldValueExpression(ConditionContext p_Context);

		protected virtual Expression GetTypedValueExpression(ConditionContext p_Context)
		{
			return (Expression) Expression.Constant((object) this.Value);
		}

		protected Expression BuildBinaryExpression(Func<Expression, Expression, Expression> p_OperatorMethod, ConditionContext p_Context)
		{
			Expression fieldValueExpression = this.GetTypedFieldValueExpression(p_Context);
			Expression typedValueExpression = this.GetTypedValueExpression(p_Context);
			return p_OperatorMethod(fieldValueExpression, typedValueExpression);
		}
	}
}