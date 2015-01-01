
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

namespace rangr.droid
{
    public class PostsFragment : VMFragment<FeedViewModel>
    {
        public override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            //Todo: Move hardcoded string to resource file
            Activity.Title = "Feed";
            //Activity.ActionBar.SetTitle(Resource.String.app_name);

            RetainInstance = true;
            SetHasOptionsMenu(true);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.posts, null);

            postListView = view.FindViewById<EndlessListView>(Resource.Id.list);

            postListView.EmptyView = view.FindViewById<View>(Android.Resource.Id.Empty);
            postListView.InitEndlessness(Resource.Layout.loading, Resource.Id.loadMoreButton, Resource.Id.loadMoreProgress);

            postListView.OnLoadMoreTriggered += async (sender, e) =>
            {
                await view_model.OlderPosts();
                JavaScriptTimer.SetTimeout(delegate
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
                await view_model.RefreshPosts();
                JavaScriptTimer.SetTimeout(delegate
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

                    JavaScriptTimer.SetTimeout(delegate
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
            menu.Add("New Post").SetShowAsAction(ShowAsAction.IfRoom);
            inflater.Inflate(Resource.Menu.main, menu);
            //var cartItem = menu.FindItem(Resource.Id.cart_menu_item);

            base.OnCreateOptionsMenu(menu, inflater);
        }

        public PostsFragment()
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
}

