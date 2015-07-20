
using System;
using System.Linq;

using Foundation;
using UIKit;
using common_lib;

namespace ios_ui_lib
{
    public class MobileEntryViewController : UITableViewController
    {
        private MobileEntrySequenceViewModel view_model;

        public MobileEntryViewController(MobileEntrySequenceViewModel the_view_model)
            : base(UITableViewStyle.Grouped)
        {
            view_model = Guard.IsNotNull(the_view_model, "the_view_model");
        }


        private MobileEntryViewSource table_source;

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
            TableView.Source = setup_and_return_view_source();
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
            table_source.clear_number_field();
        }

        private MobileEntryViewSource setup_and_return_view_source()
        {
            table_source = new MobileEntryViewSource(view_model, view_model);

            table_source.OnCountryChooserSelected += () => {
                OnCountryChooserSelected();
            };

            table_source.OnNumberEntered += (number) => {
                view_model.mobile_number = number;
                OnNumberEntered(number);
                OnNumberIsValid(view_model.is_valid_international_number(number));
            };

            return table_source;
        }

        public event Action OnCountryChooserSelected = delegate {};
        public event Action<string> OnNumberEntered = delegate {};
        public event Action<bool> OnNumberIsValid = delegate{};
    }

    public class MobileEntryViewSource : UITableViewSource
    {
        private ISelectedCountry view_model;
        private NumberEntryCell.IPhoneNumberFormatter formatter;

        private MobileEntryCellType[] cells;

        public MobileEntryViewSource(ISelectedCountry the_view_model
                                    , NumberEntryCell.IPhoneNumberFormatter the_formatter)
        {
            view_model = Guard.IsNotNull(the_view_model, "the_view_model");
            formatter = Guard.IsNotNull(the_formatter, "the_formatter");

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
        public event Action<string> OnNumberEntered = delegate{};
        public event Action OnCountryChooserSelected = delegate{};

        public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
        {
            if (cells[indexPath.Row] == MobileEntryCellType.CountrySelection)
            {
                OnCountryChooserSelected();
            }

            tableView.DeselectRow (indexPath, true); // iOS convention is to remove the highlight
        }

        public void focus_number_text_field()
        {
            number_cell.focus_number_field();
        }

        public void clear_number_field()
        {
            number_cell.clear_number_field();
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


            var data = view_model.selected_country;

            cell.TextLabel.Text = data.ISOName;

            cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;

            return cell;
        }

        private NumberEntryCell number_cell;

        public UITableViewCell GetNumberEntryCell(UITableView tableView, NSIndexPath indexPath)
        {
//            var cell = tableView.DequeueReusableCell(NumberEntryCell.Key) as NumberEntryCell;
//            if (cell == null)
            var cell = new NumberEntryCell(formatter);


            var data = view_model.selected_country;

            ((NumberEntryCell)cell).BindDataToCell(data.ISODialCode);

            cell.SetNeedsUpdateConstraints();
            cell.UpdateConstraintsIfNeeded();

            number_cell = cell;

            number_cell.OnNumberEntered += (num) =>
            {
                OnNumberEntered(num);
            };
            return cell;
        }
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

            this.description_label.SetContentCompressionResistancePriority(EasyLayout.RequiredPriority, UILayoutConstraintAxis.Vertical);


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

        public NumberEntryCell(IPhoneNumberFormatter the_formatter)
        {
            formatter = Guard.IsNotNull(the_formatter, "the_formatter");
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

            number_field.EditingChanged += (sender, e) => {

                //For now, there needs to be a white-space in-between for things to work.
                var condensed = code_field.Text + WHITE_SPACE + number_field.Text;

                var formatted = formatter.format_input(condensed);

                number_field.Text = remove_dial_code(formatted);

                OnNumberEntered(condensed);
            };
        }

        const string WHITE_SPACE = " ";

        private string remove_dial_code(string formatted_number)
        {            
            return formatted_number.Split(null).Skip(1).Aggregate((i, j) => i + WHITE_SPACE + j);
        }

        private IPhoneNumberFormatter formatter;

        public event Action<string> OnNumberEntered = delegate{};

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

            this.code_field.SetContentCompressionResistancePriority(EasyLayout.RequiredPriority, UILayoutConstraintAxis.Vertical);
            this.number_field.SetContentCompressionResistancePriority(EasyLayout.RequiredPriority, UILayoutConstraintAxis.Vertical);

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

        public void clear_number_field()
        {
            number_field.Text = "";
            number_field.SendActionForControlEvents(UIControlEvent.EditingChanged);
        }

        private UITextField number_field;
        private UITextField code_field;

        public interface IPhoneNumberFormatter
        {
            string format_input(string input);
        }

    }

    public enum MobileEntryCellType
    {
        Description,
        CountrySelection,
        NumberEntry
    }
}

