using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Sitecore.ContentSearch.Utilities;
using Sitecore.Data;
using Sitecore.Diagnostics;
using Sitecore.Rules;
using Sitecore.Rules.Conditions.FieldConditions;

namespace Velir.Search.Core.Rules.Conditions
{
	public class WhenFieldWrapper<T> : StringConditionWrapper<T, WhenField<T>> where T : RuleContext
	{
		public WhenFieldWrapper(WhenField<T> p_Condition)
			: base(p_Condition)
		{
		}

		protected override Expression GetFieldExpression(ParameterExpression p_Instance, WhenField<T> p_Condition)
		{
			return GetFieldIndexerExpression(p_Instance, p_Condition.FieldName);
		}

		protected override Expression GetConstantExpression(ParameterExpression p_Instance, WhenField<T> p_Condition)
		{
			string value = p_Condition.Value;
			if (ID.IsID(p_Condition.Value))
			{
				value = IdHelper.NormalizeGuid(p_Condition.Value);
			}

			return (Expression)Expression.Constant((object)value);
		}

		internal static Expression GetFieldIndexerExpression(ParameterExpression p_Instance, string p_FieldName)
		{
			string name = "Item";
			object[] customAttributes = p_Instance.Type.GetCustomAttributes(typeof(DefaultMemberAttribute), true);
			if (Enumerable.Any<object>((IEnumerable<object>)customAttributes))
				name = (customAttributes[0] as DefaultMemberAttribute).MemberName;
			PropertyInfo property = p_Instance.Type.GetProperty(name, new Type[1]
      {
        typeof (string)
      });
			Assert.IsNotNull((object)property, "Could not find the proper indexer. Expecting: string this[string]");
			MethodInfo getMethod = property.GetMethod;
			return (Expression)Expression.Call((Expression)p_Instance, getMethod, new Expression[1]
      {
        (Expression) Expression.Constant((object) p_FieldName)
      });
		}
	}
}