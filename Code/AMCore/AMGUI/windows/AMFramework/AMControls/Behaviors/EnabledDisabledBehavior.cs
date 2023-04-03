using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;

namespace AMControls.Behaviors
{
    /// <summary>
    /// EnabledDisabledBehavior sets the colors used for enabled and disabled controls
    /// </summary>
    public class EnabledDisabledBehavior : Behavior<FrameworkElement>
    {
        #region Fields


        #endregion

        #region Property
        /// <summary>
        /// Enable color dependency property 
        /// </summary>
        public static readonly DependencyProperty EnabledColorProperty = DependencyProperty.Register("EnabledColor", typeof(SolidColorBrush), typeof(EnabledDisabledBehavior), new PropertyMetadata(null));

        /// <summary>
        /// Enable color dependency property 
        /// </summary>
        public static readonly DependencyProperty DisabledColorProperty = DependencyProperty.Register("DisabledColor", typeof(SolidColorBrush), typeof(EnabledDisabledBehavior), new PropertyMetadata(null));

        /// <summary>
        /// Enable color dependency property 
        /// </summary>
        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.Register("IsEnabled", typeof(bool), typeof(EnabledDisabledBehavior), new PropertyMetadata(true, OnIsEnabledPropertyChanged));


        /// <summary>
        /// Get/Set enable color
        /// </summary>
        public SolidColorBrush EnabledColor
        {
            get => (SolidColorBrush)GetValue(EnabledColorProperty);
            set => SetValue(EnabledColorProperty, value);
        }

        /// <summary>
        /// Get/Set enable color
        /// </summary>
        public SolidColorBrush DisabledColor
        {
            get => (SolidColorBrush)GetValue(DisabledColorProperty);
            set => SetValue(DisabledColorProperty, value);
        }

        /// <summary>
        /// Get/Set enable color
        /// </summary>
        public bool IsEnabled
        {
            get => (bool)GetValue(IsEnabledProperty);
            set 
            {
                SetValue(IsEnabledProperty, value);
                SetColor();
            }
        }

        
        #endregion

        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        public EnabledDisabledBehavior() 
        { 
            // empty
        }
        #endregion

        #region Behavior
        /// <summary>
        /// Attached behavior
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.IsEnabledChanged += OnIsEnabledChangedHandle;
        }

        /// <summary>
        /// Detached behavior
        /// </summary>
        protected override void OnDetaching() 
        {
            base.OnDetaching();

            AssociatedObject.IsEnabledChanged += OnIsEnabledChangedHandle;
        }


        #endregion

        #region Methods
        /// <summary>
        /// IsChanged handle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnIsEnabledChangedHandle(object sender, DependencyPropertyChangedEventArgs e)
        {
            IsEnabled = (bool)e.NewValue;
        }

        /// <summary>
        /// Sets the color to the current state
        /// </summary>
        private void SetColor() 
        {
            Brush brush = IsEnabled ? EnabledColor : DisabledColor;

            if (AssociatedObject is Border border)
            {
                border.Background = brush;
            }
            else if (AssociatedObject is FontAwesome6.Svg.SvgAwesome icon) 
            {
                icon.PrimaryColor = brush;
            }
            else if (AssociatedObject is TextBlock textBlock)
            {
                textBlock.Foreground = brush;
            }
        }

        /// <summary>
        /// On IsEnabled property changed handle
        /// </summary>
        private static void OnIsEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is EnabledDisabledBehavior control)
            {
                control.IsEnabled = (bool)e.NewValue;
            }
        }
        #endregion
    }
}
