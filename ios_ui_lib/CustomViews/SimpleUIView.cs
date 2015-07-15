using System;
using UIKit;
using Foundation;
using common_lib;

namespace ios_ui_lib
{
    public abstract class SimpleUIView : UIView
    {
        public SimpleUIView()
        {
            have_constraints_been_added = false;

            BackgroundColor = UIColor.White;
        }

        protected nfloat parent_child_margin = HumanInterface.parent_child_margin;

        protected nfloat double_parent_child_margin = HumanInterface.parent_child_margin * 2;

        protected nfloat sibling_sibling_margin = HumanInterface.sibling_sibling_margin;

        protected nfloat double_sibling_sibling_margin = HumanInterface.sibling_sibling_margin * 2;

        protected nfloat finger_tip_diameter = HumanInterface.finger_tip_diameter;

        protected nfloat double_finger_tip_diameter = HumanInterface.finger_tip_diameter * 2;

        protected nfloat combined_parent_and_sibling_margin = HumanInterface.sibling_sibling_margin + HumanInterface.parent_child_margin;

        public abstract void WillAddConstraints();

        public virtual void WillUpdateConstraints() {}

        public override void UpdateConstraints()
        {
            if (!have_constraints_been_added)
            {
                WillAddConstraints();

                have_constraints_been_added = true;
            }
            else
            {
                WillUpdateConstraints();
            }

            base.UpdateConstraints();
        }

        protected bool have_constraints_been_added { get; set;}
    }
}

