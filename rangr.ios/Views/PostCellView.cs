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
        const string PlaceholderImagePath = "user-default-avatar.png";
        public static readonly NSString Key = new NSString("PostCellView");

        public UILabel time_ago;
        public UIImageView user_image;
        public UILabel user_name;
        public UILabel post_text;

        public UIView content_view;

        public PostCellView()
        {
            SelectionStyle = UITableViewCellSelectionStyle.None;
            BackgroundColor = UIColor.LightGray;

            content_view = new UIView();
            content_view.BackgroundColor = UIColor.White;
            content_view.Layer.CornerRadius = 10;
            content_view.Layer.MasksToBounds = true;

            AddSubview(content_view);

            content_view.AddSubview(user_image = new UIImageView());

            content_view.AddSubview(user_name = new UILabel(){
                TextColor = UIColor.Black,
                Font = UIFont.BoldSystemFontOfSize(17.0f),
                TextAlignment = UITextAlignment.Left,
                Lines = 1
            });

            content_view.AddSubview(time_ago = new UILabel(){
                TextColor = UIColor.Black,
                Font = UIFont.SystemFontOfSize(10.0f),
                TextAlignment = UITextAlignment.Left,
                Lines = 1                
            });

            content_view.AddSubview(post_text = new UILabel(){
                TextColor = UIColor.Black,
                Font = UIFont.PreferredSubheadline,
                TextAlignment = UITextAlignment.Left,
                Lines = 0,
                LineBreakMode = UILineBreakMode.WordWrap,
                //VerticalAlignment = UITextVerticalAlignment.Top
            });

            user_name.apply_simple_border();
            time_ago.apply_simple_border();
            user_image.apply_simple_border();
            post_text.apply_simple_border();
            content_view.apply_simple_border();

        }

        public override void LayoutSubviews()
        {
            post_text.Text += "\n";
            post_text.SizeToFit();

            var user_name_height = user_name.Text.get_text_size_with_font(user_name.Font).Height;
            var time_ago_height = time_ago.Text.get_text_size_with_font(time_ago.Font).Height;
            var user_image_width = user_image.Image.Size.Width;
            var user_image_height = user_image.Image.Size.Height;
            var sibling_sibling_margin = HumanInterface.sibling_sibling_margin;

            this.ConstrainLayout(() => 
                content_view.Frame.Top == this.Frame.Top + sibling_sibling_margin &&
                content_view.Frame.Left == this.Frame.Left + sibling_sibling_margin &&
                content_view.Frame.Right == this.Frame.Right - sibling_sibling_margin &&
                content_view.Frame.Bottom == this.Frame.Bottom - sibling_sibling_margin
            );

            content_view.ConstrainLayout(() => 
                user_image.Frame.Width == user_image_width &&
                user_image.Frame.Height == user_image_height &&
                user_image.Frame.Top == content_view.Frame.Top + sibling_sibling_margin &&
                user_image.Frame.Left == content_view.Frame.Left + sibling_sibling_margin &&

                user_name.Frame.Left == user_image.Frame.Right + sibling_sibling_margin &&
                user_name.Frame.Right == content_view.Frame.Right - sibling_sibling_margin &&         
                user_name.Frame.Top == content_view.Frame.Top + sibling_sibling_margin &&
                user_name.Frame.Height == user_name_height &&

                time_ago.Frame.Left == user_name.Frame.Left &&
                time_ago.Frame.Right == user_name.Frame.Right &&         
                time_ago.Frame.Top == user_name.Frame.Bottom + sibling_sibling_margin &&
                time_ago.Frame.Height == time_ago_height &&

                post_text.Frame.Left == user_name.Frame.Left &&
                post_text.Frame.Right == content_view.Frame.Right - sibling_sibling_margin &&
                post_text.Frame.Top == time_ago.Frame.Bottom + sibling_sibling_margin &&
                post_text.Frame.Bottom == content_view.Frame.Bottom - sibling_sibling_margin
            );
        }

        public void BindDataToCell(Post post)
        {
            user_name.Text = post.user_display_name;
            post_text.Text = post.text;
            user_image.Image = UIImage.FromBundle(PlaceholderImagePath);
            time_ago.Text = TimeAgoConverter.Current.Convert(post.date);
        }
    }
}

