
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
using solid_lib;
using Android.Content.PM;

namespace rangr.droid
{

    [Activity(Label = "@string/app_name",
        Icon = "@drawable/icon",
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class SearchFragmentActivity : FragmentActivity<SearchFragment>
    {
        private const string intent_name = "hashtag";

        public override SearchFragment InitFragment()
        {
            if (Intent.HasExtra(intent_name))
            {
                var hash_tag_name = Intent.GetStringExtra(intent_name);
                return SearchFragment.newInstance(hash_tag_name);
            }
            return SearchFragment.newInstance("NO_PARAMS");
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Title = "Search";

            if (!Intent.HasExtra(intent_name))
            {
                Finish();
            }

            ActionBar.SetDisplayHomeAsUpEnabled(true);  

            Fragment.HashTagSelected += (ht) => {
                StartActivity(typeof(SearchFragmentActivity));
            };

            Fragment.PostItemSelected += (p) => {
                var postDetails = PostDetailFragmentActivity.CreateIntent(this, p);
                StartActivity(postDetails);
            };
               
        }

        public override bool OnNavigateUp()
        {
            base.OnNavigateUp();

            Finish();

            return true;
        }

        public static Intent CreateIntent(Context context, string hash_tag)
        {
            return new Intent(context, typeof(SearchFragmentActivity)).PutExtra(intent_name, hash_tag);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            base.OnCreateOptionsMenu(menu);

            MenuInflater.Inflate(Resource.Menu.search, menu);

            return true;
        }
    }

    //Todo: Pull out a common abstract PostListFragment
    //and let there be a FeedFragment and a SearchFragment
    public class SearchFragment : VMFragment<SearchViewModel>
    {
        public override string TitleLabel { 
            get {
                return string.Format("Search: #{0}", view_model.hash_tag_search_keyword);
            } 
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.post_list, null);

            postListView = view.FindViewById<EndlessListView>(Resource.Id.list);

            postListView.EmptyView = view.FindViewById<View>(Android.Resource.Id.Empty);
            postListView.InitEndlessness(Resource.Layout.loadMore, Resource.Id.loadMoreButton, Resource.Id.loadMoreProgress);

            postListView.OnLoadMoreTriggered += (sender, e) => {
                //await view_model.OlderPosts ();
                JSTimer.SetTimeout(delegate {
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

            refresher.Refresh += (sender, e) => {
                //await view_model.RefreshPosts ();
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

            return view;
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

        protected override void Initialise()
        {
            var search_string = Arguments.GetString(INIT_ARG_KEY);
            view_model = new SearchViewModel();
            view_model.hash_tag_search_keyword = search_string;
        }

        public static SearchFragment newInstance(string the_hash_tag) 
        {           

            var fragment = new SearchFragment(){
                Arguments = new Bundle()
                                .Chain(b=>b.PutString(INIT_ARG_KEY, the_hash_tag))
                };
            return fragment;
        }

        private PostsAdapter postListAdapter;
        private EndlessListView postListView;
        private SwipeRefreshLayout refresher;

        public event Action<Post> PostItemSelected = delegate {};
        public event Action<string> HashTagSelected = delegate{};
    }
}

