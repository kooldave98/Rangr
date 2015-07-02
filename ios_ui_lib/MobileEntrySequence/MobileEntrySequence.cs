using System;
using System.Threading.Tasks;
using UIKit;
using System.Collections.Generic;
using solid_lib;

namespace ios_ui_lib
{
    public class MobileEntrySequence
    {
        private readonly TaskCompletionSource<MobileEntrySequenceResults> task_completion_source = 
            new TaskCompletionSource<MobileEntrySequenceResults>();

        private UINavigationController navigation = new UINavigationController();

        private MobileEntryViewController mobile_entry;
        private CountryCodesViewController country_chooser;

        public MobileEntrySequence(MobileEntrySequenceViewModel the_view_model)
        {
            view_model = Guard.IsNotNull(the_view_model, "the_view_model");
        }

        private void set_cancel_button(bool shown)
        {
            var cancelButton = new UIBarButtonItem(UIBarButtonSystemItem.Cancel);
            cancelButton.Clicked += (sender, e) =>
            {
                HandleCancel();
            };

            mobile_entry.NavigationItem.SetLeftBarButtonItem(cancelButton, false);
        }

        private void set_done_button()
        {
            var doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done);
            doneButton.Clicked += (sender, e) =>
            {
                HandleDoneMobileEntry();
            };
            doneButton.Enabled = false;

            mobile_entry.NavigationItem.SetRightBarButtonItem(doneButton, false);
        }

        private async Task show_mobile_entry_page(UIViewController mother, bool is_cancelable)
        {
            mobile_entry = new MobileEntryViewController(view_model);

            set_done_button();

            set_cancel_button(is_cancelable);

            mobile_entry.OnCountryChooserSelected += () => {
                show_country_chooser_page();
            };

            mobile_entry.OnNumberIsValid += (is_valid) => {
                if (is_valid)
                {
                    mobile_entry.NavigationItem.RightBarButtonItem.Enabled = true;
                }
                else
                {                    
                    mobile_entry.NavigationItem.RightBarButtonItem.Enabled = false;
                }
            };

            navigation = mobile_entry.ToNavigationController();

            await mother.PresentViewControllerAsync(navigation, true);
        }

        private void show_country_chooser_page()
        {
            country_chooser = new CountryCodesViewController(view_model);
            navigation.PushViewController(country_chooser, true);
            country_chooser.Refresh();
            country_chooser.OnCountrySelected += (country) =>
            {
                mobile_entry.RefreshView();
                navigation.PopViewController(true);
            };
        }

        private void HandleDoneMobileEntry()
        {
            task_completion_source.SetResult(new MobileEntrySequenceResults(view_model.mobile_number));
        }

        private void HandleCancel()
        {
            task_completion_source.SetResult(MobileEntrySequenceResults.CanceledResult);
        }

        private MobileEntrySequenceViewModel view_model;


        public async Task<MobileEntrySequenceResults> StartAsync(UIViewController root)                                                                
        {
            await this.show_mobile_entry_page(root, true);

            var results = await this.task_completion_source.Task;

            await this.navigation.DismissViewControllerAsync(true);

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

