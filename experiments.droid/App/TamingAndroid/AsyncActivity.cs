using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using droid_ui_lib;

namespace experiments.droid
{
    [Activity (Label = "AsyncActivity")]
    public class AsyncActivity : FragmentActivity<AsyncActivity.HostedFragment>
    {
        public const string TextExtra = "Text";
    

        public class HostedFragment : FragmentBase
        {
            private EditText _editText;

            public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
            {
                var view = inflater.Inflate(Resource.Layout.async, container, attachToRoot: false);

                var button = view.FindViewById<Button>(Resource.Id.Button);
                _editText = view.FindViewById<EditText>(Resource.Id.EditText);

                button.Click += delegate {
                    var resultData = new Intent();
                    resultData.PutExtra(AsyncActivity.TextExtra, _editText.Text);
                    Activity.SetResult(Result.Ok, resultData);
                    Activity.Finish();
                };

                return view;
            }
        }
    }
}

