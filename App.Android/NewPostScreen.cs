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

//		PostRepository _postRepository;
//		IGeoLocation _geoLocationInstance;
//		ISession _sessionInstance;
		Application _global;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			_global = Global.Current;
//			_geoLocationInstance = GeoLocation.GetInstance (SynchronizationContext.Current, this);
//			_sessionInstance = Session.Current;
//			_postRepository = new PostRepository (new HttpRequest ());


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
			_global.client.sendPost (async(hubProxy) => {
				await hubProxy.Invoke ("sendPost", postText);
			});
			//var geoLocationString = await _geoLocationInstance.GetCurrentPosition();
			//await _postRepository.CreatePost(postText, _sessionInstance.GetCurrentUser().ID.ToString(), geoLocationString);

			Finish();
		}

		void Cancel()
		{

			Finish();
		}
	}
}