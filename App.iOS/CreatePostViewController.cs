using System;
using System.Drawing;
using App.Common.Shared;
using App.Core.Portable.Device;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace App.iOS
{
	public partial class CreatePostViewController : UIViewController
	{
		Posts post_services;
		Global _global;
		private Action success_callback;

		public CreatePostViewController (Action the_success_callback) : base ("CreatePostViewController", null)
		{
			_global = Global.Current;
			post_services = new Posts(HttpRequest.Current);
			success_callback = the_success_callback;
		}

		public void ConfigureView ()
		{						
			this.CreatePostBtn.TouchUpInside += async delegate {
				var postText = this.NewPostTbx.Text;
						
				await post_services.Create(postText, _global.current_connection.connection_id.ToString());		

				success_callback();

				this.NavigationController.PopToRootViewController (true);
			};
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

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			// Perform any additional setup after loading the view, typically from a nib.
			ConfigureView ();
		}
	}
}

