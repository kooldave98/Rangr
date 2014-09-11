using System;
using Android.Widget;
using Android.Content;
using Android.Util;
using Android.Views;
using System.Collections.Generic;
using App.Android;
using App.Common;

namespace CustomViews
{
	public class EndlessListView : ListView, AbsListView.IOnScrollListener
	{
		private View footer;
		private bool isLoading;

		public event EventHandler<EventArgs> OnListEndReached = delegate{};

		public void SetListEndLoadingView (int resId)
		{
			LayoutInflater inflater = (LayoutInflater)base.Context.GetSystemService (Context.LayoutInflaterService);
			footer = (View)inflater.Inflate (resId, null);
		}


		public void OnScroll (AbsListView view, int firstVisibleItem, int visibleItemCount, int totalItemCount)
		{
//			if (Adapter == null)
//				return;
//
//			if (Adapter.Count == 0)
//				return;
//
//			int l = visibleItemCount + firstVisibleItem;
//			if (l >= totalItemCount && !isLoading) {
//				// It is time to add new data. We call the listener
//
//				this.AddFooterView (footer);
//				isLoading = true;
//
//				OnListEndReached (this, EventArgs.Empty);
//
//			}
		}

		private const int threshold = 0;

		public void OnScrollStateChanged (AbsListView view, ScrollState scrollState)
		{
			if (scrollState == ScrollState.Idle) {
				if (view.LastVisiblePosition >= view.Count - 1 - threshold) {
					if (!isLoading) {
						isLoading = true;
						this.AddFooterView (footer);
						OnListEndReached (this, EventArgs.Empty);
					}
				}
			}
		}

		public void SetEndlessListLoaderComplete ()
		{
			this.RemoveFooterView (footer);
			isLoading = false;
		}

		public EndlessListView (Context ctx, IAttributeSet attrs, int defStyle)
			: base (ctx, attrs, defStyle)
		{
			this.SetOnScrollListener (this);
		}

		public EndlessListView (Context context, IAttributeSet attrs)
			: base (context, attrs)
		{
			this.SetOnScrollListener (this);
		}

		public EndlessListView (Context context)
			: base (context)
		{					
			this.SetOnScrollListener (this);
		}
	}
}

