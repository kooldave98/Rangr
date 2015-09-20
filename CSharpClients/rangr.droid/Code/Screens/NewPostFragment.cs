
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
using Android.Content.PM;
using Android.Text;

namespace rangr.droid
{
    [Activity(Label = "@string/app_name"
            , ScreenOrientation = ScreenOrientation.Portrait)]         
    public class NewPostFragmentActivity : FragmentActivity<NewPostFragment>
    {
        public override bool OnNavigateUp()
        {
            base.OnNavigateUp();

            Finish();

            return true;
        }

        public override NewPostFragment InitFragment()
        {
            return new NewPostFragment();
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            ActionBar.SetDisplayHomeAsUpEnabled(true);

            Fragment.PostTextChanged += (txt) => {
                send_button.SetEnabled(!string.IsNullOrWhiteSpace(txt));
            };
        }

        private IMenuItem send_button;

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            base.OnCreateOptionsMenu(menu);

            MenuInflater.Inflate(Resource.Menu.new_post, menu);
            send_button = menu.FindItem(Resource.Id.send_post_menu_item);

            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            { 
                case Resource.Id.send_post_menu_item:
                    send_button.SetEnabled(false);
                    Fragment.HandleSaveButtonClicked(item, EventArgs.Empty);
                    send_button.SetEnabled(true);
                    break;
            }

            return base.OnOptionsItemSelected(item);
        }
    }

    public class NewPostFragment : VMFragment<NewPostViewModel>
    {
        public override string TitleLabel { 
            get {
                return GetString(Resource.String.new_post_title);
            } 
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.new_post, null);

            view.FindViewById<TextView>(Resource.Id.UserNameText).SetText(view_model.CurrentUser.user_display_name, TextView.BufferType.Normal);

            post_text_input = view.FindViewById<EditText>(Resource.Id.PostText);
            post_text_input.TextChanged += (object sender, TextChangedEventArgs e) => {
                var text = ((EditText)sender).Text;

                PostTextChanged(text);
                view_model.PostText = text;
            };

            return view;
        }

        private EditText post_text_input;

        //All this logic can be pushed into the ViewModel
        public async void HandleSaveButtonClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(view_model.PostText))
            {

                if (AppGlobal.Current.IsGeoLocatorRefreshed)
                {
                    var successful = await view_model.CreatePost();

                    if (successful)
                    {
                        hide_keyboard_for(post_text_input);
                        NewPostCreated();
                        return;
                    }
                }
                else
                {
                    AppEvents.Current.TriggerGeolocatorFailedEvent();
                }
            }
        }

        public event Action<string> PostTextChanged = delegate{};

        public NewPostFragment()
        {
            if (view_model == null)
            {
                view_model = new NewPostViewModel();
            }
        }

        public event Action NewPostCreated = delegate {};
    }
}