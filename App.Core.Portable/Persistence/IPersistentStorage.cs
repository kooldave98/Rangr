using System;

namespace App.Core.Portable.Persistence
{
	public interface IPersistentStorage
	{
		bool Clear(string key);


		bool Save(string key, object value);


		T Load<T>(string key);
	}
}

