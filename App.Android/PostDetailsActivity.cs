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
using App.Core.Android;

namespace App.Android
{
	[Activity (Label = "@string/app_name", ScreenOrientation = ScreenOrientation.Portrait)]			
	public class PostDetailsActivity : Activity
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

			view_model = new PostDetailsViewModel ();

			if (Intent.HasExtra ("Post")) {
				var postBytes = Intent.GetByteArrayExtra ("Post");
				view_model.Deserialize (postBytes);
			}

			// set our layout to be the home screen
			SetContentView (Resource.Layout.PostDetails);

			ActionBar.SetDisplayHomeAsUpEnabled (true);

			FindViewById<TextView> (Resource.Id.UserNameText).SetText (view_model.CurrentPost.user_display_name, TextView.BufferType.Normal);

			postTextLabel = FindViewById<TextView> (Resource.Id.PostTextLabel);

			postTextLabel.Text = view_model.CurrentPost.text;
		}



		public static Intent CreateIntent (Context context, SeenPost post)
		{
			var postStream = PostDetailsViewModel.Serialize (post);

			var intent = new Intent (context, typeof(PostDetailsActivity))
								.PutExtra ("Post", postStream.ToArray ());
			return intent;
		}

		PostDetailsViewModel view_model;

		TextView postTextLabel;
	}
}

