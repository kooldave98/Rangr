﻿
using System;

using Foundation;
using UIKit;
using solid_lib;

namespace ios_ui_lib
{
    public class MobileEntryViewController : UITableViewController, ICountryChooser
    {
        public MobileEntryViewController()
            : base(UITableViewStyle.Grouped)
        {
        }

        MobileEntryViewSource table_source;

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();
			
            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            Title = "Your Number";
			
            // Register the TableView's data source
            TableView.Source = table_source = new MobileEntryViewSource(this);
            TableView.RowHeight = UITableView.AutomaticDimension;
            TableView.EstimatedRowHeight = 100.0f;

            //TableView.BackgroundColor = UIColor.White;

            TableView.RegisterClassForCellReuse(typeof(TextDisplayCell), TextDisplayCell.Key);

            TableView.RegisterClassForCellReuse(typeof(NumberEntryCell), NumberEntryCell.Key);

        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            table_source.focus_number_text_field();
        }

        public void RefreshView()
        {
            TableView.ReloadData();
        }

        public int last_chosen_country { get; set;}

        public void CountryChooserSelected(int current_index)
        {
            OnCountryChooserSelected(current_index);
        }

        public event Action<int> OnCountryChooserSelected = delegate {};
    }

    public interface ICountryChooser
    {
        int last_chosen_country { get; set;}

        void CountryChooserSelected(int current_index);
    }


    public class MobileEntryViewSource : UITableViewSource
    {
        private ICountryChooser chooser;

        private MobileEntryCellType[] cells;

        public MobileEntryViewSource(ICountryChooser the_chooser)
        {
            chooser = Guard.IsNotNull(the_chooser, "the_chooser");

            cells = new MobileEntryCellType[]{
                MobileEntryCellType.Description,
                MobileEntryCellType.CountrySelection,
                MobileEntryCellType.NumberEntry,
            };
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            // TODO: return the actual number of sections
            return 1;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            // TODO: return the actual number of items in the section
            return cells.Length;
        }

        public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
        {
            if (cells[indexPath.Row] == MobileEntryCellType.CountrySelection)
            {
                chooser.CountryChooserSelected(chooser.last_chosen_country);
            }

            tableView.DeselectRow (indexPath, true); // iOS convention is to remove the highlight
        }

        public void focus_number_text_field()
        {
            number_cell.focus_number_field();
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = new UITableViewCell();

            switch (cells[indexPath.Row])
            {
                case MobileEntryCellType.Description:
                    return GetDescriptionCell(tableView, indexPath);
                case MobileEntryCellType.CountrySelection:
                    return GetCountrySelectionCell(tableView, indexPath);
                case MobileEntryCellType.NumberEntry:
                    return GetNumberEntryCell(tableView, indexPath);
                default:
                    break;
            }

            return cell;
        }

        public UITableViewCell GetDescriptionCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(TextDisplayCell.Key) as TextDisplayCell;
            if (cell == null)
                cell = new TextDisplayCell();

            ((TextDisplayCell)cell).BindDataToCell("Please confirm your country code and enter your phone number");

            cell.SetNeedsUpdateConstraints();
            cell.UpdateConstraintsIfNeeded();

            return cell;
        }

        public UITableViewCell GetCountrySelectionCell(UITableView tableView, NSIndexPath indexPath)
        {
            const string CountryCellKey = "CountryCellKey";

            var cell = tableView.DequeueReusableCell(CountryCellKey) as UITableViewCell;
            if (cell == null)
                cell = new UITableViewCell(UITableViewCellStyle.Value1, CountryCellKey);


            var data = CountryCodesViewController.codes.ToArray()[chooser.last_chosen_country];

            cell.TextLabel.Text = data.Key;

            cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;

            return cell;
        }

        private NumberEntryCell number_cell;

        public UITableViewCell GetNumberEntryCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(NumberEntryCell.Key) as NumberEntryCell;
            if (cell == null)
                cell = new NumberEntryCell();


            var data = CountryCodesViewController.codes.ToArray()[chooser.last_chosen_country];

            ((NumberEntryCell)cell).BindDataToCell(data.Value);

            cell.SetNeedsUpdateConstraints();
            cell.UpdateConstraintsIfNeeded();

            number_cell = cell;

            return cell;
        }

