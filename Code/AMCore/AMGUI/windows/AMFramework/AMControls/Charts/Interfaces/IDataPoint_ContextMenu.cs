using AMControls.Interfaces;

namespace AMControls.Charts.Interfaces
{
    public interface IDataPoint_ContextMenu : IDrawable, IObjectInteraction
    {
        public bool DoAnimation { get; set; }
    }
}
