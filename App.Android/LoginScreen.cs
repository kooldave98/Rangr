using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using App.Core.Portable.Device;
using Android.Content.PM;
using App.Common.Shared;

namespace App.Android
{
	[Activity (Label = "Login", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait)]			
	public class LoginScreen : Activity
	{
		ISession _sessionInstance = Session.Current;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			var _httpRequest = new HttpRequest ();

			var user = _sessionInstance.GetCurrentUser ();

			if (user != null) {
				StartActivity (typeof(MainActivity));
			}




			//Create the user interface in code
			var layout = new LinearLayout (this);
			layout.Orientation = Orientation.Vertical;

			var aLabel = new TextView (this);
			aLabel.Text = "Enter a display name to continue";

			var textBox = new EditText (this);

			var aButton = new Button (this);
			aButton.Text = "Continue";
			aButton.Click += async (sender, e) => {
				if (!string.IsNullOrWhiteSpace(textBox.Text)){
					var userID = await new UserRepository(_httpRequest).CreateUser(textBox.Text);
					user = await new UserRepository(_httpRequest).GetUserById(userID.ID);
					_sessionInstance.AddCurrentUser(user);
					StartActivity (typeof(MainActivity));
				}
			};
			layout.AddView (aLabel);
			layout.AddView (textBox);
			layout.AddView (aButton);
			SetContentView (layout);
		}
	}
}

