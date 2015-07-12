#if __IOS__
using System;
using UIKit;

namespace common_lib
{
    public class SimpleStartScreenController : SimpleViewController
    {
        public override string TitleLabel {
            get { return "Welcome"; }
        }

        public event Action OnStartApp = delegate {};

        private UIView card_view;
        private UIButton start_button;

        public override void WillPopulateView()
        {

            View.AddSubview(card_view = new UIView(){
                TranslatesAutoresizingMaskIntoConstraints = false,
                BackgroundColor = UIColor.Gray
            });

            card_view.Layer.CornerRadius = 5;
            card_view.Layer.MasksToBounds = true;

            start_button = UIButton.FromType(UIButtonType.RoundedRect);
            start_button.SetTitle("Start Keeping App >", UIControlState.Normal);
            start_button.TranslatesAutoresizingMaskIntoConstraints = false;
            start_button.TouchUpInside += delegate {
                OnStartApp();
            };
            View.AddSubview(start_button);
        }

        public override void WillAddConstraints()
        {
            var double_parent_margin = parent_child_margin * 2;

            View.ConstrainLayout(() => 
                card_view.Frame.Top == View.Bounds.Top + double_parent_margin &&
                card_view.Frame.Left == View.Bounds.Left + sibling_sibling_margin &&
                card_view.Frame.Right == View.Bounds.Right - sibling_sibling_margin &&
                card_view.Frame.Bottom == start_button.Frame.Top - double_parent_margin &&

                start_button.Frame.Height == finger_tip_diameter &&
                start_button.Frame.Width == start_button.Frame.Height * 8.0f && //Really should investigate using COntent hugging
                start_button.Frame.Bottom == View.Frame.Bottom - parent_child_margin &&
                start_button.Frame.GetCenterX() == View.Frame.GetCenterX()

            );
        }


    }
}
#endif
