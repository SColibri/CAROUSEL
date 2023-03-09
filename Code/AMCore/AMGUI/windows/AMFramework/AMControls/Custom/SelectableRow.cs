using AMControls.Interfaces;
using Catel.Data;
using Catel.MVVM;
using FontAwesome.WPF;
using System;

namespace AMControls.Custom
{
    /// <summary>
    /// Selectable row is a class 
    /// </summary>
    public class SelectableRow : ViewModelBase, ISelectable, ISearchable
    {
        #region ISelectable implementation
        public static readonly PropertyData IsSelectedProperty = RegisterProperty(nameof(IsSelected), typeof(bool), false);
        public bool IsSelected
        {
            get => GetValue<bool>(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }

        public static readonly PropertyData AllowsMultiSelectProperty = RegisterProperty(nameof(AllowsMultiSelect), typeof(bool), false);
        public bool AllowsMultiSelect
        {
            get => GetValue<bool>(AllowsMultiSelectProperty);
            set => SetValue(AllowsMultiSelectProperty, value);
        }
        #endregion

        #region ISearchable
        /// <summary>
        /// Search in object
        /// </summary>
        /// <param name="searchObject"></param>
        /// <returns></returns>
        public bool Search(object searchObject)
        {
            return string.IsNullOrEmpty(searchObject.ToString()) || Text.Contains(searchObject.ToString(), StringComparison.OrdinalIgnoreCase);
        }
        #endregion

        #region Properties

        public static readonly PropertyData IsVisibleProperty = RegisterProperty(nameof(IsVisible), typeof(bool), true);
        /// <summary>
        /// Get/set Visibility state
        /// </summary>
        public bool IsVisible
        {
            get => GetValue<bool>(IsVisibleProperty);
            set => SetValue(IsVisibleProperty, value);
        }

        public static readonly PropertyData IconProperty = RegisterProperty(nameof(Icon), typeof(Enum), FontAwesomeIcon.None);
        /// <summary>
        /// Set/get Icon image
        /// </summary>
        public Enum Icon
        {
            get => GetValue<Enum>(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public static readonly PropertyData TextProperty = RegisterProperty(nameof(Text), typeof(string), "");
        /// <summary>
        /// Get/set Text content
        /// </summary>
        public string Text
        {
            get => GetValue<string>(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly PropertyData TimeStampProperty = RegisterProperty(nameof(TimeStamp), typeof(DateTime), DateTime.Now);
        /// <summary>
        /// Get/Set timestamp
        /// </summary>
        public DateTime TimeStamp
        {
            get => GetValue<DateTime>(TimeStampProperty);
            set => SetValue(TimeStampProperty, value);
        }
        #endregion



    }
}
