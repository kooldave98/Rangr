#if __IOS__
using System;
using UIKit;
using common_lib;

namespace ios_ui_lib
{
    public class SimpleStartScreenController : SimpleViewController
    {
        public override string TitleLabel {
            get { return "Welcome"; }
        }

        public event Action OnStartApp = delegate {};

        public override void WillPopulateView()
        {
            var items = new UIView[] {
                new UIView(){ BackgroundColor = UIColor.Blue, TranslatesAutoresizingMaskIntoConstraints = false },
                new UIView(){ BackgroundColor = UIColor.Red, TranslatesAutoresizingMaskIntoConstraints = false },
                new UIView(){ BackgroundColor = UIColor.Green, TranslatesAutoresizingMaskIntoConstraints = false },
                new UIView(){ BackgroundColor = UIColor.Yellow, TranslatesAutoresizingMaskIntoConstraints = false },
                new UIView(){ BackgroundColor = UIColor.Orange, TranslatesAutoresizingMaskIntoConstraints = false },
                new UIView(){ BackgroundColor = UIColor.Purple, TranslatesAutoresizingMaskIntoConstraints = false },
                new UIView(){ BackgroundColor = UIColor.DarkGray, TranslatesAutoresizingMaskIntoConstraints = false },
                new UIView(){ BackgroundColor = UIColor.Magenta, TranslatesAutoresizingMaskIntoConstraints = false },
                new UIView(){ BackgroundColor = UIColor.Black, TranslatesAutoresizingMaskIntoConstraints = false },
                new UIView(){ BackgroundColor = UIColor.LightGray, TranslatesAutoresizingMaskIntoConstraints = false },
                new UIView(){ BackgroundColor = UIColor.Brown, TranslatesAutoresizingMaskIntoConstraints = false }
            };


            View.Add(
                swipe_view = new SwipePagerView(items) {
                TranslatesAutoresizingMaskIntoConstraints = false
            });



            start_button = UIButton.FromType(UIButtonType.RoundedRect);
            //see this for arrow beside button :http://stackoverflow.com/a/20821178/502130
            start_button.SetTitle("Start Keeping App", UIControlState.Normal);
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
                swipe_view.Frame.Top == View.Bounds.Top &&
                swipe_view.Frame.Left == View.Bounds.Left &&
                swipe_view.Frame.Right == View.Bounds.Right &&
                swipe_view.Frame.Bottom == start_button.Frame.Top - double_parent_margin &&


                start_button.Frame.Height == finger_tip_diameter &&
                start_button.Frame.Width == start_button.Frame.Height * 8.0f && //Really should investigate using COntent hugging
                start_button.Frame.Bottom == View.Frame.Bottom - parent_child_margin &&
                start_button.Frame.GetCenterX() == View.Frame.GetCenterX()

            );


        }

        private SwipePagerView swipe_view;

        private UIButton start_button;
    }
}
#endif
