
using System;
using System.Drawing;

using Foundation;
using UIKit;
using App.Common;
using System.Collections.Generic;
using CoreAnimation;
using CoreGraphics;
using ios_ui_lib;

namespace rangr.ios
{
    /// <summary>
    ///see below for how to manually embed a refresh control
    /// https://github.com/vandadnp/iOS-8-Swift-Programming-Cookbook/blob/master/chapter-tables/Displaying%20a%20Refresh%20Control%20for%20Table%20Views/Displaying%20a%20Refresh%20Control%20for%20Table%20Views/ViewController.swift
    /// </summary>
    public class PostListViewController : BaseViewModelController<FeedViewModel>
    {
        public override string TitleLabel 
        {
            get { return "Feed"; }
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

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.AddSubview(TableView = new UITableView());

            TableView.Source = new PostsTableViewSource(this, view_model.Posts);

            RefreshControl = new UIRefreshControl();

            RefreshControl.ValueChanged += async (object sender, EventArgs e) =>
            {
                await view_model.RefreshPosts();
                RefreshControl.EndRefreshing();
            };

            TableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
            TableView.AddSubview(RefreshControl);
            TableView.BackgroundColor = UIColor.LightGray;
        }

        public override void ViewDidLayoutSubviews()
        {
            View.ConstrainLayout(() => 
                TableView.Frame.Top == View.Frame.Top &&
                TableView.Frame.Left == View.Frame.Left &&
                TableView.Frame.Right == View.Frame.Right &&
                TableView.Frame.Height == View.Frame.Height
            );
        }


        public override async void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (AppGlobal.Current.CurrentUserAndConnectionExists)
            {
                show_progress();
                await view_model.RefreshPosts();
                dismiss_progress();
            }
        }

        public void ItemSelected(Post selected_post)
        {
            PostItemSelected(selected_post);
        }


        public event Action<Post> PostItemSelected = delegate {};
        //public event Action NewPostSelected = delegate {};
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

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return objects.Count;
        }

        // Customize the appearance of table view cells.
        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var post = objects[indexPath.Row];

            var cell = tableView.DequeueReusableCell(PostCellView.Key) as PostCellView;

            if (cell == null)
            {
                cell = new PostCellView();
                //cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
            }

            cell.BindDataToCell(post);

            return cell;
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            //http://stackoverflow.com/questions/19597988/monotouch-calculate-uilabel-height
            //http://forums.xamarin.com/discussion/22686/dynamically-set-row-height-according-to-content
//            var cell = GetCell(tableView, indexPath);
//            cell.LayoutIfNeeded();
//            return cell.Frame.Height;
            return 120;
        
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            var selected_post = objects[indexPath.Row];
            controller.ItemSelected(selected_post);
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }

        public override bool CanEditRow(UITableView tableView, NSIndexPath indexPath)
        {
            // Return false if you do not want the specified item to be editable.
            return false;
        }

//        public override void WillDisplay(UITableView tableView, UITableViewCell cell, NSIndexPath indexPath)
//        {
//
//            //Nice, uncomment this method and see some magical effects.
//            //Learn more about transforms from here
//            //http://www.thinkandbuild.it/introduction-to-3d-drawing-in-core-animation-part-1/
//            //http://www.thinkandbuild.it/interactive-transitions/
//
//            //1. Setup the CATransform3D structure
//            CATransform3D rotation;
//            rotation = CATransform3D.MakeRotation( (90.0f*3.142f)/180.0f, 0.0f, 0.7f, 0.4f);
//            rotation.m34 = 1.0f/ -600.0f;
//
//            //2. Define the initial state (Before the animation)
//            cell.Layer.ShadowColor = UIColor.Black.CGColor;
//            cell.Layer.ShadowOffset = new CGSize(10, 10);
//            cell.Alpha = 0;
//
//            cell.Layer.Transform = rotation;
//            cell.Layer.AnchorPoint = new CGPoint(0.0f, 0.5f);
//
//            //3. Define the final state (After the animation) and commit the animation
//            UIView.BeginAnimations(@"rotation", (IntPtr)null);
//            UIView.SetAnimationDuration(0.8f);
//            cell.Layer.Transform = CATransform3D.Identity;
//            cell.Alpha = 1;
//            cell.Layer.ShadowOffset = new CGSize(0, 0);
//            UIView.CommitAnimations();
//        }

//        public override void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
//        {
//            //ToInvestigate: I think i'm inserting into the table wrongly.
//            //I add to the underlying collection then tell the table to reload.
//            //Performance wise, i'm not sure it is a great idea.
//            //Mabe using this method will be better off.
//
//            if (editingStyle == UITableViewCellEditingStyle.Insert)
//            {
//                // Create a new instance of the appropriate class, insert it into the array, and add a new row to the table view.
//            }
//        }

    }
}

