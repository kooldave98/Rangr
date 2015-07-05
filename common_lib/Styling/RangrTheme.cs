#if __IOS__
using System;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;

namespace common_lib
{
    public class RangrTheme
    {
        //See James Montemagnos Post-It app for how he uses the appearance api for themeing.
        public static void Apply(string options = null)
        {
            UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.LightContent, false);

            Apply(UINavigationBar.Appearance, options);

            //            Apply(UITabBar.Appearance, options);
            //            Apply(UIToolbar.Appearance, options);
            //            Apply(UIBarButtonItem.Appearance, options);
            //            Apply(UISlider.Appearance, options);
            //            Apply(UISegmentedControl.Appearance, options);
            //            Apply(UIProgressView.Appearance, options);
            //            Apply(UISearchBar.Appearance, options);
            //            Apply(UISwitch.Appearance, options);
            //            Apply(UIRefreshControl.Appearance, options);
        }

        public static void Apply(UINavigationBar.UINavigationBarAppearance appearance, string options = null)
        {

            appearance.TintColor = UIColor.White;
            appearance.BarTintColor = Color.Blue;
            appearance.SetTitleTextAttributes(new UITextAttributes
                {
                    TextColor = UIColor.White
                });
        }

        public static void Apply(UITabBar.UITabBarAppearance appearance, string options = null)
        {

        }

        public static void Apply(UIButton.UIButtonAppearance appearance, string options = null)
        {

        }

        public static void Apply(UILabel.UILabelAppearance appearance, string options = null)
        {
        }

        public static void Apply(UITextView.UITextViewAppearance appearance, string options = null)
        {

        }

        #region"Primitive API"
        //Primitive themeing API for older devices maybe ??
        public class Primitive
        {
            public static void Apply(UIView view, string options = null)
            {
                view.BackgroundColor = UIColor.LightGray;
            }

            public static void Apply(UINavigationBar view, string options = null)
            {
                view.TintColor = UIColor.White;
                view.BarTintColor = Color.Blue;
            }

            public static void Apply(UITabBar view, string options = null)
            {
            }

            public static void Apply(UIButton view, string options = null)
            {
            }

            public static void Apply(UILabel view, string options = null)
            {
            }

            public static void Apply(UITextView view, string options = null)
            {

            }
        }
        #endregion
    }
}
#endif