using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace AMControls.Behaviors
{
    /// <summary>
    /// BorderAsContextMenuAnimationBehavior animates the Content border to expand as a context menu near the 
    /// Framework element to which it is bounded.
    /// 
    /// </summary>
    public class BorderAsContextMenuAnimationBehavior : Behavior<FrameworkElement>
    {
        #region properties
        public static readonly DependencyProperty ContentBorderProperty = DependencyProperty.Register("ContentBorder", typeof(Border), typeof(BorderAsContextMenuAnimationBehavior), new PropertyMetadata(null));
        /// <summary>
        /// Border to be expanded
        /// </summary>
        public Border ContentBorder 
        {
            get => (Border)GetValue(ContentBorderProperty);
            set => SetValue(ContentBorderProperty, value);
        }
        #endregion

        #region Behavior
        /// <summary>
        /// Detach behavior
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.MouseDown += OnMouseDown;
            AssociatedObject.LostFocus += OnFocusLost;
        }

        /// <summary>
        /// Attach behavior
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();

            AssociatedObject.MouseDown -= OnMouseDown;
            AssociatedObject.LostFocus -= OnFocusLost;
        }
        #endregion

        #region Methods
        /// <summary>
        /// On mouse move handle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseDown(object sender, MouseButtonEventArgs e) 
        {
            if (ContentBorder != null) 
            {
                ContentBorder.Child.Visibility = Visibility.Hidden;

                DoubleAnimation dbAnimation = new()
                {

                };

            }
        }

        /// <summary>
        /// On lost focus handle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFocusLost(object sender, RoutedEventArgs e) 
        {
            if (ContentBorder != null)
            {
                ContentBorder.Child.Visibility = Visibility.Hidden;

                DoubleAnimation dbAnimation = new()
                {

                };

            }
        }
        #endregion

    }
}
