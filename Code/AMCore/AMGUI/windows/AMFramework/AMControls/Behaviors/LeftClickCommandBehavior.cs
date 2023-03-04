using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AMControls.Behaviors
{
    public class LeftClickCommandBehavior : Behavior<FrameworkElement>
    {
        #region Fields

        #endregion

        #region Properties

        public static readonly DependencyProperty ParameterProperty = DependencyProperty.Register("CommandParameter", typeof(object), typeof(LeftClickCommandBehavior), new PropertyMetadata(null));
        /// <summary>
        /// Command parameter
        /// </summary>
        public object CommandParameter 
        {
            get => GetValue(ParameterProperty);
            set => SetValue(ParameterProperty, value);
        }

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(LeftClickCommandBehavior), new PropertyMetadata(null));
        /// <summary>
        /// Command parameter
        /// </summary>
        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        #endregion

        #region Behavior
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.MouseDown += OnMouseDown;
        }
        protected override void OnDetaching()
        {
            base.OnDetaching();

            AssociatedObject.MouseDown -= OnMouseDown;
        }
        #endregion

        #region Methods

        private void OnMouseDown(object sender, MouseButtonEventArgs e) 
        { 
            if (e.LeftButton == MouseButtonState.Pressed && Command != null) 
            {
                Command.Execute(CommandParameter);
            }
        }

        #endregion
    }
}
