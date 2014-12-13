
using System;
using System.Drawing;
using App.Common;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace App.iOS
{
	public partial class StreamViewController : UIViewController
	{
		AppDelegate del;

		public StreamViewController () : base ("StreamViewController", null)
		{
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			del = UIApplication.SharedApplication.Delegate as AppDelegate;

			//show navigation bar
			del.NavigationController.SetNavigationBarHidden (false, false);

			//navigation bar design
			NavigationBarSetup ();
		}

		void NavigationBarSetup() 
		{
			//hide back button
			this.NavigationItem.HidesBackButton = true;
			this.NavigationItem.Title = "walkr";

			//add post button to navigationItem (right)
			UIBarButtonItem composeBarButton = new UIBarButtonItem (UIBarButtonSystemItem.Compose, composeClick);
			this.NavigationItem.RightBarButtonItem = composeBarButton;
		}

		void composeClick(object sender, EventArgs e)
		{
			//go to compose view
			del.NavigationController.PushViewController (new ComposeViewController (), true);
		}
	}
}

