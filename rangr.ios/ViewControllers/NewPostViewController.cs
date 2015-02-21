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
            Theme.Primitive.Apply(NewPostTbx);

            CreatePostBtn = UIButton.FromType(UIButtonType.RoundedRect);
            CreatePostBtn.SetTitle("Send", UIControlState.Normal);
            CreatePostBtn.TouchUpInside += Save;

            View.Add(CreatePostBtn);

        }

        public override void ViewDidLayoutSubviews()
        {

            View.ConstrainLayout(() => 
                NewPostTbx.Frame.Top == View.Frame.Top + parent_child_margin &&
                NewPostTbx.Frame.Left == View.Frame.Left + parent_child_margin &&
                NewPostTbx.Frame.Right == View.Frame.Right - parent_child_margin &&
                NewPostTbx.Frame.Height == View.Frame.Width * 0.5f &&


                CreatePostBtn.Frame.Top == NewPostTbx.Frame.Bottom + parent_child_margin &&
                CreatePostBtn.Frame.Height == finger_tip_diameter &&
                CreatePostBtn.Frame.Width == CreatePostBtn.Frame.Height * 2.0f &&
                CreatePostBtn.Frame.GetCenterX() == View.Frame.GetCenterX()
            );
        
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

