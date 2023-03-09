using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media.Imaging;

namespace AMFramework.AMsystem
{
    public class AMFramework_ImageSource
    {

        private static List<Tuple<string, BitmapImage>> _images = new();
        private static BitmapImage _noImage = new(new Uri($"/{"Resources/Icons/tablerIcons/alert-triangle.png"}", UriKind.Relative));

        private static string Get_FullPath(string filename)
        {
            return AppDomain.CurrentDomain.BaseDirectory  + "Resources\\Icons\\tablerIcons\\" + filename + ".png";
        }

        public static BitmapImage Get_faIcon(string ImageName)
        {
            //Check if image is already loaded
            BitmapImage selectedImage = _noImage;
            Tuple<string, BitmapImage> imageInList = _images.Find(e => e.Item1.CompareTo(ImageName) == 0);

            if (imageInList == null)
            {
                string filename = "Resources\\Icons\\tablerIcons\\" + ImageName + ".png";
                string testPath = AppDomain.CurrentDomain.BaseDirectory + filename;
                bool testThis = File.Exists(filename);
                if (File.Exists(filename) == true)
                {
                    BitmapImage Itembitmap = new(new Uri($"/{filename}", UriKind.Relative))
                    {
                        CacheOption = BitmapCacheOption.OnLoad
                    };


                    imageInList = new Tuple<string, BitmapImage>(ImageName, Itembitmap);
                    _images.Add(imageInList);
                    return imageInList.Item2;
                }
            }
            else
            {
                return imageInList.Item2;
            }

            return selectedImage;
        }

        public static System.Drawing.Bitmap Get_faIcon_bitmap(string ImageName)
        {
            string filename = Get_FullPath(ImageName);

            if (!File.Exists(filename))
            {
                System.Drawing.Bitmap errBitmap = new(17, 17);
                return new(17, 17);
            }

            System.Drawing.Bitmap originalIMG = new(filename);
            return new System.Drawing.Bitmap(originalIMG, new System.Drawing.Size(17, 17));
        }
    }
}
