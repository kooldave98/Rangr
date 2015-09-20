
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
using rangr.common;
using Android.Views.InputMethods;
using AndroidHUD;
using solid_lib;
using System.Threading.Tasks;
using System.Threading;

namespace rangr.droid
{
    /// <summary>
    /// Investigate using custom notifications/toasts using the below library
    /// https://github.com/Redth/AndHUD
    /// </summary>
    public abstract class BaseFragment : FragmentBase
    {
        public abstract string TitleLabel { get; }

        protected void hide_keyboard_for(EditText edit_text)
        {
            var imm = (InputMethodManager)this.Activity.GetSystemService(Context.InputMethodService);
            imm.HideSoftInputFromWindow(edit_text.WindowToken, HideSoftInputFlags.NotAlways);
        }

        protected void show_progress(string title = "Loading...", string message = "Busy")
        {
            this.
            Activity.RunOnUiThread(() => {
                progress = ProgressDialog.Show(this.Activity, title, message, true);
            });

        }

        protected void dismiss_progress()
        {
            this.Activity.RunOnUiThread(() => {
                if (progress != null)
                    progress.Dismiss();
            });

        }

        protected void ShowToast(string message)
        {
//            if (!is_paused)
//            {
            this.Activity.RunOnUiThread(() => {
//                    if (true)
//                    {
                AndHUD.Shared.ShowToast(this.Activity, message, MaskType.Clear, TimeSpan.FromSeconds(5));


//                        var t = Toast.MakeText(this.Activity, message, ToastLength.Long);
//                        t.SetGravity(GravityFlags.Center, 0, 0);
//                        t.Show();
//                    }
            });
            //}

        }

        protected void ShowAlert(string title, string message, string ok_button_text = "Ok", Action ok_button_action = null)
        {
//            if (!is_paused)
//            {
            var builder = new AlertDialog.Builder(this.Activity)
                    .SetTitle(title)
                    .SetMessage(message)
                    .SetPositiveButton(ok_button_text, (innerSender, innere) => {
                                                            Activity.RunOnUiThread(() => {
                                                                if (ok_button_action != null)
                                                                {
                                                                    ok_button_action();
                                                                }
                                                            }
                    );

            });
            var dialog = builder.Create();
            dialog.Show();
            //}
        }

        protected void notify(String methodName)
        {
            var name = Activity.LocalClassName;

            var noti = new Notification.Builder(this.Activity)
                .SetContentTitle(methodName + " " + name).SetAutoCancel(true)
                .SetSmallIcon(rangr.droid.Resource.Drawable.Icon)
                .SetContentText(name).Build();

            var notificationManager = (NotificationManager)Activity.GetSystemService(Activity.NotificationService);
            notificationManager.Notify((int)CurrentTimeMillis(), noti);
        }

        private static readonly DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        //Search online for C# equivalent of Javas CurrentTimeMillis
        private static long CurrentTimeMillis()
        {
            return (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
        }


        private ProgressDialog progress;

    }

    public abstract class VMFragment<V> : BaseFragment where V : ViewModelBase
    {
        public static string INIT_ARG_KEY = "ARG_KEY";
        protected virtual void Initialise() {/*Meant to be optionally overriden*/}

        public override void OnCreate(Bundle bundle)
        {
            Initialise();

            Guard.IsNotNull(view_model, "view_model");
            //notify ("OnCreate");
            base.OnCreate(bundle);
            SetHasOptionsMenu(true);
        }

        public Action SetupActionBarNavigation = delegate {
        };

        public override void OnResume()
        {
            //notify ("OnResume");
            base.OnResume();

            Activity.ActionBar.Title = TitleLabel;

            SetupActionBarNavigation();

            isBusyChangedEventHandler = (sender, e) => {

                if (view_model.IsBusy)
                {
                    show_progress();
                }
                else
                {
                    dismiss_progress();
                }
            };

            view_model.IsBusyChanged += isBusyChangedEventHandler;

            ConnectionFailedHandler = (sender, e) => {
                ShowToast("An attempt to establish a network connection failed.");
            };

            GeolocatorFailedHandler = (sender, e) => {
                ShowToast("An attempt to retrieve your geolocation failed. " +
                "\n Please set your location mode on your phone's location settings to HIGH ACCURRACY");
            };

            AppEvents.Current.ConnectionFailed += ConnectionFailedHandler;

            AppEvents.Current.GeolocatorFailed += GeolocatorFailedHandler;

            view_model.ResumeState();

            AppGlobal.Current.Resume(this.Activity);


            is_paused = false;
        }

