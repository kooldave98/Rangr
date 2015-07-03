using System;
using Foundation;
using UIKit;
using System.Collections.Generic;
using common_lib;

namespace ios_ui_lib
{
    public class CountryCodesViewController : UITableViewController
    {
        public CountryCodesViewController(MobileEntrySequenceViewModel the_view_model)
            : base(UITableViewStyle.Grouped)
        {
            view_model = Guard.IsNotNull(the_view_model, "the_view_model");
        }

        private MobileEntrySequenceViewModel view_model;

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();
			
            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            Title = "Your Country";

            // Register the TableView's data source

            TableView.Source = setup_and_return_view_source();
        }

        private CountryCodesViewSource setup_and_return_view_source()
        {
            table_source = new CountryCodesViewSource(view_model);

            table_source.OnCountryIndexSelected += (index) => {
                view_model.select_country_index(index);
                OnCountrySelected(view_model.selected_country);
            };

            return table_source;
        }

        public void Refresh()
        {
            TableView.ReloadData();
        }

        public event Action<ISOCountry> OnCountrySelected = delegate {};


        private CountryCodesViewSource table_source;
    }

    public class CountryCodesViewSource : UITableViewSource
    {
        private ISelectableCountries view_model;

        public event Action<int> OnCountryIndexSelected = delegate {};

        public CountryCodesViewSource(ISelectableCountries the_view_model)
        {
            view_model = Guard.IsNotNull(the_view_model, "the_view_model");
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            // TODO: return the actual number of sections
            return 1;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return (nint)view_model.iso_countries.Length;
        }

        public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
        {
            tableView.DeselectRow (indexPath, true); // iOS convention is to remove the highlight
            OnCountryIndexSelected(indexPath.Row);
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(Key) as UITableViewCell;
            if (cell == null)
                cell = new UITableViewCell(UITableViewCellStyle.Value1, Key);


            var data = view_model.iso_countries[indexPath.Row];

            cell.TextLabel.Text = data.ISOName;

            cell.DetailTextLabel.Text = data.ISODialCode;
            if (indexPath.Row == view_model.selected_country_index)
            {
                cell.Accessory = UITableViewCellAccessory.Checkmark;
            }
            else
            {
                cell.Accessory = UITableViewCellAccessory.None;
            }

            return cell;
        }

        public static readonly NSString Key = new NSString("CellKEY");
    }
}

