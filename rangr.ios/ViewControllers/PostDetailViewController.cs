using System;
using UIKit;
using CoreLocation;
using CoreGraphics;
using System.Collections.Generic;
using Foundation;
using App.Common;

using Google.Maps;
using ios_ui_lib;

namespace rangr.ios
{
    public class PostDetailViewController : BaseViewModelController<PostDetailsViewModel>
    {
        public override string TitleLabel {
            get { return "Detail"; }
        }

        private const string PlaceholderImagePath = "user-default-avatar.png";

        private UIView card_view;
        private MapView map_view;
        private UILabel time_ago;
        private UIImageView user_image;
        private UILabel user_name;
        private UILabel post_text;

        private Post item;

        public PostDetailViewController(Post the_item)
        {
            view_model = new PostDetailsViewModel();
            view_model.CurrentPost = item = the_item;
        }

        //According to http://blog.adamkemp.com/2014/11/ios-layout-gotchas-and-view-controller.html
        //Need to moveout layout code from here and move to the ViewDidLayoutSubviews
        //Never set your own Frame in a UIView or your own View.Frame in a UIViewController.
        //Do layout in LayoutSubviews in a UIView and in ViewDidLayoutSubviews in a UIViewController.
        //Don't use the screen's size for layout. Instead use Bounds in a UIView and View.Bounds in a UIViewController.
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            create_view();

            BindDataToCell(item);
        }

        private void create_view()
        {
            View.BackgroundColor = UIColor.LightGray;

            View.AddSubview(card_view = new UIView(){
                TranslatesAutoresizingMaskIntoConstraints = false,
                BackgroundColor = UIColor.White
            });

            card_view.Layer.CornerRadius = 5;
            card_view.Layer.MasksToBounds = true;

            card_view.AddSubview(user_image = new UIImageView(){
                TranslatesAutoresizingMaskIntoConstraints = false
            });

            card_view.AddSubview(user_name = new UILabel(){
                TextColor = UIColor.Black,
                Font = UIFont.BoldSystemFontOfSize(17.0f),
                TextAlignment = UITextAlignment.Left,
                Lines = 1,
                LineBreakMode = UILineBreakMode.TailTruncation,
                TranslatesAutoresizingMaskIntoConstraints = false
            });

            card_view.AddSubview(time_ago = new UILabel(){
                TextColor = UIColor.Black,
                Font = UIFont.SystemFontOfSize(10.0f),
                TextAlignment = UITextAlignment.Left,
                Lines = 1,
                LineBreakMode = UILineBreakMode.TailTruncation,
                TranslatesAutoresizingMaskIntoConstraints = false
            });

            card_view.AddSubview(post_text = new UILabel(){
                TextColor = UIColor.Black,
                Font = UIFont.PreferredSubheadline,
                TextAlignment = UITextAlignment.Left,
                Lines = 0,
                LineBreakMode = UILineBreakMode.WordWrap,
                TranslatesAutoresizingMaskIntoConstraints = false
            });

            card_view.AddSubview(map_view = LoadMap());
            map_view.TranslatesAutoresizingMaskIntoConstraints = false;

        }

