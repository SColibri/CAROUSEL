using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace AMFRamework_Components.Charts.SpyderChart
{
    internal class SpyderChart: System.Windows.Controls.Canvas
    {
        public int Intervals = 3;

        public FontFamily LabelFontFamily = new("Arial");
        public int LabelFontSize = 11;
        public FontFamily LabelGridFontFamily = new("Arial");
        public int LabelGridFontSize = 11;

        private List<Axis> _axes = new();
        private List<Series> _series = new();
        private List<SolidColorBrush> _PalleteData = new List<SolidColorBrush>()
        {
            Brushes.YellowGreen,
            Brushes.LightBlue,
            Brushes.LightGreen,
            Brushes.LightPink,
            Brushes.LightCyan,
            Brushes.LightSeaGreen,
            Brushes.LightGray
        };

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
        }

        public void Add_axe(Axis newAxis) 
        {
            _axes.Add(newAxis);
            _series.Clear();

            // refresh view
            this.InvalidateVisual();
        }

        public void Add_series(Series series) 
        {
            // data has to have same entries as number of axes
            if (series.Data.Count != _axes.Count) return; 

            // add new series and scale axes
            _series.Add(series);
            Get_minmax();

            // refresh view
            this.InvalidateVisual();
        }

        public void Clear_Series() { _series.Clear(); }
        public void Clear_Axes() { _axes.Clear(); }
        private void Get_minmax() 
        {
            for (int n1 = 0; n1 < _axes.Count; n1++)
            {
                _axes[n1].Min = 0;
                _axes[n1].Max = 0.0000000001;

                for (int n2 = 0; n2 < _series.Count; n2++)
                {
                    if(_axes[n1].Max < _series[n2].Data[n1]) { _axes[n1].Max = _series[n2].Data[n1]; }
                }

                _axes[n1].Interval = _axes[n1].Max/Intervals;
            }
        }

      

       


    }
}
