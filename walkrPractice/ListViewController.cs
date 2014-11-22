
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace walkrPractice
{

	public class MyCustomClass : UITableViewSource
	{
		UITableViewCell [] cells;

		public MyCustomClass (params UITableViewCell [] cells)
		{
			this.cells = cells;
		}

		public override int NumberOfSections (UITableView tableView)
		{
			return 1;
		}

		public override int RowsInSection (UITableView tableview, int section)
		{
			return 2;
		}
		// Customize the appearance of table view cells.
		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			return cells [indexPath.Row];
		}
	}

	public partial class ListViewController : UIViewController
	{
		public ListViewController () : base ("ListViewController", null)
		{

		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();

			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			tblTable.Source = new MyCustomClass (tblCellLogin, tblCellTableButton);
			btnClick.TouchUpInside += Save;
			btnTableButton.TouchUpInside += Save;
			// Perform any additional setup after loading the view, typically from a nib.
		}

		private void Save (object sender, EventArgs e)
		{
			UIAlertView alert = new UIAlertView ("Alert", "Welcome", null, "OK",  null);
			alert.Show ();

			alert.Clicked += alertClicked;
		}

		private void alertClicked(object a, UIButtonEventArgs b)
		{
			Console.WriteLine("It worked");
		}
	}
}

