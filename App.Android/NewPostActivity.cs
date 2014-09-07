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
using App.Core.Portable.Device;
using App.Core.Portable.Models;

namespace App.Android
{
	[Activity (Label = "@string/app_name", ScreenOrientation = ScreenOrientation.Portrait)]			
	public class NewPostActivity : BaseActivity
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
			SetContentView (Resource.Layout.NewPost);

			ActionBar.SetDisplayHomeAsUpEnabled (true);

			FindViewById<TextView> (Resource.Id.UserNameText).SetText (view_model.CurrentUser.user_display_name, TextView.BufferType.Normal);

			FindViewById<EditText> (Resource.Id.PostText).TextChanged += HandlePostTextChanged;

		}

		private IMenuItem send_button;

		public override bool OnCreateOptionsMenu (IMenu menu)
		{
			send_button = menu.Add ("Send").SetEnabled (false);
			send_button.SetShowAsAction (ShowAsAction.IfRoom);

			return base.OnCreateOptionsMenu (menu);

			//return true;
		}

		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			switch (item.TitleFormatted.ToString ()) { 
			case "Send":
				HandleSaveButtonClicked (item, EventArgs.Empty);
				break;
			}

			return base.OnOptionsItemSelected (item);
		}

		private async void HandleSaveButtonClicked (object sender, EventArgs e)
		{
			send_button.SetEnabled (false);
			if (!string.IsNullOrWhiteSpace (view_model.PostText)) {

				if (AppGlobal.Current.IsGeoLocatorRefreshed) {
					var successful = await view_model.CreatePost ();

					if (successful) {
						SetResult (Result.Ok);

						Finish ();
					}
				} else {
					AppEvents.Current.TriggerGeolocatorFailedEvent ();
				}
			}
			send_button.SetEnabled (true);
		}

		private void HandlePostTextChanged (object sender, EventArgs e)
		{
			var text = ((EditText)sender).Text;

			if (string.IsNullOrWhiteSpace (text)) {
				send_button.SetEnabled (false);
			
			} else {
				view_model.PostText = text;

				send_button.SetEnabled (true);
			}

		}


		private NewPostViewModel view_model;

		protected override ViewModelBase init_view_model ()
		{

			if (view_model == null) {
				view_model = new NewPostViewModel (PersistentStorage.Current);
			}
			return view_model;
			
		}
	}
}