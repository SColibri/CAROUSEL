using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMControls.Interfaces
{
    public interface IObjectInteraction
    {
        /// <summary>
        /// Bounding box for the object dimension
        /// </summary>
        public System.Windows.Rect Bounds { get; set; }
        
        /// <summary>
        /// indicates if mouse is hovering on top of object 
        /// </summary>
        public bool IsMouseHover { get; set; }

        /// <summary>
        /// indicates if left button was clicked
        /// </summary>
        public bool IsLButton { get; set; }

        /// <summary>
        /// indicates if right was clicked
        /// </summary>
        public bool IsRButton { get; set; }

        /// <summary>
        /// Indicates if object is selected
        /// </summary>
        public bool IsSelected { get; set; }

        /// <summary>
        /// Checks if coordinates are inside the bounding box
        /// </summary>
        /// <returns></returns>
        public bool IsInside(double x, double y);

        /// <summary>
        /// Checks if mouse is hovering on top of object
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public bool Mouse_Hover(double x, double y);

        /// <summary>
        /// Checks if mouse click is on top of object
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public bool Mouse_LeftButton_Down(double x, double y);

        /// <summary>
        /// Checks if mouse click is on top of object
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public bool Mouse_RightButton_Down(double x, double y);


    }
}
