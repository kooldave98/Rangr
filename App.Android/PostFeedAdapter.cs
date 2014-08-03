using System;
using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;
using App.Core.Portable.Models;

namespace App.Android
{
	public class PostFeedAdapter : BaseAdapter<SeenPost>
	{
		Activity context = null;
		IList<SeenPost> _posts = new List<SeenPost>();

		public PostFeedAdapter (Activity context, IList<SeenPost> posts) : base ()
		{
			this.context = context;
			this._posts = posts;
		}

		public override SeenPost this[int position]
		{
			get { return _posts[position]; }
		}

		public override long GetItemId (int position)
		{
			return position;
		}

		public override int Count
		{
			get { return _posts.Count; }
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			// Get our object for position
			var item = _posts[position];			

			//Try to reuse convertView if it's not  null, otherwise inflate it from our item layout
			// gives us some performance gains by not always inflating a new view
			// will sound familiar to MonoTouch developers with UITableViewCell.DequeueReusableCell()
			var view = (convertView ?? 
				context.LayoutInflater.Inflate(
					Resource.Layout.PostListItem, 
					parent, 
					false)) as LinearLayout;

			// Find references to each subview in the list item's view
			var txtName = view.FindViewById<TextView>(Resource.Id.UserNameText);
			var txtDescription = view.FindViewById<TextView>(Resource.Id.PostText);

			//Assign item's values to the various subviews
			txtName.SetText (item.user_display_name, TextView.BufferType.Normal);
			txtDescription.SetText (item.text, TextView.BufferType.Normal);
			//view.FindViewById<ImageView>(Resource.Id.UserImageButton).SetImageResource(Resource.Drawable.Placeholder);
			//Finally return the view
			return view;
		}
	}
}

