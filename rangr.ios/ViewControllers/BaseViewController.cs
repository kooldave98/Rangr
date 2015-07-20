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

