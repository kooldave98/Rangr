using System;
using App.Core.Portable.Models;

namespace App.Core.Portable.Device
{
	public interface ISession
	{
		//User
		User GetCurrentUser ();
		void PersistCurrentUser (User user);
	}
}