        public override void OnPause()
        {
            //notify ("OnPause");
            base.OnPause();

            view_model.IsBusyChanged -= isBusyChangedEventHandler;

            view_model.PauseState();

            dismiss_progress();

            //Potentially easier way to unsubscribe event handlers
            //http://www.h3mm3.com/2011/06/unsubscribing-to-events-in-c.html

            AppEvents.Current.ConnectionFailed -= ConnectionFailedHandler;

            AppEvents.Current.GeolocatorFailed -= GeolocatorFailedHandler;

            AppGlobal.Current.Pause(this.Activity);

            is_paused = true;

            ////I think this is a quick and dirty way to prevent crashes really.
            ////If every activity is loaded fresh and killed everytime, then there'll be nothing that can crash, haha
            ////Only problem is this may be resource intensive.
            //Finish();
        }

        private bool is_paused = false;

        private EventHandler isBusyChangedEventHandler;

        private EventHandler<AppEventArgs> ConnectionFailedHandler;

        private EventHandler<AppEventArgs> GeolocatorFailedHandler;

        protected V view_model;
    }

    /// <summary>
    /// The base class for top-level fragments in Android. These are the fragments which maintain the view hierarchy and state for each top-level
    /// Activity. These fragments all use RetainInstance = true to allow them to maintain state across configuration changes (i.e.,
    /// when the device rotates we reuse the fragments). Activity classes are basically just dumb containers for these fragments.
    /// </summary>
    public abstract class FragmentBase : Fragment
    {
        /// <summary>
        /// Tries to locate an already created fragment with the given tag. If the fragment is not found then a new one will be created and inserted into
        /// the given activity using the given containerId as the parent view.
        /// </summary>
        /// <typeparam name="TFragment">The type of fragment to create.</typeparam>
        /// <param name="activity">The activity to search for or create the view in.</param>
        /// <param name="fragmentTag">The tag which uniquely identifies the fragment.</param>
        /// <param name="containerId">The resource ID of the parent view to use for a newly created fragment.</param>
        /// <returns>The found or created fragment.</returns>
        public static TFragment FindOrCreateFragment<TFragment>(FragmentActivity<TFragment> activity, string fragmentTag, int containerId) where TFragment : FragmentBase
        {
            var fragment = activity.FragmentManager.FindFragmentByTag(fragmentTag) as TFragment;
            if (fragment == null)
            {
                fragment = activity.InitFragment();
                SwapFragment(activity, fragment, fragmentTag, containerId);
            }

            return fragment;
        }

        public static void SwapFragment<TFragment>(Activity activity, TFragment fragment, string fragmentTag, int containerId) where TFragment : FragmentBase
        {
            activity.FragmentManager.BeginTransaction().Add(containerId, fragment, fragmentTag).Commit();
        }

        /// <inheritdoc />
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            RetainInstance = true;
        }

        /// <summary>
        /// Called when this fragment's activity is given a new Intent.
        /// </summary>
        /// <remarks>The default implementation does nothing</remarks>
        public virtual void OnNewIntent(Intent intent)
        {
        }

        /// <summary>
        /// Called when this fragment's activity is attached to a window.
        /// </summary>
        /// <remarks>The default implementation does nothing</remarks>
        public virtual void OnAttachedToWindow()
        {
        }
    }


    /// <summary>
    /// An Activity that uses a retained Fragment for its implementation.
    /// </summary>
    public abstract class FragmentActivity<TFragment> : Activity where TFragment : FragmentBase
    {
        #region "This bit really shouldn't be mixed with the rest, don't wanna contamite something that should really be a library type stuff"

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            if (this.GetType() != typeof(LoginFragmentActivity))
            {
                MenuInflater.Inflate(Resource.Menu.menu, menu);
            }

            return base.OnCreateOptionsMenu(menu);
        }


        public override bool OnMenuItemSelected(int featureId, IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.feed_menu_item:
                    ResurrectActivity(typeof(PostListFragmentActivity));
                    break;
                case Resource.Id.people_menu_item:
                    ResurrectActivity(typeof(PeopleFragmentActivity));
                    break;
                case Resource.Id.profile_menu_item:
                    ResurrectActivity(typeof(ProfileActivity));
                    break;
                case Resource.Id.simulation_menu_item:
                    ResurrectActivity(typeof(SimulationActivity));
                    break;
                case Resource.Id.settings_menu_item:
                    ResurrectActivity(typeof(AboutAppActivity));
                    break;
            }

