﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using AMControls.Charts.DataPointContextMenu;
using AMControls.Charts.Interfaces;
using AMFramework.Interfaces;
using AMFramework.Model;

namespace AMFramework.Components.Charting.ContextMenu
{
    public class DataPoint_ProjectViewContextMenu : IDataPoint_ContextMenu_Decorator
    {
        private Point _location;
        private Size _sizeObject;
        private List<string> _content;

        // SubTitle font
        private FontFamily _subTitleFontFamily = new("Lucida Sans");
        private int _subTitleFontSize = 9;
        private System.Windows.FontStyle _subTitleFontStyle = System.Windows.FontStyles.Normal; // https://docs.microsoft.com/en-us/dotnet/api/system.drawing.fontstyle?view=dotnet-plat-ext-6.0
        private System.Windows.FontWeight _subTitleFontWeight = System.Windows.FontWeights.Bold; // https://docs.microsoft.com/en-us/dotnet/api/system.windows.fontweights?view=windowsdesktop-6.0
        private System.Windows.FontStretch _subTitleFontStretch = System.Windows.FontStretches.Normal;
        private Color _subTitleFontColor = Colors.Black;

        // content font
        private FontFamily _contentFontFamily = new("Lucida Sans");
        private int _contentFontSize = 9;
        private System.Windows.FontStyle _contentFontStyle = System.Windows.FontStyles.Normal; // https://docs.microsoft.com/en-us/dotnet/api/system.drawing.fontstyle?view=dotnet-plat-ext-6.0
        private System.Windows.FontWeight _contentFontWeight = System.Windows.FontWeights.Bold; // https://docs.microsoft.com/en-us/dotnet/api/system.windows.fontweights?view=windowsdesktop-6.0
        private System.Windows.FontStretch _contentFontStretch = System.Windows.FontStretches.Normal;
        private Color _contentFontColor = Colors.Black;

        public DataPoint_ProjectViewContextMenu(List<string> contentCells, string Title) 
        {
            _content = contentCells;
            DataPoint_ContextMenu = new DataPoint_ContextMenu_Text() { Title = Title };
            SizeObject = new Size(180,200);
        }
        public IDataPoint_ContextMenu DataPoint_ContextMenu { get; set; }
        public Point Location 
        {
            get { return _location; }
            set 
            { 
                _location = value; 
                DataPoint_ContextMenu.Location = value;
            } 
        }
        public Size SizeObject 
        { 
            get { return _sizeObject; }
            set 
            { 
                _sizeObject = value;
                DataPoint_ContextMenu.SizeObject = value;
            } 
        }
        public bool DoAnimation { get; set; } = true;

        public void CheckHit(double x, double y)
        {
            throw new NotImplementedException();
        }

