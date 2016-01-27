using System.Collections.Generic;
using System.Linq;
using Sitecore.Diagnostics;
using Sitecore.Rules;
using Sitecore.Rules.Actions;

namespace Velir.Search.Core.Rules
{
	public class SearchRule<T> where T : RuleContext
	{
		public string ID { get; private set; }

		public string Name { get; private set; }

		public IEnumerable<RuleAction<T>> Actions { get; private set; }

		public ISearchCondition<T> Condition { get; private set; }

		public SearchRule(string id, string name, ISearchCondition<T> condition, IEnumerable<RuleAction<T>> actions)
		{
			Assert.ArgumentNotNullOrEmpty(id, "p_Id");
			this.ID = id;
			this.Name = name;
			this.Condition = condition;
			if (actions != null && actions.Any())
				this.Actions = new List<RuleAction<T>>(actions);
			else
				this.Actions = new RuleAction<T>[0];
		}

		public SearchRule(string id, string name, ISearchCondition<T> condition)
			: this(id, name, condition, null)
		{
		}
	}
}
