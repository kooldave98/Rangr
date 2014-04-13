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
	[Activity (Label = "New Post", ScreenOrientation = ScreenOrientation.Portrait)]			
	public class NewPostActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			view_model = new NewPostViewModel (PersistentStorage.Current);

			// set our layout to be the home screen
			SetContentView (Resource.Layout.NewPost);

			postTextEdit = FindViewById<EditText> (Resource.Id.PostText);
			saveButton = FindViewById<Button> (Resource.Id.SaveButton);
			cancelButton = FindViewById<Button> (Resource.Id.CancelButton);

			postTextEdit.TextChanged += (sender, e) => {
				view_model.PostText = postTextEdit.Text;
			};

			saveButton.Click += HandleSaveButtonClicked;

			cancelButton.Click += HandleCancelClicked;
		}

		private async void HandleSaveButtonClicked (object sender, EventArgs e)
		{
			if (!string.IsNullOrWhiteSpace (view_model.PostText)) {
				await view_model.CreatePost ();

				SetResult (Result.Ok);

				Finish ();
			}
		}

		private void HandleCancelClicked (object sender, EventArgs e)
		{
			Finish ();
		}

		private NewPostViewModel view_model;
		private Button cancelButton;
		private EditText postTextEdit;
		private Button saveButton;
	}
}