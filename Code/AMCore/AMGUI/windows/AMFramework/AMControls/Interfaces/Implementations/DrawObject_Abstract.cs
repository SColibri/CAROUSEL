using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace AMControls.Interfaces.Implementations
{
    public abstract class DrawObject_Abstract : IDrawable, IObjectInteraction
    {
        private Point _location = new();
        private Size _sizeObject = new();

        // IDrawable
        public Point Location
        {
            get { return _location; }
            set
            {
                _location = value;
                Update_bounds();
            }
        }
        public Size SizeObject
        {
            get { return _sizeObject; }
            set
            {
                _sizeObject = value;
                Update_bounds();
            }
        }
        public bool IsVisible { get; set; } = true;
        public bool NeedsUpdate { get; set; } = false;
        public abstract void Draw(DrawingContext dc, Canvas canvas);

        // IObjectInteraction
        public Rect Bounds { get; set; } = new();
        public bool IsMouseHover { get; set; } = false;
        public bool IsLButton { get; set; } = false;
        public bool IsRButton { get; set; } = false;
        public bool IsSelected { get; set; } = false;
        public bool IsInside(double x, double y)
        {
            Point LocationPoint = new(x, y);
            return Bounds.Contains(LocationPoint);
        }

        public bool Mouse_Hover(double x, double y)
        {
            IsMouseHover = IsInside(x, y);
            IsLButton = false;
            IsRButton = false;

            Mouse_Hover_Action(x, y);
            return IsMouseHover;
        }
        public bool Mouse_LeftButton_Down(double x, double y) 
        {
            IsMouseHover = false;
            IsLButton = IsInside(x, y);
            IsRButton = false;

            Mouse_LeftButton_Action(x, y);
            return IsLButton;
        }
        public bool Mouse_RightButton_Down(double x, double y) 
        {
            IsMouseHover = false;
            IsLButton = false;
            IsRButton = IsInside(x, y);

            Mouse_RightButton_Action(x, y);
            return IsRButton;
        }

        public abstract void Mouse_Hover_Action(double x, double y);
        public abstract void Mouse_RightButton_Action(double x, double y);
        public abstract void Mouse_LeftButton_Action(double x, double y);

        // Methods
        private void Update_bounds()
        {
            Bounds = new(_location, _sizeObject);
            OnBoundUpdate();
        }

        protected virtual void OnBoundUpdate(){}
    }
}
