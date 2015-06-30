using System;
using System.Threading.Tasks;
using UIKit;

namespace ios_ui_lib
{
    public class MobileEntrySequence
    {
        private readonly TaskCompletionSource<MobileEntrySequenceResults> task_completion_source = 
            new TaskCompletionSource<MobileEntrySequenceResults>();

        private UINavigationController navigation;

        private MobileEntryViewController mobile_entry;
        private CountryCodesViewController country_chooser;

        private string entered_mobile = "";

        private MobileEntrySequence()
        {
            navigation = new UINavigationController();
        }

        private void set_cancel_button(bool shown)
        {
            var cancelButton = new UIBarButtonItem(UIBarButtonSystemItem.Cancel);
            cancelButton.Clicked += (sender, e) => {
                HandleCancel();
            };

            mobile_entry.NavigationItem.SetLeftBarButtonItem(cancelButton, false);
        }

        private void set_done_button()
        {
            var doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done);
            doneButton.Clicked += (sender, e) => {
                HandleNumberEntered(entered_mobile);
            };
            doneButton.Enabled = false;

            mobile_entry.NavigationItem.SetRightBarButtonItem(doneButton, false);
        }

        private async Task show_mobile_entry_page(UIViewController mother, bool is_cancelable)
        {
            mobile_entry = new MobileEntryViewController();

            set_done_button();

            set_cancel_button(is_cancelable);

            mobile_entry.last_chosen_country = 0;

            mobile_entry.OnCountryChooserSelected += (i) => {
                show_country_chooser_page(i);
            };

            mobile_entry.OnNumberEntered += (num) => {
                entered_mobile = validate_number(num);
            };

            navigation = mobile_entry.ToNavigationController();

            await mother.PresentViewControllerAsync(navigation, true);
        }

        private string validate_number(string input)
        {
            if (input.Length > 7)
            {
                mobile_entry.NavigationItem.RightBarButtonItem.Enabled = true;
            }
            else
            {
                mobile_entry.NavigationItem.RightBarButtonItem.Enabled = false;
            }

            return input;
        }

        private void show_country_chooser_page(int index)
        {
            country_chooser = new CountryCodesViewController();
            country_chooser.LastSelectedCountry = index;
            navigation.PushViewController(country_chooser, true);
            country_chooser.Refresh();
            country_chooser.OnCountrySelected += (i) => {
                mobile_entry.last_chosen_country = i;
                mobile_entry.RefreshView();
                navigation.PopViewController(true);
            };


        }

        private void HandleNumberEntered(string mobile_number)
        {
            task_completion_source.SetResult(new MobileEntrySequenceResults(mobile_number));
        }

        private void HandleCancel()
        {
            task_completion_source.SetResult(MobileEntrySequenceResults.CanceledResult);
        }

        public static async Task<MobileEntrySequenceResults> ShowAsync(UIViewController mother)
        {
            var sequence = new MobileEntrySequence();

            await sequence.show_mobile_entry_page(mother, true);

            var results = await sequence.task_completion_source.Task;

            await sequence.navigation.DismissViewControllerAsync(true);

            return results;
        }
    }

    public class MobileEntrySequenceResults
    {
        public static MobileEntrySequenceResults CanceledResult = new MobileEntrySequenceResults();

        public bool Canceled { get; private set; }

        public string EnteredMobileNumber { get; private set; }

        public MobileEntrySequenceResults(string entered_mobile_number)
        {
            EnteredMobileNumber = entered_mobile_number;
        }

        private MobileEntrySequenceResults()
        {
            Canceled = true;
        }
    }
}

