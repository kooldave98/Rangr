
using System;
using System.Drawing;

using Foundation;
using UIKit;
using common_lib;

namespace experiments.ios
{
    public class ItemCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("ItemCell");

        private UILabel titleLabel;
        private UILabel bodyLabel;
        private bool didSetupConstraints;

        public ItemCell()
        {
            this.CreateView();
        }

        public ItemCell(IntPtr handle)
            : base(handle)
        {
            this.CreateView();
        }

        public void bind_data(Item item)
        {
            titleLabel.Text = item.Title;
            bodyLabel.Text = item.Body;
        }

        public override void UpdateConstraints()
        {
            base.UpdateConstraints();

            if (this.didSetupConstraints)
            {
                return;
            }

            titleLabel.SetContentCompressionResistancePriority(Layout.RequiredPriority, UILayoutConstraintAxis.Vertical);
            bodyLabel.SetContentCompressionResistancePriority(Layout.RequiredPriority, UILayoutConstraintAxis.Vertical);

            var sibling_sibling_margin = HumanInterface.sibling_sibling_margin;

            ContentView.ConstrainLayout(() =>
                titleLabel.Frame.Top == ContentView.Frame.Top + sibling_sibling_margin &&
                titleLabel.Frame.Left == ContentView.Frame.Left + sibling_sibling_margin &&
                titleLabel.Frame.Right == ContentView.Frame.Right - sibling_sibling_margin &&
                bodyLabel.Frame.Top >= titleLabel.Frame.Bottom + sibling_sibling_margin &&
                bodyLabel.Frame.Left == ContentView.Frame.Left + sibling_sibling_margin &&
                bodyLabel.Frame.Right == ContentView.Frame.Right - sibling_sibling_margin &&
                bodyLabel.Frame.Bottom == ContentView.Frame.Bottom - sibling_sibling_margin
            );

            didSetupConstraints = true;
        }

        public void UpdateFonts()
        {
            titleLabel.Font = UIFont.PreferredHeadline;
            bodyLabel.Font = UIFont.PreferredCaption2;
        }

        private void CreateView()
        {
            titleLabel = new UILabel {
                LineBreakMode = UILineBreakMode.TailTruncation,
                Lines = 1,
                TextAlignment = UITextAlignment.Left,
                TextColor = UIColor.Black,
                BackgroundColor = UIColor.FromRGBA(0, 0, 255, 30),
                TranslatesAutoresizingMaskIntoConstraints = false
            };

            bodyLabel = new UILabel {
                LineBreakMode = UILineBreakMode.TailTruncation,
                Lines = 0,
                TextAlignment = UITextAlignment.Left,
                TextColor = UIColor.DarkGray,
                BackgroundColor = UIColor.FromRGBA(255, 0, 0, 30),
                TranslatesAutoresizingMaskIntoConstraints = false
            };

            UpdateFonts();

            ContentView.AddSubviews(this.titleLabel, this.bodyLabel);

            ContentView.BackgroundColor = UIColor.FromRGBA(0, 255, 0, 30);
        }
    }
}

