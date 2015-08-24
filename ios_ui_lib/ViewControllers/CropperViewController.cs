using System;
using UIKit;
using System.Drawing;
using solid_lib;
using CoreGraphics;

namespace ios_ui_lib
{
    public class CropperViewController : UIViewController
    {
        private string selected_image_path { get; set;}

        private UIImageView imageView;
        private CropperView cropperView;
        private UIPanGestureRecognizer pan;
        private UIPinchGestureRecognizer pinch;
        private UITapGestureRecognizer doubleTap;

        public CropperViewController (string the_selected_image_path)
        {
            selected_image_path = Guard.IsNotNull(the_selected_image_path, "the_selected_image_path");
        }

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();

            create_view();

            bind_cropper_gestures();
               
        }

        public override void ViewDidLayoutSubviews()
        {
            //aspect ratio stuff
            var divisor = imageView.Image.Size.Width / View.Bounds.Width;
            var image_height = imageView.Image.Size.Height / divisor;



            View.ConstrainLayout(() => 

                imageView.Frame.Width == View.Frame.Width &&
                imageView.Frame.Height == image_height &&
                imageView.Frame.Top == View.Frame.Top &&
                imageView.Frame.Left == View.Frame.Left &&

                cropperView.Frame.Left == View.Frame.Left &&
                cropperView.Frame.Right == View.Frame.Right &&
                cropperView.Frame.Top == View.Frame.Top &&
                cropperView.Frame.Bottom == View.Frame.Bottom
            );
        }

        private void create_view()
        {
            using (var image = UIImage.FromFile (selected_image_path)) 
            {
                View.AddSubview(imageView = new UIImageView(){
                    Image = image,
                    TranslatesAutoresizingMaskIntoConstraints = false
                });
            }

            View.AddSubview(cropperView = new CropperView { 
                //Frame = View.Bounds, 
                TranslatesAutoresizingMaskIntoConstraints = false 
            });
        }

        private void bind_cropper_gestures()
        {
            nfloat dx = 0;
            nfloat dy = 0;

            pan = new UIPanGestureRecognizer(g=>{
                if ((g.State == UIGestureRecognizerState.Began || g.State == UIGestureRecognizerState.Changed) && (g.NumberOfTouches == 1)) {

                    var p0 = g.LocationInView (View);

                    if (dx == 0)
                        dx = p0.X - cropperView.Origin.X;

                    if (dy == 0)
                        dy = p0.Y - cropperView.Origin.Y;

                    var p1 = new CGPoint (p0.X - dx, p0.Y - dy);

                    cropperView.Origin = p1;
                } else if (g.State == UIGestureRecognizerState.Ended) {
                    dx = 0;
                    dy = 0;
                }
            });

            nfloat s0 = 1;

            pinch = new UIPinchGestureRecognizer (g=>{
                nfloat s = g.Scale;
                nfloat ds = (nfloat)Math.Abs (s - s0);
                nfloat sf = 0;
                const float rate = 0.5f;

                if (s >= s0) {
                    sf = 1 + ds * rate;
                } else if (s < s0) {
                    sf = 1 - ds * rate;
                }
                s0 = s;

                cropperView.CropSize = new CGSize (cropperView.CropSize.Width * sf, cropperView.CropSize.Height * sf);  

                if (g.State == UIGestureRecognizerState.Ended) {
                    s0 = 1;
                } 
            });

            doubleTap = new UITapGestureRecognizer ((gesture) => {
                Crop();

            }) { 
                NumberOfTapsRequired = 2, NumberOfTouchesRequired = 1 
            };

            cropperView.AddGestureRecognizer (pan);
            cropperView.AddGestureRecognizer (pinch);
            cropperView.AddGestureRecognizer (doubleTap);
        }


        private void Crop()
        {
            var img = UIImage.FromFile(selected_image_path);

            var inputCGImage = img.CGImage;

            var image = inputCGImage.WithImageInRect (cropperView.CropRect);
            using (var croppedImage = UIImage.FromImage (image)) {

                imageView.Image = croppedImage;
                imageView.Frame = cropperView.CropRect;
                imageView.Center = View.Center;

                cropperView.Origin = new CGPoint (imageView.Frame.Left, imageView.Frame.Top);
                cropperView.Hidden = true;
            }
        }
    }
}

