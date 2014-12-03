using System;
using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;
using App.Common;
using CustomViews;
using System.Text.RegularExpressions;
using Android.Text.Style;
using Android.Content;
using Android.Text;
using Android.Text.Method;

namespace App.Android
{
    public class PostFeedAdapter : BaseAdapter<Post>
    {
        Activity context = null;
        IList<Post> _posts = new List<Post>();

        public PostFeedAdapter(Activity context, IList<Post> posts)
            : base()
        {
            this.context = context;
            this._posts = posts;
        }

        public override Post this [int position]
        {
            get { return _posts[position]; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override int Count
        {
            get { return _posts.Count; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            // Get our object for position
            var item = _posts[position];			

            //Try to reuse convertView if it's not  null, otherwise inflate it from our item layout
            // gives us some performance gains by not always inflating a new view
            // will sound familiar to MonoTouch developers with UITableViewCell.DequeueReusableCell()
            var view = (convertView ??
                       context.LayoutInflater.Inflate(
                           Resource.Layout.PostListItem, 
                           parent, 
                           false)) as LinearLayout;

            view.DescendantFocusability = DescendantFocusability.BlockDescendants;


            // Find references to each subview in the list item's view
            var txtName = view.FindViewById<TextView>(Resource.Id.name);
            var txtDescription = view.FindViewById<TextView>(Resource.Id.txtStatusMsg);

            //Assign item's values to the various subviews
            txtName.SetText(item.user_display_name, TextView.BufferType.Normal);

            List<int[]> hashtagSpans = getSpans(item.text, '#');

            var postContent = new SpannableString(item.text);

            for (int i = 0; i < hashtagSpans.Count; i++)
            {
                int[] span = hashtagSpans[i];
                int hashTagStart = span[0];
                int hashTagEnd = span[1];

                postContent.SetSpan(new AClickableSpan((tag) =>
                        {
                            context.StartActivity(SearchActivity.CreateIntent(context, tag));
                        }), hashTagStart, hashTagEnd, 0);

            }

            //txtDescription.MovementMethod = LinkMovementMethod.Instance;
            txtDescription.SetText(postContent, TextView.BufferType.Normal);
            txtDescription.SetOnTouchListener(new TextViewHyperlinkOnTouchListener());

            view.FindViewById<TextView>(Resource.Id.timestamp)
				.SetText(TimeAgoConverter.Current.Convert(item.date), TextView.BufferType.Normal);

            view.FindViewById<ImageView>(Resource.Id.profilePic).SetImageResource(Resource.Drawable.user_default_avatar);
            //Finally return the view
            return view;
        }

        public List<int[]> getSpans(string body, char prefix)
        {
            List<int[]> spans = new List<int[]>();

            Regex pattern = new Regex(prefix + "\\w+");
            MatchCollection matches = pattern.Matches(body);

            foreach (Match match in matches)
            {
                var m = match.Groups[0];
                // Check all occurrences

                int[] currentSpan = new int[2];
                currentSpan[0] = m.Index;
                currentSpan[1] = currentSpan[0] + m.Length;
                spans.Add(currentSpan);
            }
            return  spans;
        }
    }
}