        public void Draw(DrawingContext dc, Canvas canvas)
        {
            DataPoint_ContextMenu.Draw(dc, canvas);
            if (_content.Count < 12) return;

            Point sPoint = new Point(Location.X + 10, Location.Y + 42);
            double boxWidth = SizeObject.Width - 20;

            SolidColorBrush subTitleBrush = new SolidColorBrush(Colors.Black);
            SolidColorBrush RedID = new SolidColorBrush(Colors.Red);
            SolidColorBrush contentBrush = new SolidColorBrush(Colors.Black);

            if (DoAnimation) 
            {
                DoubleAnimation tileTextAnimation = new(1, new Duration(TimeSpan.FromMilliseconds(800))){ From = 0, To = 1 };

                subTitleBrush.ApplyAnimationClock(SolidColorBrush.OpacityProperty, tileTextAnimation.CreateClock());
                RedID.ApplyAnimationClock(SolidColorBrush.OpacityProperty, tileTextAnimation.CreateClock());
                contentBrush.ApplyAnimationClock(SolidColorBrush.OpacityProperty, tileTextAnimation.CreateClock());
            }

            FormattedText IDProject_sub = new("Project id: ", System.Globalization.CultureInfo.CurrentCulture,
                                                  System.Windows.FlowDirection.LeftToRight,
                                                  new Typeface(_subTitleFontFamily, _subTitleFontStyle, _subTitleFontWeight, _subTitleFontStretch),
                                                  _subTitleFontSize, subTitleBrush, VisualTreeHelper.GetDpi(canvas).PixelsPerDip);

            
            FormattedText IDProject = new(_content[7], System.Globalization.CultureInfo.CurrentCulture,
                                                  System.Windows.FlowDirection.LeftToRight,
                                                  new Typeface(_contentFontFamily, _contentFontStyle, _contentFontWeight, _contentFontStretch),
                                                  _contentFontSize, RedID, VisualTreeHelper.GetDpi(canvas).PixelsPerDip);

            FormattedText IDCase_sub = new("Case id: ", System.Globalization.CultureInfo.CurrentCulture,
                                                  System.Windows.FlowDirection.LeftToRight,
                                                  new Typeface(_subTitleFontFamily, _subTitleFontStyle, _subTitleFontWeight, _subTitleFontStretch),
                                                  _subTitleFontSize, subTitleBrush, VisualTreeHelper.GetDpi(canvas).PixelsPerDip);


            FormattedText IDCase = new(_content[8], System.Globalization.CultureInfo.CurrentCulture,
                                                  System.Windows.FlowDirection.LeftToRight,
                                                  new Typeface(_contentFontFamily, _contentFontStyle, _contentFontWeight, _contentFontStretch),
                                                  _contentFontSize, contentBrush, VisualTreeHelper.GetDpi(canvas).PixelsPerDip);

            FormattedText IDHT_sub = new("Heat treatment id: ", System.Globalization.CultureInfo.CurrentCulture,
                                                  System.Windows.FlowDirection.LeftToRight,
                                                  new Typeface(_subTitleFontFamily, _subTitleFontStyle, _subTitleFontWeight, _subTitleFontStretch),
                                                  _subTitleFontSize, subTitleBrush, VisualTreeHelper.GetDpi(canvas).PixelsPerDip);


            FormattedText IDHT = new(_content[2], System.Globalization.CultureInfo.CurrentCulture,
                                                  System.Windows.FlowDirection.LeftToRight,
                                                  new Typeface(_contentFontFamily, _contentFontStyle, _contentFontWeight, _contentFontStretch),
                                                  _contentFontSize, contentBrush, VisualTreeHelper.GetDpi(canvas).PixelsPerDip);

            FormattedText NameHT_sub = new("name: ", System.Globalization.CultureInfo.CurrentCulture,
                                                  System.Windows.FlowDirection.LeftToRight,
                                                  new Typeface(_subTitleFontFamily, _subTitleFontStyle, _subTitleFontWeight, _subTitleFontStretch),
                                                  _subTitleFontSize, subTitleBrush, VisualTreeHelper.GetDpi(canvas).PixelsPerDip);


            FormattedText NameHT = new(_content[9], System.Globalization.CultureInfo.CurrentCulture,
                                                  System.Windows.FlowDirection.LeftToRight,
                                                  new Typeface(_contentFontFamily, _contentFontStyle, _contentFontWeight, _contentFontStretch),
                                                  _contentFontSize, contentBrush, VisualTreeHelper.GetDpi(canvas).PixelsPerDip);

            FormattedText PrecipitationDomain_sub = new("Precipitation Domain: ", System.Globalization.CultureInfo.CurrentCulture,
                                                  System.Windows.FlowDirection.LeftToRight,
                                                  new Typeface(_subTitleFontFamily, _subTitleFontStyle, _subTitleFontWeight, _subTitleFontStretch),
                                                  _subTitleFontSize, subTitleBrush, VisualTreeHelper.GetDpi(canvas).PixelsPerDip);


            FormattedText PrecipitationDomain = new(_content[10], System.Globalization.CultureInfo.CurrentCulture,
                                                  System.Windows.FlowDirection.LeftToRight,
                                                  new Typeface(_contentFontFamily, _contentFontStyle, _contentFontWeight, _contentFontStretch),
                                                  _contentFontSize, contentBrush, VisualTreeHelper.GetDpi(canvas).PixelsPerDip);

            FormattedText PrecipitationPhase_sub = new("Precipitation phase: ", System.Globalization.CultureInfo.CurrentCulture,
                                                  System.Windows.FlowDirection.LeftToRight,
                                                  new Typeface(_subTitleFontFamily, _subTitleFontStyle, _subTitleFontWeight, _subTitleFontStretch),
                                                  _subTitleFontSize, subTitleBrush, VisualTreeHelper.GetDpi(canvas).PixelsPerDip);


            FormattedText PrecipitationPhase = new(_content[11], System.Globalization.CultureInfo.CurrentCulture,
                                                  System.Windows.FlowDirection.LeftToRight,
                                                  new Typeface(_contentFontFamily, _contentFontStyle, _contentFontWeight, _contentFontStretch),
                                                  _contentFontSize, contentBrush, VisualTreeHelper.GetDpi(canvas).PixelsPerDip);

            // Project
            Point lsPoint = new Point(sPoint.X, sPoint.Y + IDProject_sub.Height + 5);
            Point lePoint = new Point(sPoint.X + boxWidth, sPoint.Y + IDProject_sub.Height + 5);
            Do_line(dc, lsPoint, lePoint);

            dc.DrawText(IDProject_sub, sPoint);
            dc.DrawText(IDProject, new Point(sPoint.X + IDProject_sub.Width + 5, sPoint.Y));
            sPoint.Y = lePoint.Y + 5;

            // Case
            dc.DrawText(IDCase_sub, sPoint);
            dc.DrawText(IDCase, new Point(sPoint.X + IDCase_sub.Width + 5, sPoint.Y));
            sPoint.Y += IDCase.Height + 5;

            lsPoint = new Point(sPoint.X, sPoint.Y);
            lePoint = new Point(sPoint.X + boxWidth, sPoint.Y);
            Do_line(dc, lsPoint, lePoint);
            sPoint.Y += IDHT.Height + 5;

            // Heat Treatment
            dc.DrawText(IDHT_sub, sPoint);
            dc.DrawText(IDHT, new Point(sPoint.X + IDHT_sub.Width + 5, sPoint.Y));
            sPoint.Y += IDHT.Height + 5;

            dc.DrawText(NameHT_sub, sPoint);
            dc.DrawText(NameHT, new Point(sPoint.X + NameHT_sub.Width + 5, sPoint.Y));
            sPoint.Y += NameHT_sub.Height + 5;

            lsPoint = new Point(sPoint.X, sPoint.Y);
            lePoint = new Point(sPoint.X + boxWidth, sPoint.Y);
            Do_line(dc, lsPoint, lePoint);
            sPoint.Y += IDHT.Height + 5;

            // Precipitation
            dc.DrawText(PrecipitationDomain_sub, sPoint);
            dc.DrawText(PrecipitationDomain, new Point(sPoint.X + PrecipitationDomain_sub.Width + 5, sPoint.Y));
            sPoint.Y += PrecipitationDomain_sub.Height + 5;

            dc.DrawText(PrecipitationPhase_sub, sPoint);
            sPoint.Y += PrecipitationPhase_sub.Height + 5;
            dc.DrawText(PrecipitationPhase, new Point(sPoint.X + boxWidth + 5 - PrecipitationPhase.Width, sPoint.Y));
            sPoint.Y += PrecipitationPhase_sub.Height + 5;

            lsPoint = new Point(sPoint.X, sPoint.Y);
            lePoint = new Point(sPoint.X + boxWidth, sPoint.Y);
            Do_line(dc, lsPoint, lePoint);
            sPoint.Y += IDHT.Height + 5;

            DoAnimation = false;
        }

        private void Do_line(DrawingContext dc, Point lsPoint, Point lePoint) 
        {
            if (DoAnimation)
            {
                PointAnimationBase sAnim = new PointAnimation(lsPoint, new Duration(TimeSpan.FromMilliseconds(800)));
                PointAnimationBase eAnim = new PointAnimation(lePoint, new Duration(TimeSpan.FromMilliseconds(800)));

                dc.DrawLine(new Pen(new SolidColorBrush(Colors.DimGray), 0.5),
                            lsPoint, sAnim.CreateClock(),
                            lsPoint, eAnim.CreateClock());
            }
            else
            {
                dc.DrawLine(new Pen(new SolidColorBrush(Colors.DimGray), 0.5), lsPoint, lePoint);
            }
        }
    }
}