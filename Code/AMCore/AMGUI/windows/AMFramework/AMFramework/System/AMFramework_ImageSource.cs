using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.IO;

namespace AMFramework.AMsystem
{
    public class AMFramework_ImageSource
    {

        private static List<Tuple<string, BitmapImage>> _images = new();
        private static BitmapImage _noImage = new BitmapImage(new Uri($"/{ "Resources/Icons/tablerIcons/alert-triangle.png" }", UriKind.Relative));
        public static BitmapImage Get_faIcon(string ImageName) 
        {
            //Check if image is already loaded
            BitmapImage selectedImage = _noImage;
            Tuple<string, BitmapImage> imageInList = _images.Find(e => e.Item1.CompareTo(ImageName) == 0);
            
            if(imageInList == null) 
            {
                string filename = "Resources\\Icons\\tablerIcons\\" + ImageName + ".png";
                string testPath = AppDomain.CurrentDomain.BaseDirectory + filename;
                bool testThis = File.Exists(filename);
                if (File.Exists(testPath) == true) 
                {
                    BitmapImage Itembitmap = new BitmapImage(new Uri($"/{ filename }", UriKind.Relative));
                    Itembitmap.CacheOption = BitmapCacheOption.OnLoad;


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

    }
}
