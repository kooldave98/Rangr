
using System;
using System.Drawing;

using Foundation;
using UIKit;
using System.Collections.Generic;
using ios_ui_lib;
using CoreGraphics;

namespace experiments.ios
{
    public class ConstraintsVC : BaseViewController
    {
        public override string TitleLabel { 
            get{ return "Constraints"; } 
        }

        private UIView centerView;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.View.BackgroundColor = UIColor.Green;

            centerView = new UIView(){
                BackgroundColor = UIColor.Red
            };

            centerView.Layer.CornerRadius = 10;
            centerView.Layer.BorderWidth = 1;
            centerView.Layer.BorderColor = UIColor.White.CGColor;

            View.AddSubview(centerView);
        }

        public override void ViewDidLayoutSubviews()
        {
            layout_with_simple_contraints();
        }

        NSLayoutConstraint[] potrait_constraints = new NSLayoutConstraint[0];
        NSLayoutConstraint[] landscape_constraints = new NSLayoutConstraint[0];

        //http://praeclarum.org/post/45690317491/easy-layout-a-dsl-for-nslayoutconstraint
        //https://gist.github.com/praeclarum/6225853
        //https://gist.github.com/praeclarum/8185036
        private void layout_with_simple_contraints()
        {
            View.ConstrainLayout(() => 
                centerView.Frame.GetCenterX() == View.Frame.GetCenterX() &&
                centerView.Frame.GetCenterY() == View.Frame.GetCenterY()
            );

            if(InterfaceOrientation == UIInterfaceOrientation.Portrait ||
                InterfaceOrientation == UIInterfaceOrientation.PortraitUpsideDown)
            {
                View.RemoveConstraints(landscape_constraints);

                potrait_constraints = View.ConstrainLayout(() => 
                    centerView.Frame.Width == View.Frame.Width * 0.5f &&
                    centerView.Frame.Height == centerView.Frame.Width
                );
            }

            if(InterfaceOrientation == UIInterfaceOrientation.LandscapeLeft ||
                InterfaceOrientation == UIInterfaceOrientation.LandscapeRight)
            {
                View.RemoveConstraints(potrait_constraints);

                landscape_constraints = View.ConstrainLayout(() => 
                    centerView.Frame.Height == View.Frame.Height * 0.5f &&
                    centerView.Frame.Width == centerView.Frame.Height
                );
            }
        }

        private void layout_with_complex_contraints()
        {
            //This is the math powering constraints, culled from apples docs.
            //attribute1 == multiplier × attribute2 + constant

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
    
        private void layout_with_struts()
        {
            centerView.Bounds = new CGRect { 
                Width = View.Bounds.Width/2, 
                Height = View.Bounds.Height/2 
            };
            centerView.CenterInParent();
        }
    
    }
}

