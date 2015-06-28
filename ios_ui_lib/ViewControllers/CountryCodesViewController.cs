using System;
using Foundation;
using UIKit;
using System.Collections.Generic;
using solid_lib;

namespace ios_ui_lib
{
    public class CountryCodesViewController : UITableViewController, ICountrySelector
    {
        public CountryCodesViewController()
            : base(UITableViewStyle.Grouped)
        {
        }

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

            TableView.Source = new CountryCodesViewSource(codes.ToArray(), this);
        }

        public int LastSelectedCountry { get; set; }

        public void CountrySelected(int selected_index)
        {
            OnCountrySelected(selected_index);
        }

        public void Refresh()
        {
            TableView.ReloadData();
        }

        public event Action<int> OnCountrySelected = delegate {};


        public static List<KeyValuePair<string, string>> codes = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("Nigeria", "+234"),
            new KeyValuePair<string, string>("United Kingdom", "+44"),
            new KeyValuePair<string, string>("United States", "+1")
        };
    }

    public interface ICountrySelector
    {
        int LastSelectedCountry { get; set; }
        void CountrySelected(int selected_index);
    }

    public class CountryCodesViewSource : UITableViewSource
    {
        private ICountrySelector country_selector;

        private KeyValuePair<string, string>[] codes;
        
        public CountryCodesViewSource(KeyValuePair<string, string>[]  the_codes, ICountrySelector the_country_selector)
        {
            codes = Guard.IsNotNull(the_codes, "the_codes");
            country_selector = Guard.IsNotNull(the_country_selector, "the_country_selector");
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            // TODO: return the actual number of sections
            return 1;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return (nint)codes.Length;
        }

        public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
        {
            tableView.DeselectRow (indexPath, true); // iOS convention is to remove the highlight
            country_selector.CountrySelected(indexPath.Row);
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(Key) as UITableViewCell;
            if (cell == null)
                cell = new UITableViewCell(UITableViewCellStyle.Value1, Key);


            var data = codes[indexPath.Row];

            cell.TextLabel.Text = data.Key;

            cell.DetailTextLabel.Text = data.Value;
            if (indexPath.Row == country_selector.LastSelectedCountry)
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

