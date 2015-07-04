using System;
using UIKit;
using App.Common;
using BigTed;
using Foundation;
using CoreGraphics;
using common_lib;
using ios_ui_lib;

namespace rangr.ios
{
    /// <summary>
    /// Consider deriving from SimpleViewController
    /// where trivial things like alerts etc are handled
    /// </summary>
    public abstract class BaseViewModelController<VM> : SimpleViewController where VM : ViewModelBase
    {
        protected VM view_model;

        public override void LoadView()
        {
            base.LoadView();

            Guard.IsNotNull(view_model, "view_model");

            RangrTheme.Primitive.Apply(View);
        }
    }
}

