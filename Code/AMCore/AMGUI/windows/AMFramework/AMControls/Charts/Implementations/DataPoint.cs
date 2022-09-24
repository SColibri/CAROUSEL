using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using AMControls.Charts.Interfaces;
using AMControls.Interfaces;
using AMControls.Interfaces.Implementations;

namespace AMControls.Charts.Implementations
{
    public class DataPoint : DrawObject_Abstract, IDataPoint
    {
        public DataPoint() 
        {
            SizeObject = new(10, 10);
        }

        private double _x = 0;
        private double _y = 0;
        private double _z = 0;
        private string _Label = "";

        public double X { get { return _x; } set { _x = value; } }
        public double Y { get { return _y; } set { _y = value; } }
        public double Z { get { return _z; } set { _z = value; } }


        private double _xDraw = 0;
        private double _yDraw = 0;

        public double X_draw 
        { 
            get { return _xDraw; } 
            set 
            { 
                _xDraw = value;
                Location = new(_xDraw - SizeObject.Width / 2,_yDraw - SizeObject.Height / 2);
                DataChanged?.Invoke(this, EventArgs.Empty);
            } 
        }
        public double Y_draw
        {
            get { return _yDraw; }
            set
            {
                _yDraw = value;
                Location = new(_xDraw - SizeObject.Width / 2, _yDraw - SizeObject.Height / 2);
                DataChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public double Z_draw { get; set; }

        public string Label { get { return _Label; } set { _Label = value; } }

        public bool MouseHover { get; set; } = false;
        public bool Selected { get; set; } = false;
        public object? Tag { get; set; }
        public IDataPoint_ContextMenu? ContextMenu { get; set; }

        public override void Draw(DrawingContext dc, Canvas canvas)
        {
            throw new NotImplementedException();
        }

        public override void Mouse_Hover_Action(double x, double y)
        {
            // Do nothing
        }

        public override void Mouse_LeftButton_Action(double x, double y)
        {
            throw new NotImplementedException();
        }

        public override void Mouse_RightButton_Action(double x, double y)
        {
            throw new NotImplementedException();
        }

        public event EventHandler DataChanged;
    }
}
