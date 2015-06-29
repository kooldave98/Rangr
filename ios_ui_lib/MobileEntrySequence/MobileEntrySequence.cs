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
        private UIBarButtonItem cancelButton;

        private MobileEntryViewController mobile_entry;
        private CountryCodesViewController country_chooser;

        private string entered_mobile = "";

//        dc.NavigationItem.SetLeftBarButtonItem(new UIBarButtonItem(UIBarButtonSystemItem.Cancel), false);
//        

        private MobileEntrySequence()
        {
            cancelButton = new UIBarButtonItem(UIBarButtonSystemItem.Cancel);
            navigation = new UINavigationController();
        }

        private void AddCancelButton(UIViewController page)
        {
            //page.ToolbarItems.Add(_cancelButton);
        }

        private UIBarButtonItem Done()
        {
            return new UIBarButtonItem(UIBarButtonSystemItem.Done);
        }

        private async Task show_mobile_entry_page(UIViewController mother)
        {
            mobile_entry = new MobileEntryViewController();
            mobile_entry.NavigationItem.SetRightBarButtonItem(Done(), false);
            mobile_entry.NavigationItem.RightBarButtonItem.Enabled = false;
            mobile_entry.NavigationItem.RightBarButtonItem.Clicked += (sender, e) => {
                HandleNumberEntered(entered_mobile);
            };
            mobile_entry.last_chosen_country = 0;

            mobile_entry.OnCountryChooserSelected += (i) => {
                show_country_chooser_page(i);
            };

            mobile_entry.OnNumberEntered += (num) => {
                entered_mobile = validate_number(num);
            };

            AddCancelButton(mobile_entry);

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

            await sequence.show_mobile_entry_page(mother);

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

