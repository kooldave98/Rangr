
using System;

using Foundation;
using UIKit;

namespace common_lib
{
    //This controller's purpose at the time of creation 
    //was purely just to allow me test the Enter Mobile Number Sequence
    public partial class TextDisplayViewController : UIViewController
    {
        public TextDisplayViewController()
            : base("TextDisplayViewController", null)
        {
        }

        public void set_label_text(string text)
        {
            DisplayLabel.Text = text;
        }
    }
}

