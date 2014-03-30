using System;
using System.Collections.Generic;
using System.Drawing;
using App.Core.Portable.Models;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace App.iOS
{
	public partial class DetailViewController : UIViewController
	{
		SeenPost detailItem;

		public DetailViewController () : base ("DetailViewController", null)
		{
			Title = NSBundle.MainBundle.LocalizedString ("Detail", "Detail");

			// Custom initialization
		}

		public void SetDetailItem (SeenPost newDetailItem)
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
				detailDescriptionLabel.Text = detailItem.text;
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

