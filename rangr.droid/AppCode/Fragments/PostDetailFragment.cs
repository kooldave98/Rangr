
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
    public class PostDetailFragment : VMFragment<PostDetailsViewModel>
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Activity.ActionBar.Title = string.Empty;

            var view = inflater.Inflate(Resource.Layout.post_detail, null);

            set_map();

            view.FindViewById<TextView>(Resource.Id.UserNameText).SetText(view_model.CurrentPost.user_display_name, TextView.BufferType.Normal);

            postTextLabel = view.FindViewById<TextView>(Resource.Id.PostTextLabel);

            postTextLabel.Text = view_model.CurrentPost.text;

            return view;
        }

        private void set_map()
        {
            var transaction = FragmentManager.BeginTransaction();

            transaction.Replace(Resource.Id.fragmentContainer, new PostMapFragment(view_model.CurrentPost));

            transaction.Commit();
        }

        private TextView postTextLabel;

        public PostDetailFragment(Post the_post)
        {
            post = the_post;
            view_model = new PostDetailsViewModel(post);
        }

        private Post post;
    }
}

