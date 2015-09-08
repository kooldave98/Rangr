
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

namespace rangr.droid
{
    [Activity (Label = "@string/app_name"
            , ScreenOrientation = ScreenOrientation.Portrait)]         
    public class PostDetailFragmentActivity : FragmentActivity<PostDetailFragment>
    {        
        public override bool OnNavigateUp ()
        {
            base.OnNavigateUp ();

            Finish ();

            return true;
        }

        public override PostDetailFragment LoadFragment()
        {
            Post post;
            if (Intent.HasExtra ("Post")) {
                var postBytes = Intent.GetByteArrayExtra ("Post");
                post = PostDetailsViewModel.Deserialize (postBytes);
            } else {
                post = new Post ();
            }

            return PostDetailFragment.newInstance(post);
        }

        protected override void OnCreate (Bundle bundle)
        {
            base.OnCreate(bundle);

            ActionBar.SetDisplayHomeAsUpEnabled(true);
        }

        public static Intent CreateIntent (Context context, Post post)
        {
            var postStream = PostDetailsViewModel.Serialize (post);

            var intent = new Intent (context, typeof(PostDetailFragmentActivity))
                .PutExtra ("Post", postStream.ToArray ());
            return intent;
        }
    }
    public class PostDetailFragment : VMFragment<PostDetailsViewModel>
    {
        public override string TitleLabel { 
            get {
                return string.Empty;
            } 
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
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

            transaction.Replace(Resource.Id.fragmentContainer, new PostDetailMapFragment(view_model.CurrentPost));

            transaction.Commit();
        }

        private TextView postTextLabel;

        protected override void Initialise()
        {
            var postBytes = Arguments.GetByteArray(INIT_ARG_KEY);
            post = PostDetailsViewModel.Deserialize (postBytes);
            view_model = new PostDetailsViewModel(post);
        }

        public static PostDetailFragment newInstance(Post a_post) 
        {
            var post_bytes = PostDetailsViewModel.Serialize(a_post).ToArray();

            var fragment = new PostDetailFragment(){
                Arguments = new Bundle()
                    .Chain(b=>b.PutByteArray(INIT_ARG_KEY, post_bytes))
            };
            return fragment;
        }

        private Post post;
    }
}

