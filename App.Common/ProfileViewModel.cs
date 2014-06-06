using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using App.Core.Portable;
using App.Core.Portable.Persistence;
using App.Common.Shared;

namespace App.Common
{
	public class ProfileViewModel : ViewModelBase
	{
		public ProfileViewModel (IPersistentStorage the_persistent_storage_instance)
		{
			var person = Session.GetInstance (the_persistent_storage_instance).GetCurrentUser ();

			if (person == null) {
				throw new ArgumentNullException ("person");
			}

			person.status_message = person.status_message;
			person.twitter_name = "@walkr";
			person.telephone_number = "0123456789";

			Person = person;

			PropertyGroups = new ObservableCollection<PropertyGroup> ();

			var general = new PropertyGroup ("General");

			general.Add ("Display Name", person.user_display_name, PropertyType.Generic);
			general.Add ("Status", person.status_message, PropertyType.Generic);

			if (general.Properties.Count > 0) {
				PropertyGroups.Add (general);
			}

			var phone = new PropertyGroup ("Phone");

			phone.Add ("Phone", person.telephone_number, PropertyType.Phone);

			if (phone.Properties.Count > 0) {
				PropertyGroups.Add (phone);
			}

			var online = new PropertyGroup ("Online");

			online.Add ("Image", CleanUrl (person.image_url), PropertyType.Url);			
			online.Add ("Twitter", CleanTwitter (person.twitter_name), PropertyType.Twitter);

			if (online.Properties.Count > 0) {
				PropertyGroups.Add (online);
			}
		}

		static string CleanUrl (string url)
		{
			var trimmed = (url ?? "").Trim ();
			if (trimmed.Length == 0) return "";

			var upper = trimmed.ToUpperInvariant ();
			if (!upper.StartsWith ("HTTP")) {
				return "http://" + trimmed;
			}
			else {
				return trimmed;
			}
		}

		static string CleanTwitter (string username)
		{
			var trimmed = (username ?? "").Trim ();
			if (trimmed.Length == 0) return "";

			if (!trimmed.StartsWith ("@")) {
				return "@" + trimmed;
			}
			else {
				return trimmed;
			}
		}
			
		public IUser Person { get; private set; }

		public ObservableCollection<PropertyGroup> PropertyGroups { get; private set; }

	}

	public class PropertyGroup : IEnumerable<Property>
	{
		public string Title { get; private set; }
		public ObservableCollection<Property> Properties { get; private set; }

		public PropertyGroup (string title)
		{
			Title = title;
			Properties = new ObservableCollection<Property> ();
		}

		public void Add (string name, string value, PropertyType type)
		{
			if (!string.IsNullOrWhiteSpace (value)) {
				Properties.Add (new Property (name, value, type));
			}
		}

		IEnumerator<Property> IEnumerable<Property>.GetEnumerator ()
		{
			return Properties.GetEnumerator ();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator ()
		{
			return Properties.GetEnumerator ();
		}
	}

	public class Property
	{
		public string Name { get; private set; }
		public string Value { get; private set; }
		public PropertyType Type { get; private set; }

		public Property (string name, string value, PropertyType type)
		{
			Name = name;
			Value = value.Trim ();
			Type = type;
		}

		public override string ToString ()
		{
			return string.Format ("{0} = {1}", Name, Value);
		}
	}

	public enum PropertyType
	{
		Generic,
		Phone,
		Email,
		Url,
		Twitter,
		Address,
	}


}

