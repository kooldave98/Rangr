
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using App.Common;
using AndroidResource = Android.Resource;
using Android.Content.PM;

namespace App.Android
{
	[Activity (Label = "Simulation", ScreenOrientation = ScreenOrientation.Portrait,
		ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]			
	public class SimulationActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.simulation_activity);

			var persisted_simulation = PersistentStorage.Current.Load<string> ("simulation");

			switch (persisted_simulation) {
			case "L":
				FindViewById<RadioButton> (Resource.Id.radio_Live).Checked = true;
				break;
			case "A":
				FindViewById<RadioButton> (Resource.Id.radio_A).Checked = true;
				break;
			case "B":
				FindViewById<RadioButton> (Resource.Id.radio_B).Checked = true;
				break;
			case "C":
				FindViewById<RadioButton> (Resource.Id.radio_C).Checked = true;
				break;
			case "D":
				FindViewById<RadioButton> (Resource.Id.radio_D).Checked = true;
				break;
			default:
				FindViewById<RadioButton> (Resource.Id.radio_Live).Checked = true;
				break;
			}


			ActionBar.SetDisplayHomeAsUpEnabled (true);
			ActionBar.SetDisplayShowHomeEnabled (true);
		}

		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			switch (item.ItemId) {
			case AndroidResource.Id.Home:
				Finish ();
				break;
			}
			return base.OnOptionsItemSelected (item);
		}

		[Java.Interop.Export ("OnRadioButtonClicked")]
		public void OnRadioButtonClicked (View view)
		{
			// Is the button now checked?
			var is_checked = ((RadioButton)view).Checked;

			// Check which radio button was clicked
			switch (view.Id) {
			case Resource.Id.radio_Live:
				if (is_checked) {
					AppEvents.Current.TriggerLocationSimulatedEvent ("L");
					PersistentStorage.Current.Save ("simulation", "L");
					ShowAlert ("Simulation", "You are now back to your real location");
				}
				break;
			case Resource.Id.radio_A:
				if (is_checked) {
					AppEvents.Current.TriggerLocationSimulatedEvent ("A");
					PersistentStorage.Current.Save ("simulation", "A");
					ShowAlert ("Simulation", "You have now been jumped to location A");
				}
				break;
			case Resource.Id.radio_B:
				if (is_checked) {
					AppEvents.Current.TriggerLocationSimulatedEvent ("B");
					PersistentStorage.Current.Save ("simulation", "B");
					ShowAlert ("Simulation", "You have now been jumped to location B");
				}
				break;
			case Resource.Id.radio_C:
				if (is_checked) {
					AppEvents.Current.TriggerLocationSimulatedEvent ("C");
					PersistentStorage.Current.Save ("simulation", "C");
					ShowAlert ("Simulation", "You have now been jumped to location C");
				}
				break;
			case Resource.Id.radio_D:
				if (is_checked) {
					AppEvents.Current.TriggerLocationSimulatedEvent ("D");
					PersistentStorage.Current.Save ("simulation", "D");
					ShowAlert ("Simulation", "You have now been jumped to location D");
				}
				break;
			}
		}

		protected void ShowAlert (string title, string message, string ok_button_text = "Ok", Action ok_button_action = null)
		{
			var builder = new AlertDialog.Builder (this)
				.SetTitle (title)
				.SetMessage (message)
				.SetPositiveButton (ok_button_text, (innerSender, innere) => {
				RunOnUiThread (() => {
					if (ok_button_action != null) {
						ok_button_action ();
					}
				});

			});
			var dialog = builder.Create ();
			dialog.Show ();
		}

	}
}

