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

        private UIButton start_button;

        public override void WillPopulateView()
        {
            items = new UIView[] {
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
                scroll_view = new UIScrollView() {
                TranslatesAutoresizingMaskIntoConstraints = false,
                PagingEnabled = true,
                ScrollEnabled = true,
                ShowsHorizontalScrollIndicator = false,
                ShowsVerticalScrollIndicator = false
            }
                .Init(sv => sv.Scrolled += (s, e) => page_control.CurrentPage = (int)System.Math.Floor(sv.ContentOffset.X / sv.Frame.Size.Width))
                .Init(sv=>sv.AddSubviews(items))
            );

            View.Add(page_control = new UIPageControl(){
                Pages = items.Length,
                CurrentPage = 0,
                CurrentPageIndicatorTintColor = UIColor.DarkGray,
                PageIndicatorTintColor = UIColor.LightGray
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
                scroll_view.Frame.Top == View.Bounds.Top &&
                scroll_view.Frame.Left == View.Bounds.Left &&
                scroll_view.Frame.Right == View.Bounds.Right &&
                scroll_view.Frame.Bottom == page_control.Frame.Top - sibling_sibling_margin &&

                page_control.Frame.Bottom == start_button.Frame.Top - double_parent_margin &&
                page_control.Frame.Left == View.Bounds.Left && 
                page_control.Frame.Right == View.Bounds.Right &&
                page_control.Frame.Height == double_finger_tip_diameter &&


                start_button.Frame.Height == finger_tip_diameter &&
                start_button.Frame.Width == start_button.Frame.Height * 8.0f && //Really should investigate using COntent hugging
                start_button.Frame.Bottom == View.Frame.Bottom - parent_child_margin &&
                start_button.Frame.GetCenterX() == View.Frame.GetCenterX()

            );

            for (int i = 0; i < items.Length; i++)
            {

                View.ConstrainLayout(() => 
                    items[i].Frame.Top == View.Bounds.Top + parent_child_margin &&
                    items[i].Frame.Bottom == View.Frame.Bottom - combined_parent_and_sibling_margin &&
                    items[i].Frame.Width == View.Bounds.Width - double_parent_child_margin

                );

                if (i == 0)
                {
                    scroll_view.ConstrainLayout(() => 
                        items[i].Frame.Left >= scroll_view.Frame.Left + parent_child_margin
                    );
                }

                if (i > 0)
                {
                    scroll_view.ConstrainLayout(() => 
                        items[i].Frame.Left >= items[i-1].Frame.Right + double_parent_child_margin
                    );
                }

                if (i == items.Length-1)
                {
                    scroll_view.ConstrainLayout(() => 
                        items[i].Frame.Right <= scroll_view.Frame.Right - parent_child_margin
                    );
                }

            }
        }

        private UIView[] items;

        private UIScrollView scroll_view;
        private UIPageControl page_control;
    }
}
#endif
