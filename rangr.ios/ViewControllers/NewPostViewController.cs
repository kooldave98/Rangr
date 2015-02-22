using System;
using CoreGraphics;
using App.Common;
using Foundation;
using UIKit;
using System.Drawing;
using ios_ui_lib;

namespace rangr.ios
{
    public partial class NewPostViewController : BaseViewModelController<NewPostViewModel>
    {
        private UITextView NewPostTbx;

        public event Action CreatePostSucceeded = delegate {};

        public NewPostViewController()
        {
            view_model = new NewPostViewModel();
        }

        public override string TitleLabel {
            get { return "New Post";  }
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.BackgroundColor = UIColor.LightGray;
            View.Add(NewPostTbx = new UITextView());
        }

        public override void ViewDidLayoutSubviews()
        {

            View.ConstrainLayout(() => 
                NewPostTbx.Frame.Top == View.Frame.Top + parent_child_margin &&
                NewPostTbx.Frame.Left == View.Frame.Left + parent_child_margin &&
                NewPostTbx.Frame.Right == View.Frame.Right - parent_child_margin &&
                NewPostTbx.Frame.Height == View.Frame.Width * 0.5f
            );
        
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            NewPostTbx.BecomeFirstResponder();
        }

        public async void Save(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(NewPostTbx.Text))
            {
                view_model.PostText = this.NewPostTbx.Text;

                await view_model.CreatePost();

                CreatePostSucceeded();
            }
        }
    }
}

