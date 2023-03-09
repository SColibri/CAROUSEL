using AMControls.Charts.Interfaces;
using AMControls.Interfaces.Implementations;
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

        public abstract override void Draw(DrawingContext dc, Canvas canvas);

        public abstract override void Mouse_Hover_Action(double x, double y);

        public abstract override void Mouse_LeftButton_Action(double x, double y);

        public abstract override void Mouse_RightButton_Action(double x, double y);

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
