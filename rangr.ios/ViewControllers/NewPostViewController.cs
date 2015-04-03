using System;
using CoreGraphics;
using App.Common;
using Foundation;
using UIKit;
using System.Drawing;
using ios_ui_lib;
using System.Runtime.InteropServices;

namespace rangr.ios
{
    public partial class NewPostViewController : BaseViewModelController<NewPostViewModel>
    {
        private const string PlaceholderImagePath = "user-default-avatar.png";

        private UIView card_view;
        private UIImageView user_image;
        private UITextView post_text;
        private TopAlignedImageView selected_image;


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

            create_view();
        }

        private void create_view()
        {
            View.BackgroundColor = UIColor.LightGray;

            View.AddSubview(card_view = new UIView(){
                TranslatesAutoresizingMaskIntoConstraints = false,
                BackgroundColor = UIColor.White
            });

            card_view.Layer.CornerRadius = 5;
            card_view.Layer.MasksToBounds = true;

            card_view.AddSubview(user_image = new UIImageView(){
                TranslatesAutoresizingMaskIntoConstraints = false
            });

            card_view.AddSubview(post_text = new UITextView(){
                TextColor = UIColor.Black,
                Font = UIFont.PreferredSubheadline,
                TextAlignment = UITextAlignment.Left,
                TranslatesAutoresizingMaskIntoConstraints = false
            });

            card_view.AddSubview(selected_image = new TopAlignedImageView {
                ClipsToBounds = true,
            });

        }

        public override void ViewDidLayoutSubviews()
        {
            var user_image_width = HumanInterface.medium_square_image_length;
            var user_image_height = HumanInterface.medium_square_image_length;

            View.ConstrainLayout(() => 
                card_view.Frame.Top == View.Bounds.Top + sibling_sibling_margin &&
                card_view.Frame.Left == View.Bounds.Left + sibling_sibling_margin &&
                card_view.Frame.Right == View.Bounds.Right - sibling_sibling_margin &&
                card_view.Frame.Bottom == View.Bounds.Bottom - sibling_sibling_margin
            );

            card_view.ConstrainLayout(() => 
                user_image.Frame.Width == user_image_width &&
                user_image.Frame.Height == user_image_height &&
                user_image.Frame.Top == card_view.Frame.Top + sibling_sibling_margin &&
                user_image.Frame.Left == card_view.Frame.Left + sibling_sibling_margin &&

                post_text.Frame.Left == user_image.Frame.Right + sibling_sibling_margin &&
                post_text.Frame.Right == card_view.Frame.Right - sibling_sibling_margin &&
                post_text.Frame.Top == card_view.Frame.Top + sibling_sibling_margin &&
                post_text.Frame.Bottom == user_image.Frame.Bottom &&

                selected_image.Frame.Top == post_text.Frame.Bottom + parent_child_margin &&
                selected_image.Frame.Left == card_view.Frame.Left &&
                selected_image.Frame.Right == card_view.Frame.Right &&
                selected_image.Frame.Bottom == card_view.Frame.Bottom

            );
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            post_text.BecomeFirstResponder();
        }

        public void BindDataToView(UIImage image)
        {
            user_image.Image = UIImage.FromBundle(PlaceholderImagePath);
            selected_image.Image = image;
        }

        public async void Save(object sender, EventArgs e)
        {
            if(selected_image == null)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(post_text.Text))
            {
                return;
            }

            view_model.PostImage = prepare_image(selected_image.Image);

            view_model.PostText = post_text.Text;

            await view_model.CreatePost();

            CreatePostSucceeded();
            
        }

        private HttpFile prepare_image(UIImage image)
        {
            byte[] bytes;
            using (var imageData = image.AsJPEG())
            {
                bytes = new byte[imageData.Length];
                Marshal.Copy(imageData.Bytes, bytes, 0, Convert.ToInt32(imageData.Length));
            }
            return new HttpFile("photo","image/jpg", bytes); 
        }
    }
}

