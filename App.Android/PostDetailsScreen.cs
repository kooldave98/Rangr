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
		Global _global;
		SeenPost post = new SeenPost();
		Button cancelButton;
		TextView postTextLabel;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			_global = Global.Current;

			int postID = Intent.GetIntExtra("PostID", 0);
			if(postID > 0) {
				//IDEALLY, this would be gotten from some persistence, maybe DB
				//Just using the global for now cause its much easier.
				post = _global.Feed.Posts.Single (p => p.id == postID);
			}


			// set our layout to be the home screen
			SetContentView(Resource.Layout.PostDetails);

			postTextLabel = FindViewById<TextView>(Resource.Id.PostTextLabel);
			cancelButton = FindViewById<Button>(Resource.Id.CancelButton);

			postTextLabel.Text = post.text;


			// button clicks 
			cancelButton.Click += (sender, e) => { Cancel(); };
		}


		void Cancel()
		{

			Finish();
		}
	}
}

