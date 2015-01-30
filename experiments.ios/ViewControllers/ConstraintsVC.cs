
using System;
using System.Drawing;

using Foundation;
using UIKit;

namespace experiments.ios
{
    public class ConstraintsVC : BaseViewController
    {
        public ConstraintsVC()
            : base("Constraints")
        {
        }

        public override void LoadView()
        {
            var content_view = new UIView();
            content_view.BackgroundColor = UIColor.Green;
            this.View = content_view;

            var centerView = new UIView();
            centerView.TranslatesAutoresizingMaskIntoConstraints = false;
            centerView.BackgroundColor = UIColor.Red;
            View.AddSubview(centerView);


            this.View
                .AddConstraint(NSLayoutConstraint.Create(
                    centerView, 
                    NSLayoutAttribute.Width, 
                    NSLayoutRelation.Equal, 
                    this.View,
                    NSLayoutAttribute.Width,
                    0.5f,
                    0));

            this.View
                .AddConstraint(NSLayoutConstraint.Create(
                    centerView, 
                    NSLayoutAttribute.Height, 
                    NSLayoutRelation.Equal, 
                    this.View,
                    NSLayoutAttribute.Height,
                    0.5f,
                    0));

            this.View
                .AddConstraint(NSLayoutConstraint.Create(
                    centerView, 
                    NSLayoutAttribute.CenterX, 
                    NSLayoutRelation.Equal, 
                    this.View,
                    NSLayoutAttribute.CenterX,
                    1.0f,
                    0));

            this.View
                .AddConstraint(NSLayoutConstraint.Create(
                    centerView, 
                    NSLayoutAttribute.CenterY, 
                    NSLayoutRelation.Equal, 
                    this.View,
                    NSLayoutAttribute.CenterY,
                    1.0f,
                    0));

        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
			
            // Perform any additional setup after loading the view, typically from a nib.
        }
    }
}

