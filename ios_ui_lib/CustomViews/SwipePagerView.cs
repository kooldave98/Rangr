using System;
using UIKit;
using common_lib;
using Foundation;

namespace ios_ui_lib
{
    public class SwipePagerView : SimpleUIView
    {
        public override void WillPopulateView()
        {
            Add(
                    scroll_view = new UIScrollView() {
                        TranslatesAutoresizingMaskIntoConstraints = false,
                        PagingEnabled = true,
                        ScrollEnabled = true
                    }
                    .Init(sv => sv.Scrolled += (s, e) => page_control.CurrentPage = (int)System.Math.Floor(sv.ContentOffset.X / sv.Frame.Size.Width))
                    .Init(sv=>sv.AddSubview(first))
                    .Init(sv=>sv.AddSubview(second))
            );

            Add(page_control = new UIPageControl(){
                Pages = 2,
                CurrentPage = 0
            });

        }

        public override void WillAddConstraints()
        {
            this.ConstrainLayout(() => 
                scroll_view.Frame.Top == this.Bounds.Top &&
                scroll_view.Frame.Left == this.Bounds.Left &&
                scroll_view.Frame.Right == this.Bounds.Right &&
                scroll_view.Frame.Bottom == this.Bounds.Bottom //page_control.Frame.Top &&

//                page_control.Frame.Bottom == this.Bounds.Bottom &&
//                page_control.Frame.Left == this.Bounds.Left && 
//                page_control.Frame.Right == this.Bounds.Right &&
//                page_control.Frame.Height == double_finger_tip_diameter
            && 
                first.Frame.Top == this.Bounds.Top + parent_child_margin &&
                first.Frame.Bottom == scroll_view.Frame.Height - parent_child_margin && //Watch out for this constraint, it is using the scroll view
                first.Frame.Width == this.Bounds.Width - double_parent_child_margin

                &&

                second.Frame.Top == this.Bounds.Top + parent_child_margin &&
                second.Frame.Bottom == scroll_view.Frame.Height - parent_child_margin && //Watch out for this constraint, it is using the scroll view
                second.Frame.Width == this.Bounds.Width - double_parent_child_margin

            );

            scroll_view.ConstrainLayout(() => 
                first.Frame.Left >= scroll_view.Frame.Left + parent_child_margin

                &&
                second.Frame.Left >= first.Frame.Right + double_parent_child_margin
                &&

                second.Frame.Right <= scroll_view.Frame.Right - parent_child_margin
            );



//            for (int i = 0; i < items.Length; i++)
//            {
//                var item = items[i];
//
//                this.ConstrainLayout(() => 
//                    item.Frame.Top == this.Bounds.Top + parent_child_margin &&
//                    item.Frame.Bottom == scroll_view.Frame.Height - parent_child_margin && //Watch out for this constraint, it is using the scroll view
//                    item.Frame.Width == this.Bounds.Width - double_parent_child_margin
//
//                );
//
//                if (i == 0)
//                {
//                    scroll_view.ConstrainLayout(() => 
//                        item.Frame.Left >= scroll_view.Frame.Left + parent_child_margin
//                    );
//                }
//
//                if (i > 0)
//                {
//                    var previous_item = items[i-1];
//                    scroll_view.ConstrainLayout(() => 
//                        item.Frame.Left >= previous_item.Frame.Right + double_parent_child_margin
//                    );
//                }
//
//                if (i == items.Length-1)
//                {
//                    scroll_view.ConstrainLayout(() => 
//                        item.Frame.Right <= scroll_view.Frame.Right - parent_child_margin
//                    );
//                }
//
//            }
        }

        [Export ("requiresConstraintBasedLayout")]
        public static new bool RequiresConstraintBasedLayout () {
            return true;
        }

        public SwipePagerView()
        {
            first = new UIView(){ BackgroundColor = UIColor.Blue };
            second = new UIView(){ BackgroundColor = UIColor.Brown };

            base.InitSimpleUIView();
        }

        private UIView first;

        private UIView second;


        private UIScrollView scroll_view;
        private UIPageControl page_control;
    }
}

