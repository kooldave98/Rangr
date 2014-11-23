using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using System.Linq;
using System.Collections.Generic;
using App.Common;
using MonoTouch.ObjCRuntime;

namespace App.iOS
{
	public class PostsTableViewSource : UITableViewSource
	{
		readonly IList<Post> objects;
		readonly MainViewController controller;

		public PostsTableViewSource (MainViewController controller, IList<Post> objects)
		{
			this.controller = controller;
			this.objects = objects;
		}

		// Customize the number of sections in the table view.
		public override int NumberOfSections (UITableView tableView)
		{
			return 1;
		}

		public override int RowsInSection (UITableView tableview, int section)
		{
			return objects.Count;
		}
		// Customize the appearance of table view cells.
		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			var post = objects [indexPath.Row];

			var cell = tableView.DequeueReusableCell (PostCellView.Key) as PostCellView;

			if (cell == null) {
				cell = PostCellView.Create ();
				cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
			}

			cell.BindDataToCell (post);


//			if (cell == null) {
//				cell = new UITableViewCell (UITableViewCellStyle.Subtitle, CellIdentifier);
//
//				cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
//			}

//			cell.TextLabel.Text = objects[indexPath.Row].UserDisplayName;
//			cell.DetailTextLabel.Text = objects[indexPath.Row].Text;
//			cell.ImageView.Image = UIImage.FromBundle (PlaceholderImagePath);
			//cell.ImageView.Image = UIImage.FromFile("Images/"+tableItems[indexPath.Row].ImageName); // don't use for Value2


			return cell;
		}

		public override float GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
		{
			return 90;
		}

		public override bool CanEditRow (UITableView tableView, NSIndexPath indexPath)
		{
			// Return false if you do not want the specified item to be editable.
			return false;
		}

		public override void CommitEditingStyle (UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
		{
			if (editingStyle == UITableViewCellEditingStyle.Delete) {
				// Delete the row from the data source.
				objects.RemoveAt (indexPath.Row);
				controller.TableView.DeleteRows (new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Fade);
			} else if (editingStyle == UITableViewCellEditingStyle.Insert) {
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
		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			if (controller.DetailViewController == null)
				controller.DetailViewController = new DetailViewController ();

			controller.DetailViewController.SetDetailItem (objects [indexPath.Row]);

			// Pass the selected object to the new view controller.
			controller.NavigationController.PushViewController (controller.DetailViewController, true);
		}
	}
}

