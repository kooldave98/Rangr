using System;
using CoreGraphics;
using Foundation;
using UIKit;

//using MonoTouch.ObjCRuntime;

namespace experiments.ios
{
    [Register("PostCellView")] 
    public partial class PostCellView : UITableViewCell
    {
        const string PlaceholderImagePath = "user-default-avatar.png";

        public static readonly UINib Nib = UINib.FromName("PostCellView", NSBundle.MainBundle);
        public static readonly NSString Key = new NSString("PostCellView");

        public PostCellView(IntPtr handle)
            : base(handle)
        {
        }

        public static PostCellView Create()
        {
            return (PostCellView)Nib.Instantiate(null, null)[0];
        }

        public void BindDataToCell()
        {
            UserNameView.Text = "display name";
            UserPostTextView.Text = "The quick brown fox jumped over the lazy dog";
            UserImageView.Image = UIImage.FromBundle(PlaceholderImagePath);
            TimeAgoView.Text = "5 days ago";
        }
    }
}

