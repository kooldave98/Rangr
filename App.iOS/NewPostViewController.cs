using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using App.Common.Shared;
using App.Core.Portable.Device;

namespace App.iOS
{
	public partial class NewPostViewController : UIViewController
	{
//		PostRepository _postRepository;
//		IGeoLocation _geoLocationInstance;
//		ISession _session;
		Global _global;


		public NewPostViewController () 
		: base ("NewPostViewController", null)
		{
			_global = Global.Current;
//			_geoLocationInstance = geolocationInstance;
//			_postRepository = postRepository;
//			_session = session;
		}

		/// <summary>
		/// This is our common action handler. Two buttons call this via an action method.
		/// </summary>
		partial void  btnSend (MonoTouch.Foundation.NSObject sender)
		{
			var postText = this.tbxPost.Text;
//			var geoLocationString = await _geoLocationInstance.GetCurrentPosition();

			_global.client.sendPost (async(hubProxy) => {
				await hubProxy.Invoke ("sendPost", postText);
			});



			//await _postRepository(postText, _session.GetCurrentUser().ID.ToString(), geoLocationString);
			this.NavigationController.PopToRootViewController(true);

		}

//		public void SetDetailItem (object newDetailItem)
//		{
//			if (detailItem != newDetailItem) {
//				detailItem = newDetailItem;
//
//				// Update the view
//				ConfigureView ();
//			}
//		}

//		void ConfigureView ()
//		{
//			// Update the user interface for the detail item
//			if (IsViewLoaded && detailItem != null)
//				detailDescriptionLabel.Text = detailItem.ToString ();
//		}

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
			//ConfigureView ();
		}
	}
}

