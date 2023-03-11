using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FontAwesome6.Svg;
using FontAwesome6;
using System.Windows;
using System.Windows.Media;

namespace AMControls.Behaviors
{
    /// <summary>
    /// SwapImageBehavior swaps between image one and two using a bool value. 
    /// </summary>
    public class SwapImageBehavior : Behavior<SvgAwesome>
    {

        #region Properties
        /// <summary>
        /// First Image dependency property to specify the icon image to be visualized as the first item 
        /// </summary>
        public static readonly DependencyProperty FirstImageProperty = DependencyProperty.Register("FirstImage", typeof(EFontAwesomeIcon), typeof(SwapImageBehavior), new PropertyMetadata(EFontAwesomeIcon.None, OnPropertyValueChanged));

        /// <summary>
        /// First Image dependency property to specify the icon image to be visualized as the first item 
        /// </summary>
        public static readonly DependencyProperty FirstColorProperty = DependencyProperty.Register("FirstColor", typeof(Brush), typeof(SwapImageBehavior), new PropertyMetadata(Brushes.Black, OnPropertyValueChanged));

        /// <summary>
        /// Second Image dependency property to specify the icon image to be visualized as the second item 
        /// </summary>
        public static readonly DependencyProperty SecondImageProperty = DependencyProperty.Register("SecondImage", typeof(EFontAwesomeIcon), typeof(SwapImageBehavior), new PropertyMetadata(EFontAwesomeIcon.None, OnPropertyValueChanged));

        /// <summary>
        /// First Image dependency property to specify the icon image to be visualized as the first item 
        /// </summary>
        public static readonly DependencyProperty SecondColorProperty = DependencyProperty.Register("SecondColor", typeof(Brush), typeof(SwapImageBehavior), new PropertyMetadata(Brushes.Black, OnPropertyValueChanged));

        /// <summary>
        /// Second Image dependency property to specify the icon image to be visualized as the second item 
        /// </summary>
        public static readonly DependencyProperty UseFirstProperty = DependencyProperty.Register("UseFirst", typeof(bool), typeof(SwapImageBehavior), new PropertyMetadata(true, OnPropertyValueChanged));


        /// <summary>
        /// Get/set First icon enumerator value
        /// </summary>
        public EFontAwesomeIcon FirstImage
        {
            get => (EFontAwesomeIcon)GetValue(FirstImageProperty);
            set => SetValue(FirstImageProperty, value);
        }

        /// <summary>
        /// Get/set Second icon enumerator value
        /// </summary>
        public EFontAwesomeIcon SecondImage 
        {
            get => (EFontAwesomeIcon)GetValue(SecondImageProperty);
            set => SetValue(SecondImageProperty, value);
        }

        /// <summary>
        /// Get/set First color brush value
        /// </summary>
        public Brush FirstColor
        {
            get => (Brush)GetValue(FirstColorProperty);
            set => SetValue(FirstColorProperty, value);
        }

        /// <summary>
        /// Get/set First color brush value
        /// </summary>
        public Brush SecondColor
        {
            get => (Brush)GetValue(SecondColorProperty);
            set => SetValue(SecondColorProperty, value);
        }

        /// <summary>
        /// Get/set Second icon enumerator value
        /// </summary>
        public bool UseFirst
        {
            get => (bool)GetValue(UseFirstProperty);
            set => SetValue(UseFirstProperty, value);
        }

        #endregion

        #region Behavior
        /// <summary>
        /// Behavior attached
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();
            UpdateIcon();
        }

        /// <summary>
        /// Behavior detaching
        /// </summary>
        protected override void OnDetaching() 
        { 
            base.OnDetaching();
            UpdateIcon();
        }
        #endregion

        #region Methods

        /// <summary>
        /// Dependency property handle for when the bounded value UseFirst has changed 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private static void OnPropertyValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SwapImageBehavior swapBehavior) 
            { 
                swapBehavior.UpdateIcon();
            }
        }

        /// <summary>
        /// Updates the image selection
        /// </summary>
        protected void UpdateIcon() 
        {
            if (AssociatedObject == null) return;

            if (UseFirst)
            {
                AssociatedObject.Icon = FirstImage;
                AssociatedObject.PrimaryColor = FirstColor;
            }
            else 
            { 
                AssociatedObject.Icon = SecondImage;
                AssociatedObject.PrimaryColor = SecondColor;
            }
        }

        #endregion

    }
}