        public override void ViewDidLayoutSubviews()
        {
            this.user_name.SetContentCompressionResistancePriority(Layout.RequiredPriority, UILayoutConstraintAxis.Vertical);
            this.time_ago.SetContentCompressionResistancePriority(Layout.RequiredPriority, UILayoutConstraintAxis.Vertical);
            this.post_text.SetContentCompressionResistancePriority(Layout.RequiredPriority, UILayoutConstraintAxis.Vertical);



            var user_image_width = HumanInterface.medium_square_image_length;
            var user_image_height = HumanInterface.medium_square_image_length;

            View.ConstrainLayout(() => 
                card_view.Frame.Top == View.Bounds.Top + sibling_sibling_margin &&
                card_view.Frame.Left == View.Bounds.Left + sibling_sibling_margin &&
                card_view.Frame.Right == View.Bounds.Right - sibling_sibling_margin &&
                card_view.Frame.Bottom == View.Bounds.Bottom - sibling_sibling_margin
            );

            card_view.ConstrainLayout(() => 
                user_image.Frame.Width == user_image_width &&
                user_image.Frame.Height == user_image_height &&
                user_image.Frame.Top == card_view.Frame.Top + sibling_sibling_margin &&
                user_image.Frame.Left == card_view.Frame.Left + sibling_sibling_margin &&

                user_name.Frame.Left == user_image.Frame.Right + sibling_sibling_margin &&
                user_name.Frame.Right == card_view.Frame.Right - sibling_sibling_margin &&
                user_name.Frame.Top == card_view.Frame.Top + sibling_sibling_margin &&

                time_ago.Frame.Left == user_image.Frame.Right + sibling_sibling_margin &&
                time_ago.Frame.Right == card_view.Frame.Right - sibling_sibling_margin &&
                time_ago.Frame.Top == user_name.Frame.Bottom + sibling_sibling_margin &&
                time_ago.Frame.Bottom <= user_image.Frame.Bottom &&

                post_text.Frame.Left == card_view.Frame.Left + sibling_sibling_margin &&
                post_text.Frame.Right == card_view.Frame.Right - sibling_sibling_margin &&
                post_text.Frame.Top == user_image.Frame.Bottom + parent_child_margin &&

                map_view.Frame.Top == post_text.Frame.Bottom + parent_child_margin &&
                map_view.Frame.Left == card_view.Frame.Left &&
                map_view.Frame.Right == card_view.Frame.Right &&
                map_view.Frame.Bottom == card_view.Frame.Bottom

            );
                
        }

        private void BindDataToCell(Post post)
        {
            user_name.Text = post.user_display_name;
            post_text.Text = post.text;
            user_image.Image = UIImage.FromBundle(PlaceholderImagePath);
            time_ago.Text = TimeAgoConverter.Current.Convert(post.date);
        }

        private MapView LoadMap()
        {
            var splits = view_model.CurrentPost.long_lat_acc_geo_string.Split(',');

            var camera = CameraPosition.FromCamera(latitude: double.Parse(splits[1]), longitude: double.Parse(splits[0]), zoom: 15);

            var map_View = MapView.FromCamera(CGRect.Empty, camera);
        
            map_View.MapType = MapViewType.Normal;
        
            map_View.MyLocationEnabled = false;
        
            map_View.Settings.SetAllGesturesEnabled(false);
        
            AddMarker(view_model.CurrentPost, map_View);
        
            AddCircle(view_model.CurrentPost, map_View);
        
            map_View.Animate(GetPosition(view_model.CurrentPost.long_lat_acc_geo_string));

            return map_View;
        }

        private void AddMarker(Post the_post, MapView the_map)
        {
            var marker = new Marker();
            marker.Title = the_post.text;
            marker.Position = GetPosition(the_post.long_lat_acc_geo_string);
            marker.Map = the_map;
        
            //.InvokeIcon(BitmapDescriptorFactory
            //.DefaultMarker(BitmapDescriptorFactory
            //.HueCyan)))
        }

        private void AddCircle(Post the_post, MapView the_map)
        {
            var circle = new Circle();
            circle.Position = GetPosition(the_post.long_lat_acc_geo_string);
            circle.Radius = GetAccurracy(the_post.long_lat_acc_geo_string);
            circle.StrokeWidth = 1.0f;
            circle.StrokeColor = UIColor.LightTextColor;
            circle.FillColor = UIColor.LightTextColor;
        
            circle.Map = the_map;
        }

        private CLLocationCoordinate2D GetPosition(string geo_string)
        {
            var array = geo_string.Split(',');
            return new CLLocationCoordinate2D(double.Parse(array[1]), double.Parse(array[0]));
        }

        private int GetAccurracy(string geo_string)
        {
            var array = geo_string.Split(',');
            return int.Parse(array[2]);
        }
    }
}

