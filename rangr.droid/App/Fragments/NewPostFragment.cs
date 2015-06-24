
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
using App.Common;

namespace rangr.droid
{
    public class NewPostFragment : VMFragment<NewPostViewModel>
    {
        public override string TitleLabel
        { 
            get
            {
                return GetString(Resource.String.new_post_title);
            } 
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.new_post, null);

            view.FindViewById<TextView>(Resource.Id.UserNameText)
                .SetText(view_model.CurrentUser.user_id.ToString()
                , TextView.BufferType.Normal);

            post_text_input = view.FindViewById<EditText>(Resource.Id.PostText);
            post_text_input.TextChanged += HandlePostTextChanged;


            return view;
        }

        private EditText post_text_input;
        private IMenuItem send_button;

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.new_post, menu);
            send_button = menu.FindItem(Resource.Id.send_post_menu_item);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            { 
                case Resource.Id.send_post_menu_item:
                    HandleSaveButtonClicked(item, EventArgs.Empty);
                    break;
            }

            return base.OnOptionsItemSelected(item);
        }

        private async void HandleSaveButtonClicked(object sender, EventArgs e)
        {
            send_button.SetEnabled(false);
            if (!string.IsNullOrWhiteSpace(view_model.PostText))
            {
                
                    var successful = await view_model.CreatePost();

                    if (successful)
                    {
                        hide_keyboard_for(post_text_input);
                        NewPostCreated();
                        return;
                    }
                
            }
            send_button.SetEnabled(true);
        }

        private void HandlePostTextChanged(object sender, EventArgs e)
        {
            var text = ((EditText)sender).Text;

            if (string.IsNullOrWhiteSpace(text))
            {
                send_button.SetEnabled(false);

            }
            else
            {
                view_model.PostText = text;

                send_button.SetEnabled(true);
            }

        }


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

