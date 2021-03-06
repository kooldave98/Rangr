﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

using CustomViews;
using rangr.common;
using Android.Support.V4.Widget;
using Android.Text;
using System.Text.RegularExpressions;
using solid_lib;
using Android.Content.PM;

namespace rangr.droid
{
    [Activity(Label = "@string/app_name",
        Icon = "@drawable/icon",
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class PostListFragmentActivity : MainFragmentActivity
    {
        private PostListFragment the_fragment;
        public PostListFragment Fragment
        {
            get{
                return the_fragment ??
                    (the_fragment = new PostListFragment());
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            PushFragment(Fragment);

            Fragment.HashTagSelected += (ht) => {
                StartActivity(typeof(SearchFragmentActivity));
            };

            Fragment.PostItemSelected += (p) => {
                var postDetails = PostDetailFragmentActivity.CreateIntent(this, p);
                StartActivity(postDetails);
            };
        }


    }

    public class PostListFragment : VMFragment<FeedViewModel>
    {
        public override string TitleLabel { 
            get {
                return GetString(Resource.String.posts_title);
            } 
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.post_list, null);
            return view;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            postListView = view.FindViewById<EndlessListView>(Resource.Id.list);

            postListView.EmptyView = view.FindViewById<View>(Android.Resource.Id.Empty);
            postListView.InitEndlessness(Resource.Layout.loadMore, Resource.Id.loadMoreButton, Resource.Id.loadMoreProgress);

            postListView.OnLoadMoreTriggered += async (sender, e) => {
                await view_model.OlderPosts();
                JSTimer.SetTimeout(() => {
                    Activity.RunOnUiThread(() => {
                        ((EndlessListView)sender).SetEndlessListLoaderComplete();
                    });
                }, 1500);//1.5 secs
            };

            #region setup list adapter
            postListAdapter = new PostsAdapter(this.Activity, view_model.Posts);

            postListAdapter.on_hash_tag_selected += HashTagSelected;

            postListView.Adapter = postListAdapter;
            #endregion


            #region Setup pull to refresh

            refresher = view.FindViewById<SwipeRefreshLayout>(Resource.Id.refresher);

            refresher.Refresh += async (sender, e) => {
                await view_model.RefreshPosts();
                JSTimer.SetTimeout(delegate {
                    Activity.RunOnUiThread(() => {
                        refresher.Refreshing = false;
                    });
                }, 1500);//1.5 secs

            };

            #endregion


            // wire up post click handler
            postListView.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => {
                var post = view_model.Posts[e.Position];
                PostItemSelected(post);
            };
        }

        private EventHandler<EventArgs> NewPostsReceivedHandler;

        private EventHandler<EventArgs> GeoLocatorRefreshedHandler;

        private void show_refresher()
        {
            refresher.Refreshing = true;
        }

        private void dismiss_refresher()
        {
            refresher.Refreshing = false;
        }

        public override void OnResume()
        {
            //Doing this before to prevent the blank screen
            //cause the base can take a while
            show_refresher();

            base.OnResume();

            if (AppGlobal.Current.CurrentUserAndConnectionExists)
            {

                NewPostsReceivedHandler = (object sender, EventArgs e) => {
                    //Refresh list view data
                    Activity.RunOnUiThread(() => {
                        postListAdapter.NotifyDataSetChanged();
                    });
                };

                view_model.OnNewPostsReceived += NewPostsReceivedHandler;

                GeoLocatorRefreshedHandler = async (object sender, EventArgs e) => {
                    await view_model.RefreshPosts();

                    JSTimer.SetTimeout(delegate {
                        dismiss_refresher();
                    }, 500);//1/2 a second
                };

                AppGlobal.Current.GeoLocatorRefreshed += GeoLocatorRefreshedHandler;
            }
            //Simulation
            var persisted_simulation = PersistentStorage.Current.Load<string>("simulation");
            if (!string.IsNullOrWhiteSpace(persisted_simulation) && persisted_simulation != "L")
            {
                ShowToast("Simulated location" + persisted_simulation);
            }

        }

        public override void OnPause()
        {
            base.OnPause();
            dismiss_refresher();
            view_model.OnNewPostsReceived -= NewPostsReceivedHandler;
            AppGlobal.Current.GeoLocatorRefreshed -= GeoLocatorRefreshedHandler;
        }

        public PostListFragment()
        {
            if (view_model == null)
            {
                view_model = new FeedViewModel();
            }     
        }

        private PostsAdapter postListAdapter;
        private EndlessListView postListView;
        private SwipeRefreshLayout refresher;

        public event Action<Post> PostItemSelected = delegate {};
        public event Action<string> HashTagSelected = delegate {};
    }

    public class PostsAdapter : BaseAdapter<Post>
    {
        Activity context = null;
        IList<Post> _posts = new List<Post>();

        public PostsAdapter(Activity context, IList<Post> posts)
            : base()
        {
            this.context = context;
            this._posts = posts;
        }

        public override Post this [int position] {
            get { return _posts[position]; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override int Count {
            get { return _posts.Count; }
        }

        public event Action<string> on_hash_tag_selected = delegate{};

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            // Get our object for position
            var item = _posts[position];            

            //Try to reuse convertView if it's not  null, otherwise inflate it from our item layout
            // gives us some performance gains by not always inflating a new view
            // will sound familiar to MonoTouch developers with UITableViewCell.DequeueReusableCell()
            var view = (convertView ??
                       context.LayoutInflater.Inflate(
                           Resource.Layout.post_list_item, 
                           parent, 
                           false)) as LinearLayout;

            view.DescendantFocusability = DescendantFocusability.BlockDescendants;


            // Find references to each subview in the list item's view
            var txtName = view.FindViewById<TextView>(Resource.Id.name);
            var txtDescription = view.FindViewById<TextView>(Resource.Id.txtStatusMsg);

            //Assign item's values to the various subviews
            txtName.SetText(item.user_display_name, TextView.BufferType.Normal);

            List<int[]> hashtagSpans = getSpans(item.text, '#');

            var postContent = new SpannableString(item.text);

            for (int i = 0; i < hashtagSpans.Count; i++)
            {
                int[] span = hashtagSpans[i];
                int hashTagStart = span[0];
                int hashTagEnd = span[1];

                postContent.SetSpan(new AClickableSpan(on_hash_tag_selected), hashTagStart, hashTagEnd, 0);

            }

            //txtDescription.MovementMethod = LinkMovementMethod.Instance;
            txtDescription.SetText(postContent, TextView.BufferType.Normal);
            txtDescription.SetOnTouchListener(new TextViewHyperlinkOnTouchListener());

            view.FindViewById<TextView>(Resource.Id.timestamp)
                .SetText(TimeAgoConverter.Current.Convert(item.date), TextView.BufferType.Normal);

            view.FindViewById<ImageView>(Resource.Id.profilePic).SetImageResource(Resource.Drawable.user_default_avatar);
            Koush.UrlImageViewHelper
                .SetUrlDrawable (view.FindViewById<ImageView>(Resource.Id.feedImage1)
                                , string.Format("{0}/images/{1}",Resources.baseUrl, item.image_id));
            //Finally return the view
            return view;
        }

        public List<int[]> getSpans(string body, char prefix)
        {
            List<int[]> spans = new List<int[]>();

            Regex pattern = new Regex(prefix + "\\w+");
            MatchCollection matches = pattern.Matches(body);

            foreach (Match match in matches)
            {
                var m = match.Groups[0];
                // Check all occurrences

                int[] currentSpan = new int[2];
                currentSpan[0] = m.Index;
                currentSpan[1] = currentSpan[0] + m.Length;
                spans.Add(currentSpan);
            }
            return  spans;
        }


    }

}

