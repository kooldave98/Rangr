
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
using System.Threading.Tasks;

namespace droid_ui_lib
{
    /// <summary>
    /// An Activity that uses a retained Fragment for its implementation.
    /// </summary>
    public abstract class FragmentActivity<TFragment> : Activity where TFragment : FragmentBase, new()
    {
        /// <summary>
        /// The top-level fragment which manages the view and state for this activity.
        /// </summary>
        public FragmentBase Fragment { get; protected set; }

        protected string FragmentTag
        {
            get { return GetType().Name; }
        }

        /// <inheritdoc />
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Fragment = InitFragment();
        }

        protected FragmentBase InitFragment()
        {
            return FragmentBase.FindOrCreateFragment<TFragment>(this, FragmentTag, Android.Resource.Id.Content);
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

        /// <inheritdoc />
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            Fragment.OnActivityResult(requestCode, resultCode, data);
        }
    }
}

