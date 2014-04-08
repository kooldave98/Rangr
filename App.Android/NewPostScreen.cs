using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using App.Common.Shared;
using App.Core.Portable.Device;
using App.Core.Portable.Models;

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

			saveButton.Click += async (object sender, EventArgs e) => { await Save();};

			// button clicks 
			cancelButton.Click += (sender, e) => { Cancel(); };
		}

		private async Task Save()
		{
			//Post to server
			var postText = this.postTextEdit.Text;
			await post_services.Create(postText, _global.current_connection.connection_id.ToString());

			SetResult(Result.Ok);

			Finish();
		}

		void Cancel()
		{
			Finish();
		}
	}
}