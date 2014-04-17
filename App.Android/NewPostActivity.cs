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
using App.Common;
using App.Common.Shared;
using App.Core.Android;
using App.Core.Portable.Device;
using App.Core.Portable.Models;

namespace App.Android
{
	[Activity (Label = "@string/app_name", ScreenOrientation = ScreenOrientation.Portrait)]			
	public class NewPostActivity : Activity
	{

		public override bool OnNavigateUp()
		{
			base.OnNavigateUp ();

			Finish ();

			return true;
		}

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			view_model = new NewPostViewModel (PersistentStorage.Current);

			// set our layout to be the home screen
			SetContentView (Resource.Layout.NewPost);

			ActionBar.SetDisplayHomeAsUpEnabled (true);

			FindViewById<TextView> (Resource.Id.UserNameText).SetText (view_model.CurrentUser.user_display_name, TextView.BufferType.Normal);

			FindViewById<EditText> (Resource.Id.PostText).TextChanged += HandlePostTextChanged;

			FindViewById<Button> (Resource.Id.SaveButton).Click += HandleSaveButtonClicked;

		}

		private async void HandleSaveButtonClicked (object sender, EventArgs e)
		{
			if (!string.IsNullOrWhiteSpace (view_model.PostText)) {
				await view_model.CreatePost ();

				SetResult (Result.Ok);

				Finish ();
			}
		}

		private void HandlePostTextChanged(object sender, EventArgs e)
		{
			view_model.PostText = ((EditText)sender).Text;
		}

		private NewPostViewModel view_model;
	}
}