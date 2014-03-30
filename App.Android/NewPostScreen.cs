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
using App.Core.Portable.Models;
using App.Core.Portable.Device;
using App.Common.Shared;
using System.Threading;
using Android.Content.PM;

namespace App.Android
{
	[Activity (Label = "New Post", ScreenOrientation = ScreenOrientation.Portrait)]			
	public class NewPostScreen : Activity
	{
		Button cancelButton;
		EditText postTextEdit;
		Button saveButton;


		Global _global;
		Posts post_services;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			_global = Global.Current;
			post_services = new Posts(HttpRequest.Current);

			// set our layout to be the home screen
			SetContentView(Resource.Layout.NewPost);



			postTextEdit = FindViewById<EditText>(Resource.Id.PostText);
			saveButton = FindViewById<Button>(Resource.Id.SaveButton);
			cancelButton = FindViewById<Button>(Resource.Id.CancelButton);

			saveButton.Click += (object sender, EventArgs e) => { Save();};

			// button clicks 
			cancelButton.Click += (sender, e) => { Cancel(); };
		}

		private void Save()
		{
			//Post to server
			var postText = this.postTextEdit.Text;
			post_services.Create(postText, _global.current_connection.connection_id.ToString());

			Finish();
		}

		void Cancel()
		{
			Finish();
		}
	}
}