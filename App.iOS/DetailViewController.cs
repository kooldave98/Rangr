using System;
using System.Drawing;
using System.Collections.Generic;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using App.Core.Portable.Models;

namespace App.iOS
{
	public partial class DetailViewController : UIViewController
	{
		Post detailItem;

		public DetailViewController () : base ("DetailViewController", null)
		{
			Title = NSBundle.MainBundle.LocalizedString ("Detail", "Detail");

			// Custom initialization
		}

		public void SetDetailItem (Post newDetailItem)
		{
			if (detailItem != newDetailItem) {
				detailItem = newDetailItem;
				
				// Update the view
				ConfigureView ();
			}
		}

		void ConfigureView ()
		{
			// Update the user interface for the detail item
			if (IsViewLoaded && detailItem != null)
				detailDescriptionLabel.Text = detailItem.Text;
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
			
			// Perform any additional setup after loading the view, typically from a nib.
			ConfigureView ();
		}
	}
}

