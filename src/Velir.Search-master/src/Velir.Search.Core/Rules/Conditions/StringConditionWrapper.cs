using System;
using System.Linq.Expressions;
using System.Reflection;
using log4net;
using Sitecore.ContentSearch.Linq;
using Sitecore.Diagnostics;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;

namespace Velir.Search.Core.Rules.Conditions
{
	public abstract class StringConditionWrapper<T, TCondition> : ConditionWrapper<T>
		where T : RuleContext
		where TCondition : StringOperatorCondition<T>
	{
		private static readonly ILog s_Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private TCondition m_Condition;

		public StringConditionWrapper(TCondition p_Condition)
			: base((RuleCondition<T>)(object)p_Condition)
		{
			this.m_Condition = p_Condition;
		}

		public override Expression GetQueryExpression(ConditionContext p_Context)
		{
			StringOperatorCondition<T> p_Condition = this.Condition as StringOperatorCondition<T>;
			Assert.IsNotNull((object)p_Condition, "Condition has the wrong type. Type \"{0}\" is expected.", new object[1]
			{
				(object) typeof (StringOperatorCondition<T>).FullName
			});
			Expression expression;
			switch (p_Condition.OperatorId)
			{
				case "{2E67477C-440C-4BCA-A358-3D29AED89F47}":
					expression = this.HandleContains(p_Condition, p_Context);
					break;
				case "{22E1F05F-A17A-4D0C-B376-6F7661500F03}":
					expression = this.HandleEndsWith(p_Condition, p_Context);
					break;
				case "{10537C58-1684-4CAB-B4C0-40C10907CE31}":
					expression = this.HandleEquals(p_Condition, p_Context);
					break;
				case "{537244C2-3A3F-4B81-A6ED-02AF494C0563}":
					expression = this.HandleEqualsCaseInsensitive(p_Condition, p_Context);
					break;
				case "{F8641C26-EE27-483C-9FEA-35529ECC8541}":
					expression = this.HandleMatchRegex(p_Condition, p_Context);
					break;
				case "{A6AC5A6B-F409-48B0-ACE7-C3E8C5EC6406}":
					expression = this.HandleNotEqual(p_Condition, p_Context);
					break;
				case "{6A7294DF-ECAE-4D5F-A8D2-C69CB1161C09}":
					expression = this.HandleNotEqualCaseInsensitive(p_Condition, p_Context);
					break;
				case "{FDD7C6B1-622A-4362-9CFF-DDE9866C68EA}":
					expression = this.HandleStartsWith(p_Condition, p_Context);
					break;
				default:
					StringConditionWrapper<T, TCondition>.s_Logger.Debug(string.Format("Unknown operator id \"{0}\". Returning true expression.", (object)p_Condition.OperatorId));
					expression = (Expression)Expression.Constant((object)true);
					break;
			}
			return expression;
		}

		protected abstract Expression GetFieldExpression(ParameterExpression p_Instance, TCondition p_Condition);

		protected abstract Expression GetConstantExpression(ParameterExpression p_Instance, TCondition p_Condition);

		private Expression HandleEquals(StringOperatorCondition<T> p_Condition, ConditionContext p_Context)
		{
			return (Expression)Expression.Equal(this.GetFieldExpression(p_Context.ParameterExpression, this.m_Condition), this.GetConstantExpression(p_Context.ParameterExpression, this.m_Condition));
		}

		private Expression HandleEqualsCaseInsensitive(StringOperatorCondition<T> p_Condition, ConditionContext p_Context)
		{
			return (Expression)Expression.Call(this.GetFieldExpression(p_Context.ParameterExpression, this.m_Condition), "Equals", (Type[])null, new Expression[2]
			{
				this.GetConstantExpression(p_Context.ParameterExpression, this.m_Condition),
				(Expression) Expression.Constant((object) StringComparison.OrdinalIgnoreCase)
			});
		}

		private Expression HandleContains(StringOperatorCondition<T> p_Condition, ConditionContext p_Context)
		{
			return (Expression)Expression.Call(this.GetFieldExpression(p_Context.ParameterExpression, this.m_Condition), "Contains", (Type[])null, new Expression[1]
			{
				this.GetConstantExpression(p_Context.ParameterExpression, this.m_Condition)
			});
		}

		private Expression HandleMatchRegex(StringOperatorCondition<T> p_Condition, ConditionContext p_Context)
		{
			return (Expression)Expression.Call((Expression)null, typeof(MethodExtensions).GetMethod("Matches", new Type[2]
			{
				typeof (string),
				typeof (string)
			}), this.GetFieldExpression(p_Context.ParameterExpression, this.m_Condition), this.GetConstantExpression(p_Context.ParameterExpression, this.m_Condition));
		}

		private Expression HandleNotEqual(StringOperatorCondition<T> p_Condition, ConditionContext p_Context)
		{
			return (Expression)Expression.NotEqual(this.GetFieldExpression(p_Context.ParameterExpression, this.m_Condition), this.GetConstantExpression(p_Context.ParameterExpression, this.m_Condition));
		}

		private Expression HandleNotEqualCaseInsensitive(StringOperatorCondition<T> p_Condition, ConditionContext p_Context)
		{
			return (Expression)Expression.IsFalse((Expression)Expression.Call(this.GetFieldExpression(p_Context.ParameterExpression, this.m_Condition), "Equals", (Type[])null, new Expression[2]
			{
				this.GetConstantExpression(p_Context.ParameterExpression, this.m_Condition),
				(Expression) Expression.Constant((object) StringComparison.OrdinalIgnoreCase)
			}));
		}

		private Expression HandleEndsWith(StringOperatorCondition<T> p_Condition, ConditionContext p_Context)
		{
			return (Expression)Expression.Call(this.GetFieldExpression(p_Context.ParameterExpression, this.m_Condition), "EndsWith", (Type[])null, new Expression[1]
			{
				this.GetConstantExpression(p_Context.ParameterExpression, this.m_Condition)
			});
		}

		private Expression HandleStartsWith(StringOperatorCondition<T> p_Condition, ConditionContext p_Context)
		{
			return (Expression)Expression.Call(this.GetFieldExpression(p_Context.ParameterExpression, this.m_Condition), "StartsWith", (Type[])null, new Expression[1]
			{
				this.GetConstantExpression(p_Context.ParameterExpression, this.m_Condition)
			});
		}
	}
}