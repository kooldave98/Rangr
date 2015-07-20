#if __IOS__
using System;

namespace common_lib
{
    public class HumanInterface
    {
        //http://stackoverflow.com/questions/14055900/what-constant-can-i-use-for-the-default-aqua-space-in-autolayout
        public static nfloat parent_child_margin = 20.0f;
        public static nfloat sibling_sibling_margin = 8.0f;

        public static nfloat finger_tip_diameter = 30.0f;

        public static nfloat medium_square_image_length = 50.0f;


        //Taken from Kent boogarts code
//        // the standard spacing between sibling views
//        public const int StandardSiblingViewSpacing = 8;
//
//        // half the standard spacing between sibling views
//        public const int HalfSiblingViewSpacing = StandardSiblingViewSpacing / 2;
//
//        // the standard spacing between a view and its superview
//        public const int StandardSuperviewSpacing = 20;
//
//        // half the standard spacing between superviews
//        public const int HalfSuperviewSpacing = StandardSuperviewSpacing / 2;
    }
}

#endif