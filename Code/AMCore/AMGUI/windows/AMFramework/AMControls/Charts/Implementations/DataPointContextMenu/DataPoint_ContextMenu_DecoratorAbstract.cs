using AMControls.Charts.Interfaces;
using AMControls.Interfaces.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace AMControls.Charts.Implementations.DataPointContextMenu
{
    public abstract class DataPoint_ContextMenu_DecoratorAbstract : DrawObject_Abstract, IDataPoint_ContextMenu_Decorator
    {
        private bool _doAnimation = true;
        public IDataPoint_ContextMenu? DataPoint_ContextMenu { get; set; }
        public bool DoAnimation 
        { 
            get { return _doAnimation; } 
            set 
            { 
                _doAnimation = value;
            } 
        }

        public override abstract void Draw(DrawingContext dc, Canvas canvas);

        public override abstract void Mouse_Hover_Action(double x, double y);

        public override abstract void Mouse_LeftButton_Action(double x, double y);

        public override abstract void Mouse_RightButton_Action(double x, double y);

        protected override void OnBoundUpdate()
        {
            base.OnBoundUpdate();
            if (DataPoint_ContextMenu != null) 
            {
                DataPoint_ContextMenu.Location = Location;
                DataPoint_ContextMenu.SizeObject = SizeObject;
            }
        }
    }
}
