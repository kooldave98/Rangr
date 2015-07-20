
using System;
using System.Drawing;

using Foundation;
using UIKit;
using App.Common;
using System.Collections.Generic;
using CoreAnimation;
using CoreGraphics;
using common_lib;

namespace rangr.ios
{
    public class PostListViewController : BaseViewModelController<FeedViewModel>
    {
        public override string TitleLabel {
            get { return "Feed"; }
        }

        public PostListViewController()
        {
            view_model = new FeedViewModel();
            view_model.OnNewPostsReceived += (sender, e) => {
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

            TableView.RowHeight = UITableView.AutomaticDimension;
            TableView.EstimatedRowHeight = 200.0f;

            TableView.RegisterClassForCellReuse(typeof(PostCellView), PostCellView.Key);
        }

        public override void WillAddConstraints()
        {
            View.ConstrainLayout(() => 
                TableView.Frame.Top == View.Bounds.Top &&
                TableView.Frame.Left == View.Bounds.Left &&
                TableView.Frame.Right == View.Bounds.Right &&
                TableView.Frame.Height == View.Bounds.Height
            );
        }


        public override async void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

                show_progress();
                await view_model.RefreshPosts();
                dismiss_progress();
        }

        public void ItemSelected(Post selected_post)
        {
            PostItemSelected(selected_post);
        }


        public event Action<Post> PostItemSelected = delegate {};
        //public event Action<string> HashTagSelected = delegate {};
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

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = (PostCellView)tableView.DequeueReusableCell(PostCellView.Key);

            var post = objects[indexPath.Row];

            if (cell == null)
            {
                cell = new PostCellView();
            }

            cell.BindData(post);

            cell.SetNeedsUpdateConstraints();

            return cell;
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
            return false;
        }
    }
}

