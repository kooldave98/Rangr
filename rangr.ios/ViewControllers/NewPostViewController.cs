using System;
using CoreGraphics;
using App.Common;
using Foundation;
using UIKit;
using System.Drawing;

namespace rangr.ios
{
    public partial class NewPostViewController : BaseViewModelController<NewPostViewModel>
    {
        private UIButton CreatePostBtn;
        private UITextView NewPostTbx;

        public event Action CreatePostSucceeded = delegate {};

        public NewPostViewController()
        {
            view_model = new NewPostViewModel();
        }

        public override string TitleLabel
        {
            get
            {
                return "New Post";
            }
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.Add(NewPostTbx = new UITextView());

            CreatePostBtn = UIButton.FromType(UIButtonType.RoundedRect);
            CreatePostBtn.SetTitle("Send", UIControlState.Normal);
            CreatePostBtn.TouchUpInside += Save;

            View.Add(CreatePostBtn);

            // Perform any additional setup after loading the view, typically from a nib.
            //this.CreatePostBtn.TouchUpInside += Save;
        }

        public override void ViewDidLayoutSubviews()
        {
            NewPostTbx.Frame = new CGRect(8, 8, 304, 348);
            //CreatePostBtn.CenterInParent();
            CreatePostBtn.Frame = new CGRect(8, 200, 304, 348);
        }

        private async void Save(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(NewPostTbx.Text))
            {
                view_model.PostText = this.NewPostTbx.Text;

                await view_model.CreatePost();

                CreatePostSucceeded();
            }
        }

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
        }


    }
}

