using System;
using ios_ui_lib;
using App.Common;
using common_lib;

namespace rangr.ios
{
    public class VerifyNumberViewController : UILoginViewController
    {
        private LoginViewModel view_model ;

        public event Action VerificationSucceeded = delegate {};
        public event Action VerificationFailed = delegate {};

        public VerifyNumberViewController(string mobile_number)
        {
            view_model = new LoginViewModel(){
                user_mobile_number = mobile_number
            };
        }

        public async override void ViewDidLoad()
        {
            base.ViewDidLoad();

            //view_model.user_mobile_number = login_view.UserIDField.Text;

            login_view.PasswordField.BecomeFirstResponder();

            login_view.UserIDField.BorderStyle = UIKit.UITextBorderStyle.None;
            login_view.PasswordField.BorderStyle = UIKit.UITextBorderStyle.None;

            login_view.UserIDField.ShouldBeginEditing += (t)=> false;

            await view_model.CreateUser();
        }

        protected override async void OnRequestLogin(string id, string password)
        {
            base.OnRequestLogin(id, password);

            view_model.user_mobile_number = id;
            view_model.user_secret_code = password;

            show_progress("verifying number...");

            var create_user_successful = await view_model.VerifyUser();

            if (create_user_successful)
            {
                login_view.PasswordField.ResignFirstResponder();

                VerificationSucceeded();
            }
            else
            {
                show_toast("An error occurred.");


                login_view.PasswordField.SelectAll(this);
                login_view.PasswordField.BecomeFirstResponder();

                VerificationFailed();
            }

            dismiss_progress();
        }
    }
}

