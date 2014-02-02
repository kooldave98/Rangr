using System;
using App.Core.Portable.Models;

namespace App.Core.Portable.Device
{
	public interface ISession
	{
		User GetCurrentUser ();

		void AddCurrentUser (User user);
	}
}

