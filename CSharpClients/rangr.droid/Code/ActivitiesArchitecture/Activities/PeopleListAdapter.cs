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
using rangr.common;

namespace rangr.droid
{
	public class PeopleListAdapter : BaseAdapter
	{
		Context context;
		IList<Connection> items;
		GeoCoordinate current_location;

		public PeopleListAdapter (Context context, IList<Connection> items, GeoCoordinate current_location) : base ()
		{
			this.context = context;
			this.items = items;
			this.current_location = current_location;
		}

		public override long GetItemId (int position)
		{
			return position;
		}

		public override Java.Lang.Object GetItem (int position)
		{
			return new Java.Lang.String (items [position].ToString ());
		}

		public override int Count {
			get { return items.Count; }
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			var person = items [position];

			View view = convertView; // re-use an existing view, if one is available
			if (view == null) { // otherwise create a new one
				view = LayoutInflater.From (context).Inflate (Resource.Layout.PeopleListItem, parent, false);
			}

			view.FindViewById<TextView> (Resource.Id.personName).Text = person.user_display_name;
			view.FindViewById<TextView> (Resource.Id.distance).Text = Math.Round (get_distance (person.long_lat_acc_geo_string), 1) + " metres";
			view.FindViewById<TextView> (Resource.Id.status).Text = string.IsNullOrWhiteSpace (person.user_status_message) ? "Hey there, I'm on Walkr" : person.user_status_message;

			//var personImage = view.FindViewById<ImageView> (Resource.Id.personImage);

			//personImage.SetImageResource (Resource.Drawable.Placeholder);
			//No need to wait for the async download to return the view
			#pragma warning disable 4014
			//personImage.SetImageFromUrlAsync (product.ImageForSize (Images.ScreenWidth));
			#pragma warning restore 4014
			return view;
		}

		private double get_distance (string long_lat_acc_string)
		{
			var geo_value = long_lat_acc_string.ToGeoCoordinateFromLongLatAccString ();

			return geo_value.DistanceFrom (current_location);
		}
	}

}

