using System.Windows.Controls;
using System.Windows.Media;

namespace AMControls.Interfaces
{
    /// <summary>
    /// Object that can draw itself on to a canvas using DrawnigContext
    /// </summary>
    public interface IDrawable
    {
        /// <summary>
        /// Draw the object onto the canvas
        /// </summary>
        /// <param name="dc"></param>
        /// <param name="canvas"></param>
        public void Draw(DrawingContext dc, Canvas canvas);

        /// <summary>
        /// Location of the object
        /// </summary>
        public System.Windows.Point Location { get; set; }

        /// <summary>
        /// Size of the object
        /// </summary>
        public System.Windows.Size SizeObject { get; set; }

        /// <summary>
        /// Flag used to determine if object should or not be drawn
        /// </summary>
        public bool IsVisible { get; set; }

        /// <summary>
        /// Flag that specifies if object needs to be redrawn
        /// </summary>
        public bool NeedsUpdate { get; set; }

    }
}
