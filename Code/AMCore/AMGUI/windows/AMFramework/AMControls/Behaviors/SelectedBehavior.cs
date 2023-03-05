using AMControls.ExtensionMethods;
using AMControls.Interfaces;
using Catel.Collections;
using Microsoft.Xaml.Behaviors;
using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AMControls.Behaviors
{
    /// <summary>
    /// SelectedBehavior does the selected item behavior for border items by changing the background to the
    /// selected state.
    /// </summary>
    public class SelectedBehavior : Behavior<Border>
    {
        #region Fields
        /// <summary>
        /// Container on which the border is contained in
        /// </summary>
        private Panel? _containerObject;

        #endregion
        #region Behavior
        /// <summary>
        /// Attached handle
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();

            _containerObject = AssociatedObject.TryFindParent<Panel>();
            AssociatedObject.MouseDown += OnMouseDown;
            AssociatedObject.MouseLeave += OnMouseLeave;
            AssociatedObject.MouseEnter += OnMouseEnter;

        }

        /// <summary>
        /// Detached handle
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();

            AssociatedObject.MouseDown -= OnMouseDown;
            AssociatedObject.MouseLeave -= OnMouseLeave;
            AssociatedObject.MouseEnter -= OnMouseEnter;

        }
        #endregion

        #region Handles 
        /// <summary>
        /// On mouse enter Handle, sets the background to hover color
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseEnter(object sender, MouseEventArgs e)
        {
            SetBackgroundHover(sender as Border);
        }

        /// <summary>
        /// On mouse leave handle, sets the background to current state (selected or not selected)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            if (GetSelectableObject() is ISelectable selectableObject && !selectableObject.IsSelected)
            {
                SetBackgroundUnSelected(sender as Border);
            }
            else
            {
                SetBackgroundSelected(sender as Border);
            }
        }

        /// <summary>
        /// On mouse down change selection action
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && GetSelectableObject() is ISelectable selectableObject)
            {
                selectableObject.IsSelected = !selectableObject.IsSelected;

                if (!selectableObject.AllowsMultiSelect && selectableObject.IsSelected)
                {
                    SelectableObjects().Where(e => e != selectableObject).ForEach(e => { e.IsSelected = false; });
                }

                if (selectableObject.IsSelected) SetBackgroundSelected(sender as Border);
                else SetBackgroundUnSelected(sender as Border);
            }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Get selectable object from associated object
        /// </summary>
        /// <returns></returns>
        private ISelectable? GetSelectableObject()
        {
            if (AssociatedObject.DataContext is ISelectable) return AssociatedObject.DataContext as ISelectable;
            return null;
        }

        /// <summary>
        /// Get list of selectable objects
        /// </summary>
        /// <returns></returns>
        private ISelectable[] SelectableObjects()
        {

            if (_containerObject != null)
            {
                ISelectable?[] items = _containerObject.FindChildrenOfType<ContentPresenter>()
                    .Where(e => e.DataContext is ISelectable).Select(e => e.DataContext as ISelectable).ToArray();

                return items ?? Array.Empty<ISelectable>();
            }

            return Array.Empty<ISelectable>();
        }

        /// <summary>
        /// Sets background color to selected
        /// </summary>
        /// <param name="border"></param>
        private void SetBackgroundSelected(Border border)
        {
            if (border == null) return;

            border.Background = new SolidColorBrush()
            {
                Color = Colors.DodgerBlue,
                Opacity = 0.3
            };
        }

        /// <summary>
        /// Sets background to hover color
        /// </summary>
        /// <param name="border"></param>
        private void SetBackgroundHover(Border border)
        {
            if (border == null) return;

            border.Background = new SolidColorBrush()
            {
                Color = Colors.Silver,
                Opacity = 0.3
            };
        }

        /// <summary>
        /// Sets background to unselected color
        /// </summary>
        /// <param name="border"></param>
        private void SetBackgroundUnSelected(Border border)
        {
            if (border == null) return;

            border.Background = new SolidColorBrush()
            {
                Color = Colors.Transparent,
                Opacity = 0.3
            };
        }
        #endregion
    }
}