            return base.OnMenuItemSelected(featureId, item);
        }

        protected void ResurrectActivity(Type activityType)
        {
            var i = new Intent(Application.Context, activityType);
            i.SetFlags(ActivityFlags.ReorderToFront);
            StartActivity(i);
        }

        #endregion

        /// <summary>
        /// The top-level fragment which manages the view and state for this activity.
        /// </summary>
        public TFragment Fragment { get; protected set; }

        /// <summary>
        /// The tag string to use when finding or creating this activity's fragment. This will be contructed using the type of this generic instance.
        /// </summary>
        protected string FragmentTag {
            get {
                return GetType().Name;
            }
        }

        public abstract TFragment InitFragment();

        /// <inheritdoc />
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Fragment = FragmentBase.FindOrCreateFragment<TFragment>(this, FragmentTag, Android.Resource.Id.Content);
        }

        protected override async void OnDestroy()
        {
            base.OnDestroy();

            await Task.Yield();

            GC.Collect();
        }

        /// <inheritdoc />
        public override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();
            Fragment.OnAttachedToWindow();
        }

        /// <inheritdoc />
        protected override void OnNewIntent(Intent intent)
        {
            Fragment.OnNewIntent(intent);
        }

        // This is an arbitrary number to use as an initial request code for StartActivityForResultAsync.
        // It just needs to be high enough to avoid collisions with direct calls to StartActivityForResult, which typically would be 0, 1, 2...
        private const int FirstAsyncActivityRequestCode = 1000;

        // This is static so that they are unique across all implementations of FragmentBase.
        // This is important for the fragment initializer overloads of StartActivityForResultAsync.
        private static int _nextAsyncActivityRequestCode = FirstAsyncActivityRequestCode;
        private readonly Dictionary<int, AsyncActivityResult> _pendingAsyncActivities = new Dictionary<int, AsyncActivityResult>();
        private readonly List<AsyncActivityResult> _finishedAsyncActivityResults = new List<AsyncActivityResult>();



        #region Async Activity API

        public Task<IAsyncActivityResult> StartActivityForResultAsync<TActivity>(CancellationToken cancellationToken = default(CancellationToken))
        {
            return StartActivityForResultAsyncCore(requestCode => StartActivityForResult(typeof(TActivity), requestCode), cancellationToken);
        }

        public Task<IAsyncActivityResult> StartActivityForResultAsync(Intent intent, CancellationToken cancellationToken = default(CancellationToken))
        {
            return StartActivityForResultAsyncCore(requestCode => StartActivityForResult(intent, requestCode), cancellationToken);
        }

        /// <inheritdoc />
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            AsyncActivityResult result;
            if (_pendingAsyncActivities.TryGetValue(requestCode, out result))
            {
                result.SetResult(resultCode, data);
                _pendingAsyncActivities.Remove(requestCode);
                _finishedAsyncActivityResults.Add(result);
            }

            base.OnActivityResult(requestCode, resultCode, data);
        }

        protected override void OnResume()
        {
            base.OnResume();

            FlushPendingAsyncActivityResults();
        }

        private Task<IAsyncActivityResult> StartActivityForResultAsyncCore(Action<int> startActivity, CancellationToken cancellationToken)
        {
            var asyncActivityResult = SetupAsyncActivity();
            startActivity(asyncActivityResult.RequestCode);

            if (cancellationToken.CanBeCanceled)
            {
                cancellationToken.Register(() => {
                    FinishActivity(asyncActivityResult.RequestCode);
                });
            }

            return asyncActivityResult.Task;
        }

        private void FlushPendingAsyncActivityResults()
        {
            foreach (var result in _finishedAsyncActivityResults)
            {
                result.Complete();
            }
            _finishedAsyncActivityResults.Clear();
        }

        private AsyncActivityResult SetupAsyncActivity()
        {
            var requestCode = _nextAsyncActivityRequestCode++;
            var result = new AsyncActivityResult(requestCode);
            _pendingAsyncActivities.Add(requestCode, result);

            return result;
        }

        private class AsyncActivityResult : IAsyncActivityResult
        {
            private readonly TaskCompletionSource<IAsyncActivityResult> _taskCompletionSource = new TaskCompletionSource<IAsyncActivityResult>();

            public int RequestCode { get; private set; }

            public Result ResultCode { get; private set; }

            public Intent Data { get; private set; }

            public Task<IAsyncActivityResult> Task { get { return _taskCompletionSource.Task; } }

            public AsyncActivityResult(int requestCode)
            {
                RequestCode = requestCode;
            }

            public void SetResult(Result resultCode, Intent data)
            {
                ResultCode = resultCode;
                Data = data;
            }

            public void Complete()
            {
                _taskCompletionSource.SetResult(this);
            }
        }

        #endregion
    }

    /// <summary>
    /// The information returned by an Activity started with StartActivityForResultAsync.
    /// </summary>
    public interface IAsyncActivityResult
    {
        /// <summary>
        /// The result code returned by the activity.
        /// </summary>
        Result ResultCode { get; }

        /// <summary>
        /// The data returned by the activity.
        /// </summary>
        Intent Data { get; }
    }

}