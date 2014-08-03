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
using Android.Views.InputMethods;

namespace App.Android
{
	[Activity (Label = "@string/app_name", ScreenOrientation = ScreenOrientation.Portrait)]			
	public class EditProfileActivity : BaseActivity
	{
		protected override void OnConnectionEstablished()
		{

		}

		public override bool OnNavigateUp ()
		{
			//<hack> to ensure that status message gets saved
			display_name_field.RequestFocus ();
			//</hack>
			base.OnNavigateUp ();
			Finish ();

			return true;
		}

		protected override void OnCreate (Bundle bundle)
		{
			Title = "Edit Profile";

			base.OnCreate (bundle);

			// set our layout to be the home screen
			SetContentView (Resource.Layout.EditProfile);

			ActionBar.SetDisplayHomeAsUpEnabled (true);

			display_name_field = FindViewById<EditText> (Resource.Id.displayName);
			display_name_field.SetText (view_model.CurrentUserToBeEdited.user_display_name, TextView.BufferType.Normal);
			display_name_field.FocusChange += HandleDisplayNameChanged;

			status_message_field = FindViewById<EditText> (Resource.Id.statusMessage);
			status_message_field.SetText (view_model.CurrentUserToBeEdited.user_status_message, TextView.BufferType.Normal);
			status_message_field.FocusChange += HandleStatusMessageChanged;



			display_name_field.RequestFocus ();
		}

		private async void HandleDisplayNameChanged (object sender, EventArgs e)
		{
			var the_sender = (EditText)sender;
			if (!the_sender.HasFocus) {
				var text = the_sender.Text;

				if (!string.IsNullOrWhiteSpace (text) && text != view_model.CurrentUserToBeEdited.user_display_name) {
					view_model.CurrentUserToBeEdited.user_display_name = text;

					await view_model.UpdateUser ();

					SetResult (Result.Ok);
				}
			}
		}

		private async void HandleStatusMessageChanged (object sender, EventArgs e)
		{
			var the_sender = (EditText)sender;
			if (!the_sender.HasFocus) {
				var text = the_sender.Text;

				if (!string.IsNullOrWhiteSpace (text) && text != view_model.CurrentUserToBeEdited.user_status_message) {
					view_model.CurrentUserToBeEdited.user_status_message = text;

					await view_model.UpdateUser ();

					SetResult (Result.Ok);
				}
			}
		}

		public bool OnEditorAction (TextView v, ImeAction actionId, KeyEvent e)
		{
			if (actionId == ImeAction.Next) {
				if (display_name_field.HasFocus) {
					status_message_field.RequestFocus ();
				} else if (status_message_field.HasFocus) {
					display_name_field.RequestFocus ();
				}
				return true;
			}

			return false;
		}

		private EditText display_name_field;
		private EditText status_message_field;

		private EditProfileViewModel view_model;

		protected override ViewModelBase init_view_model ()
		{

			if (view_model == null) {
				view_model = new EditProfileViewModel (PersistentStorage.Current);
			}
			return view_model;

		}
	}
}