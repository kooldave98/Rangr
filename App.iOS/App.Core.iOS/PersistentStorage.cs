using App.Core.Portable.Persistence;
namespace App.iOS
{
	public class PersistentStorage : IPersistentStorage
	{
		private static IPersistentStorage _instance = null;

		public static IPersistentStorage GetInstance()
		{
			return _instance ?? (_instance = new PersistentStorage ());
		}

		private PersistentStorage()
		{

		}

		public bool Clear(string key)
		{
			if (null == key)
				return false;

			return true;
		}

		public bool Save(string key, object value)
		{
			if (null == value)
				return false;


			return true;
		}

		public T Load<T>(string key)
		{
			object ret = new{};
			if (false)
				return default(T);

			return (T)ret;//store[key];
		}
	}
}