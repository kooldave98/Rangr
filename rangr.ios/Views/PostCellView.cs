using System;
using CoreGraphics;
using Foundation;
using UIKit;
using App.Common;
using ios_ui_lib;

namespace rangr.ios
{
    public partial class PostCellView : UITableViewCell
    {
        private bool didSetupConstraints;
        const string PlaceholderImagePath = "user-default-avatar.png";
        public static readonly NSString Key = new NSString("PostCellView");

        public UILabel time_ago;
        public UIImageView user_image;
        public UILabel user_name;
        public UILabel post_text;

        public override void UpdateConstraints()
        {
            base.UpdateConstraints();

            if (this.didSetupConstraints)
            {
                return;
            }

            this.user_name.SetContentCompressionResistancePriority(Layout.RequiredPriority, UILayoutConstraintAxis.Vertical);
            this.time_ago.SetContentCompressionResistancePriority(Layout.RequiredPriority, UILayoutConstraintAxis.Vertical);
            this.post_text.SetContentCompressionResistancePriority(Layout.RequiredPriority, UILayoutConstraintAxis.Vertical);


            var user_image_width = HumanInterface.medium_square_image_length;
            var user_image_height = HumanInterface.medium_square_image_length;
            var sibling_sibling_margin = HumanInterface.sibling_sibling_margin;

            this.ConstrainLayout(() => 
                ContentView.Frame.Top == this.Bounds.Top + sibling_sibling_margin &&
                ContentView.Frame.Left == this.Bounds.Left + sibling_sibling_margin &&
                ContentView.Frame.Right == this.Bounds.Right - sibling_sibling_margin &&
                ContentView.Frame.Bottom == this.Bounds.Bottom - sibling_sibling_margin
            );

            ContentView.ConstrainLayout(() => 
                user_image.Frame.Width == user_image_width &&
                user_image.Frame.Height == user_image_height &&
                user_image.Frame.Top == ContentView.Frame.Top + sibling_sibling_margin &&
                user_image.Frame.Left == ContentView.Frame.Left + sibling_sibling_margin &&

                user_name.Frame.Left == user_image.Frame.Right + sibling_sibling_margin &&
                user_name.Frame.Right == ContentView.Frame.Right - sibling_sibling_margin &&
                user_name.Frame.Top == ContentView.Frame.Top + sibling_sibling_margin &&

                time_ago.Frame.Left == user_image.Frame.Right + sibling_sibling_margin &&
                time_ago.Frame.Right == ContentView.Frame.Right - sibling_sibling_margin &&
                time_ago.Frame.Top == user_name.Frame.Bottom + sibling_sibling_margin &&
                time_ago.Frame.Bottom <= user_image.Frame.Bottom &&

                post_text.Frame.Left == ContentView.Frame.Left + sibling_sibling_margin &&
                post_text.Frame.Right == ContentView.Frame.Right - sibling_sibling_margin &&
                post_text.Frame.Top >= time_ago.Frame.Bottom + sibling_sibling_margin &&
                post_text.Frame.Bottom == ContentView.Frame.Bottom - sibling_sibling_margin

            );

            this.didSetupConstraints = true;
        }

        public void BindDataToCell(Post post)
        {
            user_name.Text = post.user_display_name;
            post_text.Text = post.text + "\n";
            user_image.Image = UIImage.FromBundle(PlaceholderImagePath);
            time_ago.Text = TimeAgoConverter.Current.Convert(post.date);
        }

        public PostCellView()
        {
            this.create_view();
        }

        public PostCellView(IntPtr handle)
            : base(handle)
        {
            this.create_view();
        }

        private void create_view()
        {
            SelectionStyle = UITableViewCellSelectionStyle.None;
            BackgroundColor = UIColor.LightGray;


            ContentView.BackgroundColor = UIColor.White;
            //ContentView.Layer.CornerRadius = 10;
            //ContentView.Layer.MasksToBounds = true;
            ContentView.TranslatesAutoresizingMaskIntoConstraints = false;


            ContentView.AddSubview(user_image = new UIImageView(){
                TranslatesAutoresizingMaskIntoConstraints = false
            });

            ContentView.AddSubview(user_name = new UILabel(){
                TextColor = UIColor.Black,
                Font = UIFont.BoldSystemFontOfSize(17.0f),
                TextAlignment = UITextAlignment.Left,
                Lines = 1,
                LineBreakMode = UILineBreakMode.TailTruncation,
                TranslatesAutoresizingMaskIntoConstraints = false
            });

            ContentView.AddSubview(time_ago = new UILabel(){
                TextColor = UIColor.Black,
                Font = UIFont.SystemFontOfSize(10.0f),
                TextAlignment = UITextAlignment.Left,
                Lines = 1,
                LineBreakMode = UILineBreakMode.TailTruncation,
                TranslatesAutoresizingMaskIntoConstraints = false
            });

            ContentView.AddSubview(post_text = new UILabel(){
                TextColor = UIColor.Black,
                Font = UIFont.PreferredSubheadline,
                TextAlignment = UITextAlignment.Left,
                Lines = 0,
                LineBreakMode = UILineBreakMode.WordWrap,
                TranslatesAutoresizingMaskIntoConstraints = false
            });

        }
    }
}

