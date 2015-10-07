
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
using Android.Provider;
using Android.Graphics;
using Xamarin.Media;
using System.IO;

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

            Fragment.RequestImagePicker += async () => {

                var picker = new MediaPicker(this);

                    var intent = picker.GetPickPhotoUI();

                    var result = await StartActivityForResultAsync(intent);

                    if (result.ResultCode == Result.Canceled)
                        return;

                    var media_file = await result.Data.GetMediaFileExtraAsync(this);

                    Fragment.SetImage(ImageHelper.Current.get_square_bitmap(media_file.Path));                

            };

            Fragment.NewPostCreated += () => {
                Finish();
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

            post_text_input = view.FindViewById<EditText>(Resource.Id.PostText)
                                .Chain(p => p.TextChanged += (object sender, TextChangedEventArgs e) => {
                var text = ((EditText)sender).Text;

                PostTextChanged(text);
                view_model.PostText = text;
            });
            post_image = view.FindViewById<ImageView>(Resource.Id.PostImage);

            return view;
        }

        public override void OnResume()
        {
            base.OnResume();

            if (!is_image_set)
            {
                is_image_set = true;
                RequestImagePicker();
            }
        }

        private EditText post_text_input;
        private ImageView post_image;
        private bool is_image_set = false;

        public void SetImage(Bitmap image)
        {
            //Todo: Validate image dimensions
            post_image.SetImageBitmap(image);
            view_model.PostImage = prepare_image(image);
        }

        private HttpFile prepare_image(Bitmap image)
        {
            byte[] bytes;
            using (var stream = new MemoryStream())
            {
                image.Compress(Bitmap.CompressFormat.Jpeg, 0, stream);
                bytes = stream.ToArray();
            }
            return new HttpFile("photo","image/jpg", bytes); 
        }

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

        public event Action RequestImagePicker = delegate {};

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

    public class ImageHelper : SingletonBase<ImageHelper>
    {
        public Bitmap get_square_bitmap(string image_path)
        {
            var image_file = new Java.IO.File(image_path);
            return crop_square(BitmapFactory.DecodeFile(image_file.AbsolutePath));
        }

        private Bitmap crop_square(Bitmap bitmap)
        {
            var width = bitmap.Width;
            var height = bitmap.Height;
            var newWidth = (height > width) ? width : height;
            var newHeight = (height > width) ? height - (height - width) : height;
            var cropW = (width - height) / 2;
            cropW = (cropW < 0) ? 0 : cropW;
            var cropH = (height - width) / 2;
            cropH = (cropH < 0) ? 0 : cropH;
            var cropImg = Bitmap.CreateBitmap(bitmap, cropW, cropH, newWidth, newHeight);

            return cropImg;
        }

        private ImageHelper()
        {
        }
    }
}