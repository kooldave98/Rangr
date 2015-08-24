
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using App.Common;
using Android.Views.InputMethods;

namespace rangr.droid
{
    public class LoginFragment : VMFragment<LoginViewModel>, TextView.IOnEditorActionListener
    {
        public override string TitleLabel
        { 
            get
            {
                return GetString(Resource.String.app_name);
            } 
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.login, null);

            login = view.FindViewById<Button>(Resource.Id.logIn);
            userName = view.FindViewById<EditText>(Resource.Id.userName);
            password = view.FindViewById<EditText>(Resource.Id.password);
            progressIndicator = view.FindViewById<ProgressBar>(Resource.Id.loginProgress);
            var loginHelp = view.FindViewById<ImageButton>(Resource.Id.loginQuestion);


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

            return view;
        }

        public override void OnResume()
        {
            base.OnResume();
            if (!AppGlobal.Current.CurrentUserAndConnectionExists)
            {
                password.Text =
                    userName.Text = string.Empty;

                hide_progress();
            }
        }

        public override void OnPause()
        {
            base.OnPause();
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

                                LoginSucceeded();
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
            hide_keyboard_for(password);
            login.Visibility = ViewStates.Invisible;
            progressIndicator.Visibility = ViewStates.Visible;
        }

        public LoginFragment()
        {
            if (view_model == null)
            {
                view_model = new LoginViewModel();
            }
        }

        private EditText password, userName;
        private Button login;
        private ProgressBar progressIndicator;

        public event Action LoginSucceeded = delegate {};
    }
}

