using System;
using System.Collections.Generic;
using CoreGraphics;
using System.Linq;
using System.Threading;
using Foundation;
using UIKit;
using System.Threading.Tasks;
using App.Common;

namespace App.iOS
{
    public partial class MainViewController : UITableViewController
    {
        FeedViewModel view_model;

        public DetailViewController DetailViewController { get; set; }

        public CreatePostViewController CreatePostViewController { get; set; }

        public MainViewController()
            : base("MainViewController", null)
        {
            view_model = new FeedViewModel();
				
            Title = NSBundle.MainBundle.LocalizedString("Main", "Main");

            TableView.Source = new PostsTableViewSource(this, view_model.Posts);

            Initialize();
        }

        public void Initialize()
        {

            NavigationItem.SetRightBarButtonItem(new UIBarButtonItem(UIBarButtonSystemItem.Add), false);
            NavigationItem.RightBarButtonItem.Clicked += (sender, e) =>
            {
                //if (this.NewPostViewController == null)
                this.CreatePostViewController = new CreatePostViewController();
                //this.NewPostViewController.ModalInPopover = true;

                // Pass the selected object to the new view controller.
                this.NavigationController.PushViewController(this.CreatePostViewController, true);

            };


            NavigationItem.SetLeftBarButtonItem(new UIBarButtonItem(UIBarButtonSystemItem.Action), false);
            NavigationItem.LeftBarButtonItem.Clicked += (object sender, EventArgs e) =>
            {
                //alert hey oh
            };

            view_model.OnNewPostsReceived += HandleOnNewPostsReceived;

            RefreshControl = new UIRefreshControl();

            RefreshControl.ValueChanged += async (object sender, EventArgs e) =>
            {
                UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;

                await view_model.RefreshPosts();

                RefreshControl.EndRefreshing();
                UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
            };

        }

        private void HandleOnNewPostsReceived(object sender, EventArgs e)
        {
            TableView.ReloadData();
        }

        public override async void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;
            await view_model.RefreshPosts();
            UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;

        }

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.

            showModal();
        }

        private void showModal()
        {
            UIAlertView alert = new UIAlertView();
            alert.Title = "Add Something";
            alert.AddButton("One");
            alert.Message = "Low Memory o";
            alert.AlertViewStyle = UIAlertViewStyle.PlainTextInput;//UIAlertViewStyle.LoginAndPasswordInput;
            alert.Show();

        }
    }
}
