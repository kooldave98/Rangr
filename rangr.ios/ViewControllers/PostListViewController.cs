
using System;
using System.Drawing;

using Foundation;
using UIKit;
using App.Common;
using System.Collections.Generic;

namespace rangr.ios
{
    /// <summary>
    ///see below for how to manually embed a refresh control
    /// https://github.com/vandadnp/iOS-8-Swift-Programming-Cookbook/blob/master/chapter-tables/Displaying%20a%20Refresh%20Control%20for%20Table%20Views/Displaying%20a%20Refresh%20Control%20for%20Table%20Views/ViewController.swift
    /// </summary>
    public partial class PostListViewController : BaseViewModelController<FeedViewModel>
    {
        public override string TitleLabel
        {
            get
            {
                return "Feed";
            }
        }

        public PostListViewController()
        {
            view_model = new FeedViewModel();
            view_model.OnNewPostsReceived += (sender, e) =>
            {
                TableView.ReloadData();
            };
        }


        private UIRefreshControl RefreshControl;
        public UITableView TableView;

        public override void LoadView()
        {
            base.LoadView();
            View.AddSubview(TableView = new UITableView(View.Bounds));
        }

        public override void ViewDidLoad()
        {
            // Perform any additional setup after loading the view, typically from a nib.
            base.ViewDidLoad();

            //NavigationItem.HidesBackButton = true;

            NavigationItem.SetRightBarButtonItem(new UIBarButtonItem(UIBarButtonSystemItem.Add), false);
            NavigationItem.RightBarButtonItem.Clicked += (sender, e) =>
            {
                NewPostSelected();

            };

            TableView.Source = new PostsTableViewSource(this, view_model.Posts);


            RefreshControl = new UIRefreshControl();

            RefreshControl.ValueChanged += async (object sender, EventArgs e) =>
            {
                UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;

                await view_model.RefreshPosts();

                RefreshControl.EndRefreshing();
                UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
            };


            TableView.AddSubview(RefreshControl);

        }


        public override async void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            if (AppGlobal.Current.CurrentUserAndConnectionExists)
            {
                UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;

                await view_model.RefreshPosts();
            
                UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
            }
        }

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();
			
            // Release any cached data, images, etc that aren't in use.
        }

        public void ItemSelected(Post selected_post)
        {
            PostItemSelected(selected_post);
        }


        public event Action<Post> PostItemSelected = delegate {};
        public event Action NewPostSelected = delegate {};
        //        public event Action<string> HashTagSelected = delegate {};
    }

    public class PostsTableViewSource : UITableViewSource
    {
        readonly IList<Post> objects;
        readonly PostListViewController controller;

        public PostsTableViewSource(PostListViewController controller, IList<Post> objects)
        {
            this.controller = controller;
            this.objects = objects;
        }

        // Customize the number of sections in the table view.
        public override nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return objects.Count;
        }

        //        public override void WillDisplay(UITableView tableView, UITableViewCell cell, NSIndexPath indexPath)
        //        {
        //            //This should be interesting
        //            //http://www.thinkandbuild.it/animating-uitableview-cells/
        //        }

        // Customize the appearance of table view cells.
        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var post = objects[indexPath.Row];

            var cell = tableView.DequeueReusableCell(PostCellView.Key) as PostCellView;

            if (cell == null)
            {
                cell = PostCellView.Create();
                cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
            }

            cell.BindDataToCell(post);


            //          if (cell == null) {
            //              cell = new UITableViewCell (UITableViewCellStyle.Subtitle, CellIdentifier);
            //
            //              cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
            //          }

            //          cell.TextLabel.Text = objects[indexPath.Row].UserDisplayName;
            //          cell.DetailTextLabel.Text = objects[indexPath.Row].Text;
            //          cell.ImageView.Image = UIImage.FromBundle (PlaceholderImagePath);
            //cell.ImageView.Image = UIImage.FromFile("Images/"+tableItems[indexPath.Row].ImageName); // don't use for Value2


            return cell;
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return 90;
        }

        public override bool CanEditRow(UITableView tableView, NSIndexPath indexPath)
        {
            // Return false if you do not want the specified item to be editable.
            return false;
        }

        public override void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
        {
            if (editingStyle == UITableViewCellEditingStyle.Delete)
            {
                // Delete the row from the data source.
                objects.RemoveAt(indexPath.Row);
                controller.TableView.DeleteRows(new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Fade);
            }
            else if (editingStyle == UITableViewCellEditingStyle.Insert)
            {
                // Create a new instance of the appropriate class, insert it into the array, and add a new row to the table view.
            }
        }
        /*
            // Override to support rearranging the table view.
        public override void MoveRow (UITableView tableView, NSIndexPath sourceIndexPath, NSIndexPath destinationIndexPath)
        {
        }
        */
        /*
        // Override to support conditional rearranging of the table view.
        public override bool CanMoveRow (UITableView tableView, NSIndexPath indexPath)
        {
            // Return false if you do not want the item to be re-orderable.
            return true;
        }
        */
        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            var selected_post = objects[indexPath.Row];
            controller.ItemSelected(selected_post);
        }
    }
}

