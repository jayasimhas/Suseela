namespace Informa.Library.Actions
{
	public interface IAction<T>
	{
		void Process(T value);
	}
}
