#if __ANDROID__
using System;
using Android.Graphics;

namespace rangr.common
{
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
#endif

