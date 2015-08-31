using System;
using Android.Widget;
using Android.Content;
using Android.Util;
using Android.Views;
using System.Collections.Generic;
using rangr.droid;
using rangr.common;

namespace CustomViews
{
	public class EndlessListView : ListView
	{
		private View footer;
		private View load_more_button;
		private View load_more_progress;

		public event EventHandler<EventArgs> OnLoadMoreTriggered = delegate{};

		public void InitEndlessness (int layout_resId, int image_button_resid, int progress_indicator_resid)
		{
			LayoutInflater inflater = (LayoutInflater)base.Context.GetSystemService (Context.LayoutInflaterService);
			footer = (View)inflater.Inflate (layout_resId, null);

			this.AddFooterView (footer);

			load_more_button = FindViewById<View> (image_button_resid);

			load_more_progress = FindViewById<View> (progress_indicator_resid);

			load_more_progress.Visibility = ViewStates.Gone;
				
			load_more_button.Click += (object sender, EventArgs e) => {

				load_more_button.Visibility = ViewStates.Gone;
				load_more_progress.Visibility = ViewStates.Visible;

				OnLoadMoreTriggered (this, EventArgs.Empty);
			};
		}

		public void SetEndlessListLoaderComplete (bool more_data_available = true)
		{
			if (more_data_available) {
				load_more_button.Visibility = ViewStates.Visible;
			}
			load_more_progress.Visibility = ViewStates.Gone;
		}

		public EndlessListView (Context ctx, IAttributeSet attrs, int defStyle)
			: base (ctx, attrs, defStyle)
		{

		}

		public EndlessListView (Context context, IAttributeSet attrs)
			: base (context, attrs)
		{

		}

		public EndlessListView (Context context)
			: base (context)
		{
		}
	}
}

