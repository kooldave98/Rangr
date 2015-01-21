// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace rangr.ios
{
    partial class PostCellView
    {
        [Outlet]
        UIKit.UILabel TimeAgoView { get; set; }

        [Outlet]
        UIKit.UIImageView UserImageView { get; set; }

        [Outlet]
        UIKit.UILabel UserNameView { get; set; }

        [Outlet]
        UIKit.UILabel UserPostTextView { get; set; }

        void ReleaseDesignerOutlets()
        {
            if (UserImageView != null)
            {
                UserImageView.Dispose();
                UserImageView = null;
            }

            if (UserNameView != null)
            {
                UserNameView.Dispose();
                UserNameView = null;
            }

            if (UserPostTextView != null)
            {
                UserPostTextView.Dispose();
                UserPostTextView = null;
            }

            if (TimeAgoView != null)
            {
                TimeAgoView.Dispose();
                TimeAgoView = null;
            }
        }
    }
}
