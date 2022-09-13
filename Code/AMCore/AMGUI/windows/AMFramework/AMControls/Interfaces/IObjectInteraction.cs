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
        public System.Drawing.Rectangle Bounds { get; set; }
        /// <summary>
        /// Checks if coordinates are inside the bounding box
        /// </summary>
        /// <returns></returns>
        public bool IsInside(int x, int y);

    }
}
