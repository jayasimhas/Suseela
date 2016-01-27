using Sitecore.Data;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;

namespace Velir.Search.Core.Rules.Conditions
{
	public class WhenTemplateIsWrapper<T> : ItemPropertyConditionWrapper<T, ID> where T : RuleContext
	{
		public WhenTemplateIsWrapper(Sitecore.Rules.Conditions.ItemConditions.WhenTemplateIs<T> p_Condition)
			: base((RuleCondition<T>)p_Condition)
		{
			this.PropertyName = "TemplateId";
			this.Value = p_Condition.TemplateId;
		}
	}
}