using Sitecore.Rules;
using Sitecore.Rules.Conditions.FieldConditions;
using Sitecore.Rules.Conditions.ItemConditions;

namespace Velir.Search.Core.Rules.Conditions
{
	public class ItemIdConditionWrapper<T> : WhenFieldWrapper<T> where T : RuleContext
	{
		public ItemIdConditionWrapper(ItemIdCondition<T> p_Condition) : base(new WhenField<T>() {FieldName = "id", OperatorId = p_Condition.OperatorId, Value = p_Condition.Value })
		{
      
		}
	}
}