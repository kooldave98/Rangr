using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using App.Common.Shared;
using App.Core.Portable.Device;
using App.Core.Portable.Models;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using App.Core.Portable.Network;
using System.Threading.Tasks;
using App.Common;

namespace App.iOS
{
	public partial class MainViewController : UITableViewController
	{
		FeedViewModel view_model;

		PostsTableViewSource dataSource;

		public MainViewController () : base ("MainViewController", null)
		{
			view_model = new FeedViewModel(GeoLocation.GetInstance (), PersistentStorage.Current);
				
			Title = NSBundle.MainBundle.LocalizedString ("Main", "Main");

			TableView.Source = dataSource = new PostsTableViewSource (this);
		}

		public DetailViewController DetailViewController { get; set; }

		public CreatePostViewController CreatePostViewController { get; set; }

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();

			// Release any cached data, images, etc that aren't in use.

			showModal ();
		}

		public async void Initialize ()
		{
			NavigationItem.SetRightBarButtonItem (new UIBarButtonItem (UIBarButtonSystemItem.Add), false);
			NavigationItem.RightBarButtonItem.Clicked += (sender, e) => {
				//if (this.NewPostViewController == null)
				this.CreatePostViewController = new CreatePostViewController (async() => {
					await view_model.RefreshPosts ();
				});
				//this.NewPostViewController.ModalInPopover = true;

				// Pass the selected object to the new view controller.
				this.NavigationController.PushViewController (this.CreatePostViewController, true);

			};


			NavigationItem.SetLeftBarButtonItem (new UIBarButtonItem (UIBarButtonSystemItem.Action), false);
			NavigationItem.LeftBarButtonItem.Clicked += (object sender, EventArgs e) => {
				//alert hey oh
			};

			view_model.IsBusyChanged +=	(sender, e) => {
				if (view_model.IsBusy) {
					//Show startup progress here
				} else {
					//stop progress
				}
			};

			view_model.OnNewPostsReceived += HandleOnNewPostsReceived;

			RefreshControl = new UIRefreshControl ();

			RefreshControl.ValueChanged += async (object sender, EventArgs e) => {
				UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;

				await view_model.RefreshPosts ();

				RefreshControl.EndRefreshing ();
				UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
			};

			await view_model.init ();

		}

		private void HandleOnNewPostsReceived (object sender, EventArgs e)
		{
			foreach (var post in view_model.LatestPosts) {

				dataSource.Objects.Insert (0, post);

				using (var indexPath = NSIndexPath.FromRowSection (0, 0)) {
					TableView.InsertRows (new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Automatic);
				}
			}
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
		}

		private void showModal ()
		{
			UIAlertView alert = new UIAlertView ();
			alert.Title = "Add Something";
			alert.AddButton ("One");
			alert.AddButton ("Two");
			alert.AddButton ("Three");
			alert.Message = "Low Memory o";
			alert.AlertViewStyle = UIAlertViewStyle.PlainTextInput;//UIAlertViewStyle.LoginAndPasswordInput;
			alert.Show ();

		}
	}
}
