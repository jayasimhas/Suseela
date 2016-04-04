namespace Informa.Library.Session
{
	public interface ISessionValue<T>
	{
		bool HasValue { get; }
		T Value { get; }
	}
}