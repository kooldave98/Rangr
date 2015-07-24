
using System;

using Foundation;
using UIKit;

namespace ios_ui_lib
{
    //This is a simple xib-based UIViewController for purely displaying text.
    //Might adapt it into a sort of console for dumping runtime log data.
    //See that initial SignalR sample code that exclusively used something like this
    //for showing that signalr can work on mono/xamarin
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

