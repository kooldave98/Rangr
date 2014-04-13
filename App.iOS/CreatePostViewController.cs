using System;
using System.Drawing;
using App.Common;
using App.Common.Shared;
using App.Core.Portable.Device;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace App.iOS
{
	public partial class CreatePostViewController : UIViewController
	{
		private NewPostViewModel view_model;
		private Action success_callback;

		public CreatePostViewController (Action the_success_callback) 
				: base ("CreatePostViewController", null)
		{
			view_model = new NewPostViewModel (PersistentStorage.Current);
			success_callback = the_success_callback;
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			// Perform any additional setup after loading the view, typically from a nib.
			this.CreatePostBtn.TouchUpInside += Login;
		}

		private async void Login (object sender, EventArgs e)
		{
			if (!string.IsNullOrWhiteSpace (NewPostTbx.Text)) 
			{
				view_model.PostText = this.NewPostTbx.Text;

				await view_model.CreatePost ();		

				success_callback ();

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

