using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using droid_ui_lib;

namespace experiments.droid
{
    [Activity(Label = "FragmentActivityTest", MainLauncher = true, Icon = "@drawable/icon")]
    public class StartActivity : FragmentActivity<StartActivity.HostedFragment>
    {
        public class HostedFragment : FragmentBase
        {
            private string _text;
            private Button _button;
            private TextView _textView;

            public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
            {
                var view = inflater.Inflate(Resource.Layout.start, container, attachToRoot: false);

                _button = view.FindViewById<Button>(Resource.Id.Button);
                _textView = view.FindViewById<TextView>(Resource.Id.TextView);

                UpdateText();

                _button.Click += async (s, e) =>
                    {
                        var result = await StartActivityForResultAsync<AsyncActivity>();
                        if (result.ResultCode == Result.Ok)
                        {
                            _text = result.Data.GetStringExtra(AsyncActivity.TextExtra);
                            UpdateText();
                        }
                    };

                return view;
            }

            public override void OnDestroyView()
            {
                base.OnDestroyView();

                _button = null;
                _textView = null;
            }

            private void UpdateText()
            {
                _textView.Text = _text;
            }
        }
    }
}
