﻿using AMControls.Charts.Interfaces;
using AMControls.Interfaces.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AMControls.Charts.Implementations.DataSeries
{
    public abstract class DataSeries_Abstract : DrawObject_Abstract, IDataSeries
    {
        // IData series
        public List<IDataPoint> DataPoints { get; set; } = new();
        public int Index { get; set; } = 0;
        public string Label { get; set; } = "New series";
        public abstract Color ColorSeries { get; set; }

        public abstract event EventHandler DataPointSelectionChanged;
        public abstract event EventHandler SeriesSelected;

        // Draw Object abstract
        public override void Draw(DrawingContext dc, Canvas canvas)
        {
            throw new NotImplementedException();
        }

        public abstract void Draw(DrawingContext dc, Canvas canvas, Rect ChartArea, double xSize, double ySize, double xStart, double yStart);
        public override abstract void Mouse_Hover_Action(double x, double y);

        public override abstract void Mouse_LeftButton_Action(double x, double y);

        public override abstract void Mouse_RightButton_Action(double x, double y);

        
    }
}
