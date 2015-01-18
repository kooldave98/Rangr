using System;
using CoreGraphics;
using App.Common;
using Foundation;
using UIKit;

namespace App.iOS
{
	public partial class CreatePostViewController : UIViewController
	{
		private NewPostViewModel view_model;

		public CreatePostViewController ()
			: base ("CreatePostViewController", null)
		{
			view_model = new NewPostViewModel ();
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			// Perform any additional setup after loading the view, typically from a nib.
			this.CreatePostBtn.TouchUpInside += Save;
		}

		private async void Save (object sender, EventArgs e)
		{
			if (!string.IsNullOrWhiteSpace (NewPostTbx.Text)) {
				view_model.PostText = this.NewPostTbx.Text;

				await view_model.CreatePost ();

				this.NavigationController.PopToRootViewController (true);
			}
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();

			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
		}


	}
}

