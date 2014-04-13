using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using App.Common.Shared;
using App.Core.Android;
using App.Core.Portable.Device;
using App.Common;

namespace App.Android
{
	[Activity (Label = "@string/app_name"
				, MainLauncher = true
				, NoHistory = true
				, ScreenOrientation = ScreenOrientation.Portrait
				, LaunchMode = LaunchMode.SingleTop)]			
	public class LoginActivity : Activity, TextView.IOnEditorActionListener
	{
		private LoginViewModel view_model { get; set;}

		private EditText password, userName;
		private Button login;
		private ProgressBar progressIndicator;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			view_model = new LoginViewModel (PersistentStorage.Current);

			//Check if the user exists first before populating the view

			if (view_model.CurrentUserExists) {
				StartActivity (typeof(MainActivity));
				Finish ();
			} else {
				// Set our view from the "main" layout resource
				SetContentView (Resource.Layout.Login);

				// Get our controls from the layout resource,
				// and attach an event to it
				login = FindViewById<Button> (Resource.Id.logIn);
				userName = FindViewById<EditText> (Resource.Id.userName);
				password = FindViewById<EditText> (Resource.Id.password);
				progressIndicator = FindViewById<ProgressBar> (Resource.Id.loginProgress);
				var loginHelp = FindViewById<ImageButton> (Resource.Id.loginQuestion);



				//Set edit action listener to allow the next & go buttons on the input keyboard to interact with login.
				userName.SetOnEditorActionListener (this);
				password.SetOnEditorActionListener (this);

				userName.TextChanged += (sender, e) => {
					view_model.UserDisplayName = userName.Text;
				};
				password.TextChanged += (sender, e) => {
					//loginViewModel.Password = password.Text;
				};

				loginHelp.Click += (sender, e) => {
					var builder = new AlertDialog.Builder (this)
					.SetTitle ("Need Help?")
					.SetMessage ("Enter any username or password.")
					.SetPositiveButton ("Ok", (innerSender, innere) => {
					});
					var dialog = builder.Create ();
					dialog.Show ();
				};

				// Perform the login and dismiss the keyboard
				login.Click += DoLogin;

				//request focus to the edit text to start on username.
				userName.RequestFocus ();
			}

		}

		private async void DoLogin(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty (userName.Text)) {

				hide_keyboard_and_show_progress ();

				await view_model.Login ();

				//RunOnUiThread (() => {
				StartActivity (typeof(MainActivity));
				//});
			}
		}

		private void hide_keyboard_and_show_progress()
		{
			//this hides the keyboard
			var imm = (InputMethodManager)GetSystemService (Context.InputMethodService);
			imm.HideSoftInputFromWindow (password.WindowToken, HideSoftInputFlags.NotAlways);
			login.Visibility = ViewStates.Invisible;
			progressIndicator.Visibility = ViewStates.Visible;
		}
			
		protected override void OnResume ()
		{
			base.OnResume ();

			password.Text =
				userName.Text = string.Empty;

			login.Visibility = ViewStates.Visible;
			progressIndicator.Visibility = ViewStates.Invisible;
		}

		/// <summary>
		/// Observes the TextView's ImeAction so an action can be taken on keypress.
		/// </summary>
		public bool OnEditorAction (TextView v, ImeAction actionId, KeyEvent e)
		{
			//go edit action will login
			if (actionId == ImeAction.Go) {
				if (!string.IsNullOrEmpty (userName.Text)) {
					DoLogin (this, EventArgs.Empty);
				} else if (string.IsNullOrEmpty (userName.Text)) {
					userName.RequestFocus ();
				} else {
					password.RequestFocus ();
				}
				return true;
				//next action will set focus to password edit text.
			} else if (actionId == ImeAction.Next) {
				if (!string.IsNullOrEmpty (userName.Text)) {
					password.RequestFocus ();
				}
				return true;
			}
			return false;
		}
	}
}


////Create the user interface in code
//var layout = new LinearLayout (this);
//layout.Orientation = Orientation.Vertical;
//
//var aLabel = new TextView (this);
//aLabel.Text = "Enter a display name to continue";
//
//var textBox = new EditText (this);
//
//var aButton = new Button (this);
//aButton.Text = "Continue";
//aButton.Click += async (sender, e) => {
//	if (!string.IsNullOrWhiteSpace(textBox.Text)){
//		var userID = await new UserRepository(_httpRequest).CreateUser(textBox.Text);
//		user = await new UserRepository(_httpRequest).GetUserById(userID.ID);
//		_sessionInstance.AddCurrentUser(user);
//		StartActivity (typeof(MainActivity));
//	}
//};
//layout.AddView (aLabel);
//layout.AddView (textBox);
//layout.AddView (aButton);
//SetContentView (layout);
//
