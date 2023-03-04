using AMControls.ExtensionMethods;
using AMControls.Interfaces;
using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Linq;
using Catel.Collections;
using System.Windows.Media;

namespace AMControls.Behaviors
{
    public class SelectedBehavior : Behavior<Border>
    {
        #region Fields
        private Panel? _containerObject;

        #endregion
        #region Behavior
        protected override void OnAttached()
        {
            base.OnAttached();

            _containerObject = AssociatedObject.TryFindParent<Panel>();
            AssociatedObject.MouseDown += OnMouseDown;
            AssociatedObject.MouseLeave += OnMouseLeave;
            AssociatedObject.MouseEnter += OnMouseEnter;

        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            AssociatedObject.MouseDown -= OnMouseDown;
            AssociatedObject.MouseLeave -= OnMouseLeave;
            AssociatedObject.MouseEnter -= OnMouseEnter;

        }
        #endregion

        #region Handles 

        private void OnMouseEnter(object sender, MouseEventArgs e) 
        {
            SetBackgroundHover(sender as Border);
        }

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

        private void SetBackgroundSelected(Border border) 
        {
            if (border == null) return;

            border.Background = new SolidColorBrush() 
            { 
                Color = Colors.DodgerBlue,
                Opacity = 0.3
            };
        }

        private void SetBackgroundHover(Border border)
        {
            if (border == null) return;

            border.Background = new SolidColorBrush()
            {
                Color = Colors.Silver,
                Opacity = 0.3
            };
        }

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
