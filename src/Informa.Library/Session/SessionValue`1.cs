namespace Informa.Library.Session
{
	public class SessionValue<T> : ISessionValue<T>
	{
		public bool HasValue { get; set; }
		public T Value { get; set; }
	}
}
