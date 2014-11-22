// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;
using System.CodeDom.Compiler;

namespace walkrPractice
{
	[Register ("ListViewController")]
	partial class ListViewController
	{
		[Outlet]
		MonoTouch.UIKit.UIButton btnClick { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton btnTableButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel lblText { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITableViewCell tblCellLogin { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITableViewCell tblCellTableButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITableView tblTable { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (tblCellTableButton != null) {
				tblCellTableButton.Dispose ();
				tblCellTableButton = null;
			}

			if (btnTableButton != null) {
				btnTableButton.Dispose ();
				btnTableButton = null;
			}

			if (btnClick != null) {
				btnClick.Dispose ();
				btnClick = null;
			}

			if (lblText != null) {
				lblText.Dispose ();
				lblText = null;
			}

			if (tblCellLogin != null) {
				tblCellLogin.Dispose ();
				tblCellLogin = null;
			}

			if (tblTable != null) {
				tblTable.Dispose ();
				tblTable = null;
			}
		}
	}
}
