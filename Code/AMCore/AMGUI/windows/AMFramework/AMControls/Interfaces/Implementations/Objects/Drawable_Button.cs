using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AMControls.Interfaces.Implementations.Objects
{
    public class Drawable_Button : DrawObject_Abstract
    {
        public BitmapImage ImageIcon { get; set; }

        public Drawable_Button() 
        {
            ImageIcon = new BitmapImage();
        }

        public override void Draw(DrawingContext dc, Canvas canvas)
        {
            SolidColorBrush SBack = new SolidColorBrush(Colors.Cyan) { Opacity = 0.2};
            dc.DrawRoundedRectangle(SBack,
                                    new Pen(new SolidColorBrush(Colors.Silver), 0.4),
                                    Bounds, 3, 3);

            if (IsMouseHover || IsLButton) 
            {
                SolidColorBrush SBackHover = new SolidColorBrush(Colors.DarkRed) { Opacity = 0.2 };
                dc.DrawRoundedRectangle(SBackHover,
                                        new Pen(new SolidColorBrush(Colors.Silver), 1),
                                        Bounds, 3, 3);
            }
            else if (IsLButton) 
            { 
            
            }
            else 
            { 
            
            }

            dc.DrawImage(ImageIcon, Bounds);
        }

        public override void Mouse_Hover_Action(double x, double y)
        {
            //throw new NotImplementedException();
        }

        public override void Mouse_LeftButton_Action(double x, double y)
        {
            //throw new NotImplementedException();
        }

        public override void Mouse_RightButton_Action(double x, double y)
        {
            throw new NotImplementedException();
        }
    }
}
