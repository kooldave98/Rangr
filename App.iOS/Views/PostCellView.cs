using System;
using CoreGraphics;
using Foundation;
using UIKit;
using App.Common;

//using MonoTouch.ObjCRuntime;

namespace App.iOS
{
	[Register ("PostCellView")] 
	public partial class PostCellView : UITableViewCell
	{
		const string PlaceholderImagePath = "Placeholder.jpg";

		public static readonly UINib Nib = UINib.FromName ("PostCellView", NSBundle.MainBundle);
		public static readonly NSString Key = new NSString ("PostCellView");

		public PostCellView (IntPtr handle) : base (handle)
		{
		}

		public static PostCellView Create ()
		{
			return (PostCellView)Nib.Instantiate (null, null) [0];
		}

		public void BindDataToCell (Post post)
		{
			UserNameView.Text = post.user_display_name;
			UserPostTextView.Text = post.text;
			UserImageView.Image = UIImage.FromBundle (PlaceholderImagePath);
			TimeAgoView.Text = TimeAgoConverter.Current.Convert (post.date);
		}
	}
}

