using System;
using App.Core.Portable.Models;

namespace App.Core.Portable.Device
{
	public interface ISession
	{
		//User
		User GetCurrentUser ();
		void PersistCurrentUser (User user);

		//No need to persist this, will just keep in memory
		ConnectionIdentity CurrentConnection { get; set;}
	}
}

