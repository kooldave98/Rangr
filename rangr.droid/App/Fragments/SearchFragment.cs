
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
using App.Common;
using Android.Support.V4.Widget;
using solid_lib;

namespace rangr.droid
{
    //Todo: Pull out a common abstract PostListFragment
    //and let there be a FeedFragment and a SearchFragment
    public class SearchFragment : VMFragment<SearchViewModel>
    {
        public override string TitleLabel
        { 
            get
            {
                return string.Format("Search: #{0}", view_model.hash_tag_search_keyword);
            } 
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.post_list, null);

            postListView = view.FindViewById<EndlessListView>(Resource.Id.list);

            postListView.EmptyView = view.FindViewById<View>(Android.Resource.Id.Empty);
            postListView.InitEndlessness(Resource.Layout.loadMore, Resource.Id.loadMoreButton, Resource.Id.loadMoreProgress);

            postListView.OnLoadMoreTriggered += async (sender, e) =>
            {
                //await view_model.OlderPosts ();
                JSTimer.SetTimeout(delegate
                    {
                        Activity.RunOnUiThread(() =>
                            {
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
            refresher.SetColorScheme(Resource.Color.xam_dark_blue, Resource.Color.xam_purple, 
                Resource.Color.xam_gray, Resource.Color.xam_green);

            refresher.Refresh += async (sender, e) =>
            {
                //await view_model.RefreshPosts ();
                JSTimer.SetTimeout(delegate
                    {
                        Activity.RunOnUiThread(() =>
                            {
                                refresher.Refreshing = false;
                            });
                    }, 1500);//1.5 secs

            };

            #endregion


            // wire up post click handler
            postListView.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) =>
            {
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

                NewPostsReceivedHandler = (object sender, EventArgs e) =>
                {
                    //Refresh list view data
                    Activity.RunOnUiThread(() =>
                        {
                            postListAdapter.NotifyDataSetChanged();
                        });
                };

                view_model.OnNewPostsReceived += NewPostsReceivedHandler;

                GeoLocatorRefreshedHandler = async (object sender, EventArgs e) =>
                {
                    await view_model.RefreshPosts();

                    JSTimer.SetTimeout(delegate
                        {
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

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.search, menu);
        }

        public SearchFragment(string the_hash_tag)
        {
            view_model = new SearchViewModel();
            view_model.hash_tag_search_keyword = the_hash_tag;
        }

        public SearchFragment()
        {
        }

        private PostsAdapter postListAdapter;
        private EndlessListView postListView;
        private SwipeRefreshLayout refresher;

        public event Action<Post> PostItemSelected = delegate {};
        public event Action<string> HashTagSelected = delegate{};
    }
}

