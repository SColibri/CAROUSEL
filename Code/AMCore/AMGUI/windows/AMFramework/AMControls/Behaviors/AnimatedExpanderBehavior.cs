using AMControls.ExtensionMethods;
using Microsoft.Xaml.Behaviors;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;

namespace AMControls.Behaviors
{
    /// <summary>
    /// Animates an expander object when collapsing and expanding using a easing function.
    /// 
    /// For now this only handles vertical from bottom to top expansion behavior.
    /// </summary>
    public class AnimatedVerticalExpanderBehavior : Behavior<Expander>
    {

        #region fields
        /// <summary>
        /// Starting expanded height
        /// </summary>
        private double _rowHeight = 150;

        /// <summary>
        /// Column expanded width
        /// </summary>
        private double _columnWidth = 150;

        /// <summary>
        /// Row index in parent Grid
        /// </summary>
        private int _rowIndex;

        /// <summary>
        /// Column index in parent Grid
        /// </summary>
        private int _columnIndex;

        /// <summary>
        /// Grid Splitter object
        /// </summary>
        private GridSplitter? _splitterObject = null;

        /// <summary>
        /// Parent grid object
        /// </summary>
        private Grid? _parentGrid;

        #endregion

        #region Methods
        // ----------------------------------------------------------------------
        //                            BEHAVIOR HANDLES
        // ----------------------------------------------------------------------

        /// <summary>
        /// On attached 
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();

            // Cache parent grid and row index
            _parentGrid = AssociatedObject.TryFindParent<Grid>();
            _rowIndex = Grid.GetRow(AssociatedObject);
            _columnIndex = Grid.GetColumn(AssociatedObject);

            // check if splitter exists
            _splitterObject = AssociatedObject.TryFindParent<Grid>()?.TryFindChildOfType<GridSplitter>();
            AttachHandlesToSplitter();

            AssociatedObject.Expanded += OnExpanded;
            AssociatedObject.Collapsed += OnCollapsed;
        }

        /// <summary>
        /// On detaching
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();
            DettachHandlesToSplitter();

            AssociatedObject.Expanded -= OnExpanded;
            AssociatedObject.Collapsed -= OnCollapsed;
        }

        // ----------------------------------------------------------------------
        //                            EXPANDER HANDLES
        // ----------------------------------------------------------------------

        /// <summary>
        /// On expanded handle
        /// </summary>
        private void OnExpanded(object sender, EventArgs e)
        {
            if (sender is Expander dependencyObject)
            {
                EnableDisableSplitter(true);
                DoubleAnimation expandAnimation = new()
                {
                    Duration= TimeSpan.FromMilliseconds(300),
                    To = _rowHeight,
                    From = dependencyObject.ActualHeight,
                    EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut },
                };

                // Avoid UI lag by hiding the controls when expanding
                (dependencyObject.Content as UIElement).Visibility = Visibility.Hidden;
                expandAnimation.Completed += ExpandedAnimationCompleted;
                dependencyObject.BeginAnimation(FrameworkElement.HeightProperty, expandAnimation);
            }
        }

        /// <summary>
        /// After animation has been completed make controls visible, this way we avoid lagging
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExpandedAnimationCompleted(object sender, EventArgs e) 
        { 
            (sender as AnimationClock).Completed -= ExpandedAnimationCompleted;
            (AssociatedObject.Content as UIElement).Visibility = Visibility.Visible;
        }

        /// <summary>
        /// On collapsed handle
        /// </summary>
        private void OnCollapsed(object sender, EventArgs e)
        {
            if (sender is FrameworkElement dependencyObject)
            {
                SetRowAutoSized();
                EnableDisableSplitter(false);
                _rowHeight = dependencyObject.ActualHeight;

                DoubleAnimation expandAnimation = new()
                {
                    Duration= TimeSpan.FromMilliseconds(300),
                    To = 30,
                    From = _rowHeight,
                    EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut },
                };

                dependencyObject.BeginAnimation(FrameworkElement.HeightProperty, expandAnimation);
            }
        }

        // ----------------------------------------------------------------------
        //                         GRID SPLITTER HANDLES
        // ----------------------------------------------------------------------

        /// <summary>
        /// Enables grid splitter
        /// </summary>
        /// <param name="value"></param>
        private void EnableDisableSplitter(bool value)
        {
            if (_splitterObject != null)
            {
                _splitterObject.IsEnabled = value;
            }
        }

        /// <summary>
        /// Disables grid splitter
        /// </summary>
        private void AttachHandlesToSplitter()
        {
            if (_splitterObject != null)
            {
                _splitterObject.DragCompleted += GridSplitterDragSplitterCompletedHandle;
            }
        }

        /// <summary>
        /// Detach handles from grid splitter
        /// </summary>
        private void DettachHandlesToSplitter()
        {
            if (_splitterObject != null)
            {
                _splitterObject.DragCompleted -= GridSplitterDragSplitterCompletedHandle;
            }
        }

        /// <summary>
        /// Attach handles to gridsplitter
        /// </summary>
        private void SetRowAutoSized()
        {
            if (_parentGrid != null)
            {
                _parentGrid.RowDefinitions[_rowIndex].Height = new GridLength(0, GridUnitType.Auto);
            }
        }

        /// <summary>
        /// GridSplitter on drag complete handle
        /// </summary>
        private void GridSplitterDragSplitterCompletedHandle(object sender, DragCompletedEventArgs e)
        {
            if (_parentGrid != null)
            {
                DoubleAnimation expandAnimation = new()
                {
                    Duration= TimeSpan.FromMilliseconds(300),
                    To = _parentGrid.RowDefinitions[_rowIndex].Height.Value,
                    From = _rowHeight,
                    EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut },
                };

                _rowHeight = _parentGrid.RowDefinitions[_rowIndex].Height.Value;

                AssociatedObject.BeginAnimation(FrameworkElement.HeightProperty, expandAnimation);
            }
        }

        #endregion

    }
}
