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
using Android.Content.PM;

namespace App.Android
{
	[Activity (Label = "PostDetailsScreen", ScreenOrientation = ScreenOrientation.Portrait)]			
	public class PostDetailsScreen : Activity
	{
		Application _global;
		Post post = new Post();
		Button cancelButton;
		TextView postTextLabel;
		Button saveButton;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			_global = Global.Current;

			int postID = Intent.GetIntExtra("PostID", 0);
			if(postID > 0) {
				post = _global.Posts.Single (p => p.ID == postID);
			}


			// set our layout to be the home screen
			SetContentView(Resource.Layout.PostDetails);

			postTextLabel = FindViewById<TextView>(Resource.Id.PostTextLabel);
			cancelButton = FindViewById<Button>(Resource.Id.CancelButton);

			postTextLabel.Text = post.Text;


			// button clicks 
			cancelButton.Click += (sender, e) => { Cancel(); };
		}


		void Cancel()
		{

			Finish();
		}
	}
}

