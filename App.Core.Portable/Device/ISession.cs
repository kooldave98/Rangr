using System;
using App.Core.Portable.Models;

namespace App.Core.Portable.Device
{
	public interface ISession
	{
		//User
		User GetCurrentUser (bool allowDefault = false);
		void PersistCurrentUser (User user);

		//Will persist this, as this will rarely change
		void PersistCurrentConnection (ConnectionIdentity connection_id);

		ConnectionIdentity GetCurrentConnection(bool allowDefault = false);
	}
}

