using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using App.Core.Portable.Models;
//using MonoTouch.ObjCRuntime;

namespace App.iOS
{
	[Register("PostCellView")] 
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

		public void BindDataToCell(Post post)
		{
			UserNameView.Text = post.UserDisplayName;
			UserPostTextView.Text = post.Text;
			UserImageView.Image = UIImage.FromBundle (PlaceholderImagePath);
		}
	}
}

