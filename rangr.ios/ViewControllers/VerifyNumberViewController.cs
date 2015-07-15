using System;
using ios_ui_lib;
using App.Common;
using common_lib;

namespace rangr.ios
{
    public class VerifyNumberViewController : AdvancedLoginViewController
    {
        private LoginViewModel view_model ;

        public VerifyNumberViewController(LoginViewModel the_view_model)
        {
            view_model = Guard.IsNotNull(the_view_model, "the_view_model");
        }

        protected override async void Login()
        {
            if (string.IsNullOrEmpty(login_view.UserIDField.Text))
            {
                show_alert("Oops", "Please enter a valid number.", "Ok", () => {
                    login_view.UserIDField.BecomeFirstResponder();
                });

                return;
            }
            if (string.IsNullOrEmpty(login_view.PasswordField.Text))
            {
                show_alert("Oops", "Please enter a password.", "Ok", () => {
                    login_view.PasswordField.BecomeFirstResponder();
                });

                return;
            }

            view_model.user_mobile_number = login_view.UserIDField.Text;
            view_model.user_mobile_number = login_view.PasswordField.Text;

            show_progress("verifying number...");

            var create_user_successful = await view_model.VerifyUser();

            if (create_user_successful)
            {
                login_view.PasswordField.ResignFirstResponder();

                //LoginSucceeded();
            }
            else
            {
                show_toast("An error occurred.");


                login_view.PasswordField.SelectAll(this);
                login_view.PasswordField.BecomeFirstResponder();
            }

            dismiss_progress();
        }
    }
}

