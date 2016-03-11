namespace Informa.Library.Actions
{
	public interface IActionsProcessor<TAction, TValue>
		where TAction : IAction<TValue>
	{
		void Process(TValue value);
	}
}
