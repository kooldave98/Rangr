using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using App.Common;
using App.Core.Portable.Models;

namespace App.Android
{
	[Activity (Label = "PostDetailsScreen", ScreenOrientation = ScreenOrientation.Portrait)]			
	public class PostDetailsActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			view_model = new PostDetailsViewModel ();

			if (Intent.HasExtra ("Post")) {
				var postBytes = Intent.GetByteArrayExtra ("Post");
				view_model.Deserialize (postBytes);
			}

			// set our layout to be the home screen
			SetContentView (Resource.Layout.PostDetails);

			postTextLabel = FindViewById<TextView> (Resource.Id.PostTextLabel);
			cancelButton = FindViewById<Button> (Resource.Id.CancelButton);

			postTextLabel.Text = view_model.CurrentPost.text;

			cancelButton.Click += HandleCancelButtonClicked;
		}

		private void HandleCancelButtonClicked (object sender, EventArgs e)
		{
			Finish ();
		}

		public static Intent CreateIntent (Context context, SeenPost post)
		{
			var postStream = PostDetailsViewModel.Serialize (post);

			var intent = new Intent (context, typeof(PostDetailsActivity))
								.PutExtra ("Post", postStream.ToArray ());
			return intent;
		}

		PostDetailsViewModel view_model;

		Button cancelButton;
		TextView postTextLabel;
	}
}