//        public override nfloat GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
//        {
//            switch (cells[indexPath.Row])
//            {
//                case MobileEntryCellType.Description:
//                    return 20.5f;
//                default:
//                    break;
//            }
//
//        }
    }

    public class TextDisplayCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("TextDisplayCell");

        private bool didSetupConstraints;

        public TextDisplayCell()
        {
            this.create_view();
        }

        public TextDisplayCell(IntPtr handle)
            : base(handle)
        {
            this.create_view();
        }

        private void create_view()
        {
            SelectionStyle = UITableViewCellSelectionStyle.None;
            //BackgroundColor = UIColor.LightGray;
            //ContentView.BackgroundColor = UIColor.LightGray;

            ContentView.AddSubview(description_label = new UILabel(){
                TextColor = UIColor.Black,
                Font = UIFont.PreferredSubheadline,
                TextAlignment = UITextAlignment.Center,
                Lines = 0,
                LineBreakMode = UILineBreakMode.WordWrap,
                TranslatesAutoresizingMaskIntoConstraints = false
            });
        }

        public void BindDataToCell(string description)
        {
            description_label.Text = description;
        }

        public override void UpdateConstraints()
        {
            base.UpdateConstraints();

            if (this.didSetupConstraints)
            {
                return;
            }            

            this.description_label.SetContentCompressionResistancePriority(Layout.RequiredPriority, UILayoutConstraintAxis.Vertical);


            //var sibling_sibling_margin = HumanInterface.sibling_sibling_margin;
            var parent_child_margin = HumanInterface.parent_child_margin;

            ContentView.ConstrainLayout(() => 

                description_label.Frame.Left == ContentView.Frame.Left + parent_child_margin &&
                description_label.Frame.Right == ContentView.Frame.Right - parent_child_margin &&
                description_label.Frame.Top == ContentView.Frame.Top + parent_child_margin &&
                description_label.Frame.Bottom == ContentView.Frame.Bottom - parent_child_margin

            );

            this.didSetupConstraints = true;

        }

        private UILabel description_label;
    }

    public class NumberEntryCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("NumberEntryCell");

        private bool didSetupConstraints;

        public NumberEntryCell()
        {
            this.create_view();
        }

        public NumberEntryCell(IntPtr handle)
            : base(handle)
        {
            this.create_view();
        }

        private void create_view()
        {
            SelectionStyle = UITableViewCellSelectionStyle.None;

            ContentView.AddSubview(code_field = new UITextField(){
                TextColor = UIColor.Black,
                Font = UIFont.PreferredSubheadline,
                TextAlignment = UITextAlignment.Center,
                TranslatesAutoresizingMaskIntoConstraints = false,
                KeyboardType = UIKeyboardType.NumberPad,
                ShouldBeginEditing = (t)=>false
            });

            ContentView.AddSubview(number_field = new UITextField(){
                TextColor = UIColor.Black,
                Font = UIFont.PreferredSubheadline,
                TextAlignment = UITextAlignment.Left,
                TranslatesAutoresizingMaskIntoConstraints = false,
                KeyboardType = UIKeyboardType.NumberPad,
                Placeholder = "your number here"
            });
        }

        public void BindDataToCell(string country_code)
        {
            code_field.Text = country_code;
        }

        public override void UpdateConstraints()
        {
            base.UpdateConstraints();

            if (this.didSetupConstraints)
            {
                return;
            }            

            this.code_field.SetContentCompressionResistancePriority(Layout.RequiredPriority, UILayoutConstraintAxis.Vertical);
            this.number_field.SetContentCompressionResistancePriority(Layout.RequiredPriority, UILayoutConstraintAxis.Vertical);

            var sibling_sibling_margin = HumanInterface.sibling_sibling_margin;
            var parent_child_margin = HumanInterface.parent_child_margin;
            var double_finger_tip_width = HumanInterface.finger_tip_diameter * 2;

            ContentView.ConstrainLayout(() => 

                code_field.Frame.Left == ContentView.Frame.Left + parent_child_margin &&
                code_field.Frame.Width == double_finger_tip_width  &&
                code_field.Frame.Top == ContentView.Frame.Top + parent_child_margin &&
                code_field.Frame.Bottom == ContentView.Frame.Bottom - parent_child_margin &&

                number_field.Frame.Left == code_field.Frame.Right + sibling_sibling_margin &&
                number_field.Frame.Right == ContentView.Frame.Right - parent_child_margin &&
                number_field.Frame.Top == ContentView.Frame.Top + parent_child_margin &&
                number_field.Frame.Bottom == ContentView.Frame.Bottom - parent_child_margin

            );

            this.didSetupConstraints = true;

        }

        public void focus_number_field()
        {
            number_field.BecomeFirstResponder();
        }

        private UITextField number_field;
        private UITextField code_field;
    }

    public enum MobileEntryCellType
    {
        Description,
        CountrySelection,
        NumberEntry
    }
}

