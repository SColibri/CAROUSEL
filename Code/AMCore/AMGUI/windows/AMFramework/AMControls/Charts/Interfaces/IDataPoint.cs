﻿using AMControls.Interfaces;
using System;

namespace AMControls.Charts.Interfaces
{
    public interface IDataPoint : IDrawable, IObjectInteraction
    {
        /// <summary>
        /// Shape of the data point
        /// </summary>
        public enum Shape
        {
            CIRCLE,
            BOX,
            CROSS,
            NONE
        }

        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public double X_draw { get; set; }
        public double Y_draw { get; set; }
        public double Z_draw { get; set; }

        // Additional information
        public string Label { get; set; }
        public object Tag { get; set; }

        // Interactions
        public bool MouseHover { get; set; }
        public bool Selected { get; set; }

        public IDataPoint_ContextMenu ContextMenu { get; set; }

        public event EventHandler DataChanged;
    }
}
