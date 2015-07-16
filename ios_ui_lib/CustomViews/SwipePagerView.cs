using System;
using UIKit;
using common_lib;
using Foundation;

namespace ios_ui_lib
{
    /// <summary>
    /// Swipe pager view.
    /// see: http://www.gooorack.com/2013/07/31/xamarin-uipagecontrol-and-uiscrollview-for-ios-homescreen-look/?subscribe=success#blog_subscription-2
    /// </summary>
    public class SwipePagerView : SimpleUIView
    {
        public void WillPopulateView()
        {
            Add(
                    scroll_view = new UIScrollView() {
                        TranslatesAutoresizingMaskIntoConstraints = false,
                        PagingEnabled = false,//hmmn,should this be exposed as properties of the view ??
                        ScrollEnabled = true,
                        ShowsHorizontalScrollIndicator = false,
                        ShowsVerticalScrollIndicator = false,
                        AlwaysBounceHorizontal = true
                    }
                    .Init(sv => sv.Scrolled += (s, e) => page_control.CurrentPage = (int)System.Math.Floor(sv.ContentOffset.X / sv.Frame.Size.Width))
                    .Init(sv=>sv.AddSubviews(items))                    
            );

            Add(page_control = new UIPageControl(){
                Pages = items.Length,
                CurrentPage = 0,
                CurrentPageIndicatorTintColor = UIColor.DarkGray,
                PageIndicatorTintColor = UIColor.LightGray
            });

        }

        public override void WillAddConstraints()
        {
            this.ConstrainLayout(() => 
                scroll_view.Frame.Top == this.Bounds.Top &&
                scroll_view.Frame.Left == this.Bounds.Left &&
                scroll_view.Frame.Right == this.Bounds.Right &&
                scroll_view.Frame.Bottom == page_control.Frame.Top - sibling_sibling_margin &&

                page_control.Frame.Bottom == this.Bounds.Bottom &&
                page_control.Frame.Left == this.Bounds.Left && 
                page_control.Frame.Right == this.Bounds.Right &&
                page_control.Frame.Height == double_finger_tip_diameter
            );


            for (int i = 0; i < items.Length; i++)
            {
                var item = items[i];

                this.ConstrainLayout(() => 
                    item.Frame.Top == this.Bounds.Top + parent_child_margin &&
                    item.Frame.Bottom == this.Frame.Bottom - combined_parent_and_sibling_margin &&
                    item.Frame.Width == this.Bounds.Width - double_parent_child_margin

                );

                if (i == 0)
                {
                    scroll_view.ConstrainLayout(() => 
                        item.Frame.Left >= scroll_view.Frame.Left + parent_child_margin
                    );
                }

                if (i > 0)
                {
                    var previous_item = items[i-1];
                    scroll_view.ConstrainLayout(() => 
                        item.Frame.Left >= previous_item.Frame.Right + double_parent_child_margin
                    );
                }

                if (i == items.Length-1)
                {
                    scroll_view.ConstrainLayout(() => 
                        item.Frame.Right <= scroll_view.Frame.Right - parent_child_margin
                    );
                }

            }
        }

        [Export ("requiresConstraintBasedLayout")]
        public static new bool RequiresConstraintBasedLayout () {
            return true;
        }

        public SwipePagerView(UIView[] the_items)
        {
            items = Guard.IsNotNull(the_items, "the_items");

            WillPopulateView();
        }

        private UIView[] items;


        private UIScrollView scroll_view;
        private UIPageControl page_control;
    }
}

