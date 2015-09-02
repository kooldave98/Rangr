using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using rangr.common;

namespace rangr.droid
{
    [Activity(Label = "@string/app_name"				
				, NoHistory = true
                //, LaunchMode = LaunchMode.SingleTop
				, ScreenOrientation = ScreenOrientation.Portrait)]	
	public class LoginActivity : BaseActivity, TextView.IOnEditorActionListener
    {

        private EditText password, userName;
        private Button login;
        private ProgressBar progressIndicator;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Login);

            // Get our controls from the layout resource,
            // and attach an event to it
            login = FindViewById<Button>(Resource.Id.logIn);
            userName = FindViewById<EditText>(Resource.Id.userName);
            password = FindViewById<EditText>(Resource.Id.password);
            progressIndicator = FindViewById<ProgressBar>(Resource.Id.loginProgress);
            var loginHelp = FindViewById<ImageButton>(Resource.Id.loginQuestion);



            //Set edit action listener to allow the next & go buttons on the input keyboard to interact with login.
            userName.SetOnEditorActionListener(this);
            password.SetOnEditorActionListener(this);

            userName.TextChanged += (sender, e) =>
            {
                view_model.UserDisplayName = userName.Text;
            };
            password.TextChanged += (sender, e) =>
            {
                //loginViewModel.Password = password.Text;
            };

            loginHelp.Click += (sender, e) =>
            {
                ShowAlert("Need Help?", "Enter your desired display name and your given beta testing key.");
            };

            // Perform the login and dismiss the keyboard
            login.Click += DoLogin;

            //request focus to the edit text to start on username.
            userName.RequestFocus();


        }

        private void DoLogin(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(userName.Text))
            {

                if (!string.IsNullOrEmpty(password.Text) && password.Text == "wertyc")
                {

                    hide_keyboard_and_show_progress();

                    ShowAlert("Disclaimer", "This app is in beta, it may be subject to changes, loss of data and unavailability.", "Ok", async delegate
                        {
                            var create_user_successful = await view_model.CreateUser();

                            if (create_user_successful)
                            {
                                await AppGlobal.Current.CreateNewConnectionFromLogin();

                                StartActivity(typeof(StartUpActivity));
                                Finish();
                            }

                            hide_progress();
                        });


                }
                else
                {
                    ShowAlert("Error", "Invalid code entered. Please request a test code by emailing walkr@davidolubajo.com. Thanks");
                }
            }
        }

        protected override void OnResume()
        {
            base.OnResume();
            if (!AppGlobal.Current.CurrentUserAndConnectionExists)
            {
                password.Text =
				userName.Text = string.Empty;

                hide_progress();
            }
        }

        protected override void OnPause()
        {
            base.OnPause();
        }

        #region"editor and keyboard stuff"

        /// <summary>
        /// Observes the TextView's ImeAction so an action can be taken on keypress.
        /// </summary>
        public bool OnEditorAction(TextView v, ImeAction actionId, KeyEvent e)
        {
            //go edit action will login
            if (actionId == ImeAction.Go)
            {
                if (!string.IsNullOrEmpty(userName.Text))
                {
                    DoLogin(this, EventArgs.Empty);
                }
                else if (string.IsNullOrEmpty(userName.Text))
                {
                    userName.RequestFocus();
                }
                else
                {
                    password.RequestFocus();
                }
                return true;
                //next action will set focus to password edit text.
            }
            else if (actionId == ImeAction.Next)
            {
                if (!string.IsNullOrEmpty(userName.Text))
                {
                    password.RequestFocus();
                }
                return true;
            }
            return false;
        }

        private void hide_progress()
        {
            login.Visibility = ViewStates.Visible;
            progressIndicator.Visibility = ViewStates.Invisible;
        }


        private void hide_keyboard_and_show_progress()
        {
            //this hides the keyboard
            var imm = (InputMethodManager)GetSystemService(Context.InputMethodService);
            imm.HideSoftInputFromWindow(password.WindowToken, HideSoftInputFlags.NotAlways);
            login.Visibility = ViewStates.Invisible;
            progressIndicator.Visibility = ViewStates.Visible;
        }

        #endregion


        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu, menu);

            menu.FindItem(Resource.Id.feed_menu_item).SetVisible(false);
            menu.FindItem(Resource.Id.people_menu_item).SetVisible(false);
            menu.FindItem(Resource.Id.profile_menu_item).SetVisible(false);
            menu.FindItem(Resource.Id.simulation_menu_item).SetVisible(false);

            return base.OnCreateOptionsMenu(menu);

            //return true;
        }

        private LoginViewModel view_model;

        protected override ViewModelBase init_view_model()
        {
            if (view_model == null)
            {
                view_model = new LoginViewModel();
            }

            return view_model;
        }

    }
}
