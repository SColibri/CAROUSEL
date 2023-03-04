namespace AMControls.Interfaces
{
    /// <summary>
    /// Row that can be selected, implements the IsSelected property
    /// </summary>
    public interface ISelectable
    {
        /// <summary>
        /// defines if object is selected or not
        /// </summary>
        bool IsSelected { get; set; }

        /// <summary>
        /// Defines if multiselect is allowed
        /// </summary>
        bool AllowsMultiSelect { get; set; }
    }
}
