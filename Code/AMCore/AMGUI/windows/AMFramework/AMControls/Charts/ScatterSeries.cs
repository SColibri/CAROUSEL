using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace AMControls.Charts
{
    public class ScatterSeries: IDataSeries
    {
        private static int IndexCount = 0;

        private string _label = "";
        private List<IDataPoint> _DataPoints = new();
        private int _checkArea = 25;
        private int _index = 0;
        private bool _isSelected = false;
        private bool _isVisible = false;
        private Color _lineColor = Colors.Black;
        private double _lineThickness = 1;

        private bool _showMarkers = false;

        public ScatterSeries() 
        {
            _index = IndexCount++;
        }

        public void Draw(DrawingContext dc, Canvas canvas, System.Windows.Rect ChartArea, double xSize, double ySize, double xStart, double yStart) 
        { 
            
            if(_DataPoints.Count > 0 && _isVisible == true) 
            {
                System.Windows.Point prevPoint = new(0,0);
                bool firstPoint = false;
                for (int n1 = 0; n1 < _DataPoints.Count; n1++)
                {
                    IDataPoint item = _DataPoints[n1];
                    double xLoc = ChartArea.X + item.X * xSize - xStart * xSize;
                    double yLoc = ChartArea.Y + ChartArea.Height -  item.Y * ySize + yStart * ySize;

                    if (ChartArea.Contains(xLoc, yLoc)) 
                    { 
                        if(!firstPoint) 
                        {
                            firstPoint = true;
                            prevPoint = new(xLoc, yLoc);
                            continue;
                        }

                        dc.DrawLine(new Pen(new SolidColorBrush(_lineColor), _lineThickness), prevPoint, new System.Windows.Point(xLoc, yLoc));
                        prevPoint = new(xLoc, yLoc);
                    }
                }
            }

        }

        public void CheckHit(double x, double y)
        {
            System.Windows.Rect Area = new(x,y, _checkArea, _checkArea);
            IDataPoint? Pointy = _DataPoints.Find(e => Area.Contains(e.X, e.Y));
            
            if (Pointy != null) { _isSelected = true; }
            else { _isSelected = false; }

        }

        #region Implementation
        public List<IDataPoint> DataPoints { get { return _DataPoints; } set { _DataPoints = value; } }
        public int Index { get { return _index; } set { _index = value; } }
        public bool IsSelected { get { return _isSelected; } set { _isSelected = value; } }
        public bool IsVisible { get { return _isVisible; } set { _isVisible = value; } }
        public string Label { get { return _label; } set { _label = value; } }
        public Color ColorSeries { get { return _lineColor; } set { _lineColor = value; } }
        #endregion
    }
}
