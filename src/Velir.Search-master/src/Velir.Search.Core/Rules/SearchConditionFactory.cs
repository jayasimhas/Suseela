using Sitecore.Rules;
using Sitecore.Rules.Conditions;
using Sitecore.Rules.Conditions.ItemConditions;
using Velir.Search.Core.Rules.Conditions;

namespace Velir.Search.Core.Rules
{
	public class SearchConditionFactory
	{
		public static ISearchCondition<T> GetCondition<T>(RuleCondition<T> p_Condition) where T : RuleContext
		{
			return new ConditionConverter<T>().Convert(p_Condition);
		}

		private class ConditionConverter<T> where T : RuleContext
		{
			public ISearchCondition<T> Convert(RuleCondition<T> p_Condition)
			{
				return !(p_Condition is ISearchCondition<T>) ? (!(p_Condition is Sitecore.Rules.Conditions.FieldConditions.WhenField<T>) ? (!(p_Condition is AndCondition<T>) ? (!(p_Condition is OrCondition<T>) ? (!(p_Condition is NotCondition<T>) ? (!(p_Condition is Sitecore.Rules.Conditions.ItemConditions.WhenTemplateIs<T>) ? (!(p_Condition is ItemIdCondition<T>) ? (ISearchCondition<T>)new NotSupportedConditionWrapper<T>(p_Condition) : (ISearchCondition<T>)new ItemIdConditionWrapper<T>((ItemIdCondition<T>)p_Condition)) : (ISearchCondition<T>)new WhenTemplateIsWrapper<T>((Sitecore.Rules.Conditions.ItemConditions.WhenTemplateIs<T>)p_Condition)) : (ISearchCondition<T>)new NotConditionWrapper<T>((NotCondition<T>)p_Condition)) : (ISearchCondition<T>)new OrConditionWrapper<T>((OrCondition<T>)p_Condition)) : (ISearchCondition<T>)new AndConditionWrapper<T>((AndCondition<T>)p_Condition)) : (ISearchCondition<T>)new WhenFieldWrapper<T>((Sitecore.Rules.Conditions.FieldConditions.WhenField<T>)p_Condition)) : (ISearchCondition<T>)p_Condition;
			}
		}
	}
}
