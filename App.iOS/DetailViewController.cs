using System;
using System.Collections.Generic;
using System.Drawing;
using App.Core.Portable.Models;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using App.Common;

namespace App.iOS
{
	public partial class DetailViewController : UIViewController
	{
		PostDetailsViewModel view_model;

		public DetailViewController () 
			: base ("DetailViewController", null)
		{
			Title = NSBundle.MainBundle.LocalizedString ("Detail", "Detail");

			view_model = new PostDetailsViewModel ();
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			// Perform any additional setup after loading the view, typically from a nib.
			ConfigureView ();
		}

		public void SetDetailItem (Post newDetailItem)
		{
			if (view_model.CurrentPost != newDetailItem) {
				view_model.CurrentPost = newDetailItem;
				
				// Update the view
				ConfigureView ();
			}
		}

		private void ConfigureView ()
		{
			// Update the user interface for the detail item
			if (IsViewLoaded && view_model.CurrentPost != null) {
				detailDescriptionLabel.Text = view_model.CurrentPost.text;
			}
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}
	}
}

