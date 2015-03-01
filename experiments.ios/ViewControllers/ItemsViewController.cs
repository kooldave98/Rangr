
using System;
using System.Drawing;

using Foundation;
using UIKit;

namespace experiments.ios
{
    public class ItemsViewController : UIViewController
    {
        private Model model;
        private NSObject contentSizeCategoryChangedObserver;
        private UITableView TableView;
        public ItemsViewController()
        {
            model = new Model();
            model.PopulateItems();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.AddSubview(TableView = new UITableView(View.Bounds));

            TableView.Source = new ItemsTableViewSource(model);

            TableView.RegisterClassForCellReuse(typeof(ItemCell), ItemCell.Key);

            Title = "Auto Layout Table View";
            NavigationItem.LeftBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Trash, Clear);
            NavigationItem.RightBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Add, AddRow);

            TableView.AllowsSelection = false;
            TableView.RowHeight = UITableView.AutomaticDimension;
            TableView.EstimatedRowHeight = 44.0f;
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            this.contentSizeCategoryChangedObserver = NSNotificationCenter.DefaultCenter.AddObserver(UIApplication.ContentSizeCategoryChangedNotification, ContentSizeCategoryChanged);
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);

            if (contentSizeCategoryChangedObserver != null)
            {
                NSNotificationCenter.DefaultCenter.RemoveObserver(contentSizeCategoryChangedObserver);
            }
        }
            
        private void ContentSizeCategoryChanged(NSNotification notification)
        {
            TableView.ReloadData();
        }

        private void Clear(object sender, EventArgs e)
        {
            model.Items.Clear();
            TableView.ReloadData();
        }

        private void AddRow(object sender, EventArgs e)
        {
            model.AddSingleItem();
            var indexPath = NSIndexPath.Create(0, model.Items.Count - 1);
            TableView.InsertRows(new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Automatic);
        }
    }

    public class ItemsTableViewSource : UITableViewSource
    {
        private readonly Model model;

        public ItemsTableViewSource(Model the_model)
        {
            model = the_model;
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return model.Items.Count;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = (ItemCell)tableView.DequeueReusableCell(ItemCell.Key);

            cell.UpdateFonts();

            var item = model.Items[indexPath.Row];
            cell.bind_data(item);

            cell.SetNeedsUpdateConstraints();
            cell.UpdateConstraintsIfNeeded();

            return cell;
        }

        public override bool CanEditRow(UITableView tableView, NSIndexPath indexPath)
        {
            // Return false if you do not want the specified item to be editable.
            return false;
        }
    }
}

