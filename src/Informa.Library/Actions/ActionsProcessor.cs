using System.Collections.Generic;
using System.Linq;

namespace Informa.Library.Actions
{
	public abstract class ActionsProcessor<TAction, TValue> : IActionsProcessor<TAction, TValue>
		where TAction : IAction<TValue>
	{
		protected readonly IEnumerable<TAction> Actions;

		public ActionsProcessor(
			IEnumerable<TAction> actions)
		{
			Actions = actions;
		}

		public virtual void Process(TValue value)
		{
			Actions.ToList().ForEach(a => a.Process(value));
		}
	}
}
