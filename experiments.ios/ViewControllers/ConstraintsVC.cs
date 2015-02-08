
using System;
using System.Drawing;

using Foundation;
using UIKit;
using System.Collections.Generic;

namespace experiments.ios
{
    public class ConstraintsVC : BaseViewController
    {
        public ConstraintsVC()
            : base("Constraints")
        {
        }

        private UIView centerView;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.View.BackgroundColor = UIColor.Green;

            centerView = new UIView()
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                BackgroundColor = UIColor.Red
            };

            View.AddSubview(centerView);
        }

        public override void ViewDidLayoutSubviews()
        {
            centerView.Bounds = new RectangleF { Width = 50, Height = 50 };
            centerView.CenterInParent();
        }

        private void contraints_shizzle()
        {

            //            var viewNames = NSDictionary.FromObjectsAndKeys(new NSObject[]
            //                {
            //                    centerView
            //                }, 
            //                                new NSObject[]
            //                { 
            //                    new NSString("centerView")
            //                });
            //            var emptyDict = new NSDictionary();
            //
            //
            //            this.View.AddConstraint(
            //                NSLayoutConstraint.FromVisualFormat(
            //                    "H:|-25-[centerView(10)]-25-|",
            //                    0,
            //                    emptyDict, 
            //                    viewNames)[0]
            //            );
            //
            //            this.View.AddConstraint(
            //                NSLayoutConstraint.FromVisualFormat(
            //                    "V:|-25-[centerView(10)]-25-|",
            //                    0,
            //                    emptyDict, 
            //                    viewNames)[0]
            //            );
            //see
            //https://forums.xamarin.com/discussion/8717/can-possible-use-nslayoutconstraint-in-ios
            //and
            //https://developer.apple.com/library/ios/documentation/UserExperience/Conceptual/AutolayoutPG/VisualFormatLanguage/VisualFormatLanguage.html#//apple_ref/doc/uid/TP40010853-CH3-SW1


            //            this.View
            //                .AddConstraint(NSLayoutConstraint.Create(
            //                    centerView, 
            //                    NSLayoutAttribute.Width, 
            //                    NSLayoutRelation.Equal, 
            //                    this.View,
            //                    NSLayoutAttribute.Width,
            //                    0.5f,
            //                    0));
            //
            //            this.View
            //                .AddConstraint(NSLayoutConstraint.Create(
            //                    centerView, 
            //                    NSLayoutAttribute.Height, 
            //                    NSLayoutRelation.Equal, 
            //                    this.View,
            //                    NSLayoutAttribute.Height,
            //                    0.5f,
            //                    0));
            //
            //            this.View
            //                .AddConstraint(NSLayoutConstraint.Create(
            //                    centerView, 
            //                    NSLayoutAttribute.CenterX, 
            //                    NSLayoutRelation.Equal, 
            //                    this.View,
            //                    NSLayoutAttribute.CenterX,
            //                    1.0f,
            //                    0));
            //
            //            this.View
            //                .AddConstraint(NSLayoutConstraint.Create(
            //                    centerView, 
            //                    NSLayoutAttribute.CenterY, 
            //                    NSLayoutRelation.Equal, 
            //                    this.View,
            //                    NSLayoutAttribute.CenterY,
            //                    1.0f,
            //                    0));

        }
    }
}

