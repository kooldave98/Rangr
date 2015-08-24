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
using System.Xml.Serialization;



namespace App.Android
{
	[Activity (Label = "@string/app_name", ScreenOrientation = ScreenOrientation.Portrait)]			
	public class PostDetailsActivity : BaseActivity
	{

		public override bool OnNavigateUp ()
		{
			base.OnNavigateUp ();

			Finish ();

			return true;
		}

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// set our layout to be the home screen
			SetContentView (Resource.Layout.PostDetails);

			ActionBar.SetDisplayHomeAsUpEnabled (true);

			set_map ();

			FindViewById<TextView> (Resource.Id.UserNameText).SetText (view_model.CurrentPost.user_display_name, TextView.BufferType.Normal);

			postTextLabel = FindViewById<TextView> (Resource.Id.PostTextLabel);

			postTextLabel.Text = view_model.CurrentPost.text;
		}

		protected override void OnPause ()
		{
			base.OnPause ();

			Finish ();
		}

		public void set_map ()
		{
			var transaction = FragmentManager.BeginTransaction ();

			transaction.Replace (Resource.Id.fragmentContainer, new PostMapFragment (view_model.CurrentPost));

			transaction.Commit ();
		}



		public static Intent CreateIntent (Context context, Post post)
		{
			var postStream = PostDetailsViewModel.Serialize (post);

			var intent = new Intent (context, typeof(PostDetailsActivity))
								.PutExtra ("Post", postStream.ToArray ());
			return intent;
		}

		private TextView postTextLabel;

		private PostDetailsViewModel view_model;

		protected override ViewModelBase init_view_model ()
		{
			if (view_model == null) {
				Post post;
				if (Intent.HasExtra ("Post")) {
					var postBytes = Intent.GetByteArrayExtra ("Post");
					post = PostDetailsViewModel.Deserialize (postBytes);
				} else {
					post = new Post ();
				}

				view_model = new PostDetailsViewModel (post);
			}

			return view_model;

		}
	}
}
