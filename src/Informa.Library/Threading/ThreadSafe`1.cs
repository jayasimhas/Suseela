namespace Informa.Library.Threading
{
	public abstract class ThreadSafe<T>
	{
		private T safeObject;
		private object locker = new object();

		public T SafeObject
		{
			get
			{
				if (safeObject != null)
				{
					return safeObject;
				}

				lock(locker)
				{
					if (safeObject != null)
					{
						return safeObject;
					}

					return safeObject = UnsafeObject;
				}
			}
		}

		protected abstract T UnsafeObject { get; }

		public void Reload()
		{
			lock(locker)
			{
				safeObject = UnsafeObject;
			}
		}
	}
}
