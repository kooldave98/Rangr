
using System;

using Foundation;
using UIKit;

namespace experiments.ios
{
    //This controller's purpose at the time of creation 
    //was purely just to allow me test the Enter Mobile Number Sequence
    public partial class TextDisplayViewController : UIViewController
    {
        public TextDisplayViewController()
            : base("TextDisplayViewController", null)
        {
        }

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();
			
            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
			
            BeginButton.TouchUpInside += (sender, e) => {
                BeginPressed();
            };
        }

        public void set_label_text(string text)
        {
            DisplayLabel.Text = text;
        }

        public event Action BeginPressed = delegate {};
    }
}

