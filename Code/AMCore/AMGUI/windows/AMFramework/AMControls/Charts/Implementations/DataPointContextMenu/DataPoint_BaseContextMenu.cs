using AMControls.Charts.Implementations.DataPointContextMenu;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace AMControls.Charts.DataPointContextMenu
{
    public class DataPoint_BaseContextMenu : DataPoint_ContextMenu_Abstract
    {
        private double _cornerRadius = 5.0;
        private double _animationRadius = 1000;
        private double _animationBox = 500;
        public DataPoint_BaseContextMenu()
        {
            Location = new Point();
            SizeObject = new();
            Bounds = new();
        }

        #region Interface_implementation

        #region DataPoint_ContextMenu_Abstract
        public override void Draw(DrawingContext dc, Canvas canvas)
        {
            System.Windows.Rect menuBox = new(Location, SizeObject);
            SolidColorBrush sbc_background = new SolidColorBrush(Colors.White) { Opacity = 1.0 };

            System.Windows.Rect menuBox_Back = new(new Point(Location.X - 5, Location.Y + 5), SizeObject);
            SolidColorBrush sbc_background_Back = new SolidColorBrush(Colors.Black) { Opacity = 0.7 };

            if (DoAnimation)
            {
                RectAnimation boxAnim = new()
                {
                    Duration = TimeSpan.FromMilliseconds(_animationBox),
                    FillBehavior = FillBehavior.HoldEnd,
                    From = new Rect(Location, new Size(0, 0)),
                    To = menuBox
                };
                DoubleAnimation dAnim = new()
                {
                    Duration = TimeSpan.FromMilliseconds(_animationRadius),
                    FillBehavior = FillBehavior.HoldEnd,
                    From = 0,
                    To = _cornerRadius
                };
                RectAnimation boxAnim_Back = new()
                {
                    Duration = TimeSpan.FromMilliseconds(_animationBox),
                    FillBehavior = FillBehavior.HoldEnd,
                    From = new Rect(new Point(Location.X - 5, Location.Y - 5), new Size(0, 0)),
                    To = menuBox_Back
                };

                dc.DrawRoundedRectangle(sbc_background_Back, new Pen(new SolidColorBrush(Colors.Silver), 1),
                                        menuBox_Back, boxAnim_Back.CreateClock(), 0, dAnim.CreateClock(), 0, dAnim.CreateClock());

                dc.DrawRoundedRectangle(sbc_background, new Pen(new SolidColorBrush(Colors.Silver), 1),
                                        menuBox, boxAnim.CreateClock(), 0, dAnim.CreateClock(), 0, dAnim.CreateClock());
                DoAnimation = false;
            }
            else
            {
                dc.DrawRoundedRectangle(sbc_background_Back, new Pen(new SolidColorBrush(Colors.Silver), 1),
                                        menuBox_Back, _cornerRadius, _cornerRadius);

                dc.DrawRoundedRectangle(sbc_background, new Pen(new SolidColorBrush(Colors.Silver), 1),
                                        menuBox, _cornerRadius, _cornerRadius);
            }


        }

        #endregion

        #region IObjectInteraction


        public override void Mouse_Hover_Action(double x, double y)
        {
            throw new NotImplementedException();
        }

        public override void Mouse_LeftButton_Action(double x, double y)
        {
            throw new NotImplementedException();
        }


        public override void Mouse_RightButton_Action(double x, double y)
        {
            throw new NotImplementedException();
        }


        #endregion

        #endregion
    }
}
