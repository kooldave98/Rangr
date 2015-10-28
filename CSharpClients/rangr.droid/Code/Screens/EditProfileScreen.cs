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
using rangr.common;
using Android.Views.InputMethods;

namespace rangr.droid
{
	[Activity (Label = "@string/app_name", ScreenOrientation = ScreenOrientation.Portrait)]			
	public class EditProfileActivity : FragmentActivity
	{
        private EditProfileFragment the_fragment;
        private EditProfileFragment Fragment {
            get{ 
                return the_fragment ?? (the_fragment = new EditProfileFragment());
            }
        }

        public override bool OnNavigateUp ()
        {
            //<hack> to ensure that status message gets saved
            Fragment.display_name_field.RequestFocus ();
            //</hack>
            base.OnNavigateUp ();
            Finish ();

            return true;
        }

        protected override void OnCreate (Bundle bundle)
        {
            base.OnCreate (bundle);

            PushFragment(Fragment);

            ActionBar.SetDisplayHomeAsUpEnabled (true);

            Fragment.EditCompleted += () =>{
                SetResult (Result.Ok);
            };

        }
    }

    public class EditProfileFragment : VMFragment<EditProfileViewModel>
    {

        public override string TitleLabel { 
            get {
                return "Edit Profile"; //GetString(Resource.String.new_post_title);
            } 
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.edit_profile, null);

            display_name_field = view.FindViewById<EditText> (Resource.Id.displayName);
            display_name_field.SetText (view_model.CurrentUserToBeEdited.user_display_name, TextView.BufferType.Normal);
            display_name_field.FocusChange += HandleDisplayNameChanged;

            status_message_field = view.FindViewById<EditText> (Resource.Id.statusMessage);
            status_message_field.SetText (view_model.CurrentUserToBeEdited.user_status_message, TextView.BufferType.Normal);
            status_message_field.FocusChange += HandleStatusMessageChanged;



            display_name_field.RequestFocus ();

            return view;
        }

        public event Action EditCompleted = delegate {};

		private async void HandleDisplayNameChanged (object sender, EventArgs e)
		{
			var the_sender = (EditText)sender;
			if (!the_sender.HasFocus) {
				var text = the_sender.Text;

				if (!string.IsNullOrWhiteSpace (text) && text != view_model.CurrentUserToBeEdited.user_display_name) {
					view_model.CurrentUserToBeEdited.user_display_name = text;

					var result = await view_model.UpdateUser ();

					if (result) {
                        EditCompleted ();
					}
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

					var result = await view_model.UpdateUser ();

					if (result) {
                        EditCompleted ();
					}
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

		public EditText display_name_field;
		private EditText status_message_field;

        public EditProfileFragment()
        {
            if (view_model == null)
            {
                view_model = new EditProfileViewModel();
            }
        }
	}
}