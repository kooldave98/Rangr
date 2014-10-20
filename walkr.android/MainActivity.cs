using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace walkr.android
{
	[Activity (Label = "Home", Icon = "@drawable/icon")]
	public class MainActivity : Activity, Android.App.ActionBar.IOnNavigationListener
	{
		int count = 1;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.main_layout);

			strings = Resources.GetStringArray (Resource.Array.action_list);

			ActionBar.NavigationMode = ActionBarNavigationMode.List;
			ActionBar.Title = "";

			var adapter = new ActionBarSpinnerAdapter (this, Resource.Layout.spinner_item, strings);

//			var adapter = ArrayAdapter.CreateFromResource (this,
//				              Resource.Array.action_list, Resource.Layout.dropdownItem);

			ActionBar.SetListNavigationCallbacks (adapter, this);

			Button button = FindViewById<Button> (Resource.Id.myButton);
			
			button.Click += delegate {
				button.Text = string.Format ("{0} clicks!", count++);
			};
		}

		// Get the same strings provided for the drop-down's ArrayAdapter
		string[] strings;

		public bool OnNavigationItemSelected (int position, long itemId)
		{
			// Create new fragment from our own Fragment class
			var newFragment = new ListContentFragment ();

			FragmentTransaction ft = FragmentManager.BeginTransaction ();

			// Replace whatever is in the fragment container with this fragment
			// and give the fragment a tag name equal to the string at the position
			// selected
			ft.Replace (Resource.Id.fragmentContainer, newFragment, strings [position]);

			// Apply changes
			ft.Commit ();
			return true;
		}

	}
	//See below url for styling the spinner properly
	//http://stackoverflow.com/questions/17613912/styling-actionbar-spinner-navigation-to-look-like-its-title-and-subtitle#_=_
	public class ActionBarSpinnerAdapter : ArrayAdapter<string>, ISpinnerAdapter
	{
		private Activity context;
		//private int textViewResourceId;
		private string[] items;

		public ActionBarSpinnerAdapter (Activity context, int textViewResourceId, string[] items)
			: base (context, textViewResourceId, items)
		{
			this.context = context;	
			//this.textViewResourceId = textViewResourceId;
			this.items = items;	
		}

		public string this [int position] {
			get { return items [position]; }
		}

		public override long GetItemId (int position)
		{
			return position;
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			// Get our object for position
			var item = items [position];
	
			//Try to reuse convertView if it's not  null, otherwise inflate it from our item layout
			// gives us some performance gains by not always inflating a new view
			// will sound familiar to MonoTouch developers with UITableViewCell.DequeueReusableCell()
			var view = (convertView ??
			           context.LayoutInflater.Inflate (
				           Resource.Layout.spinner_item,
				           parent,
				           false)) as LinearLayout;
	
			// Find references to each subview in the list item's view
			var txtName = view.FindViewById<TextView> (Resource.Id.action_bar_title);
			var txtDescription = view.FindViewById<TextView> (Resource.Id.action_bar_subtitle);
	
			//Assign item's values to the various subviews
			txtName.SetText (context.Title, TextView.BufferType.Normal);
			txtDescription.SetText (item, TextView.BufferType.Normal);
	
			return view;
		}

	
		public override View GetDropDownView (int position, View convertView, ViewGroup parent)
		{
			DropDownViewHolder holder = null;
	
			if (convertView == null) {
				LayoutInflater inflater = (context).LayoutInflater;
				convertView = inflater.Inflate (Resource.Layout.drop_down_item, parent, false);
	
				holder = new DropDownViewHolder ();
				holder.mTitle = (TextView)convertView.FindViewById (Resource.Id.spinner_title);
	
				convertView.Tag = (Java.Lang.Object)holder;
			} else {
				holder = (DropDownViewHolder)convertView.Tag;
			}
	
			holder.mTitle.SetText (items [position], TextView.BufferType.Normal);
	
			return convertView;
		}

		public class DropDownViewHolder : Java.Lang.Object
		{
			public TextView mTitle { get; set; }
		}

		public override int Count {
			get { return items.Length; }
		}
	
	}

	public class ListContentFragment : Fragment
	{
		private string mText;


		public override void OnAttach (Activity activity)
		{
			// This is the first callback received; here we can set the text for
			// the fragment as defined by the tag specified during the fragment
			// transaction
			base.OnAttach (activity);
			mText = Tag;
		}


		public override View OnCreateView (LayoutInflater inflater, ViewGroup container,
		                                   Bundle savedInstanceState)
		{
			// This is called to define the layout for the fragment;
			// we just create a TextView and set its text to be the fragment tag
			TextView text = new TextView (Activity);
			text.Text = mText;
			return text;
		}
	}

}


