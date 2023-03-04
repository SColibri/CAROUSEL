using System;
using System.Windows;
using System.Windows.Controls;

namespace AMFramework.Views.Precipitation
{
    /// <summary>
    /// Interaction logic for PrecipitationPhase_List.xaml
    /// </summary>
    public partial class PrecipitationPhase_List : UserControl
    {
        public PrecipitationPhase_List()
        {
            InitializeComponent();
        }

        private void AM_button_remove_ClickButton(object sender, EventArgs e)
        {
            DeleteObject?.Invoke(((Components.Button.AM_button)sender).ModelTag, new EventArgs());
        }

        private void AM_button_edit_ClickButton(object sender, EventArgs e)
        {
            OpenObject?.Invoke(((Components.Button.AM_button)sender).Tag, new EventArgs());
        }

        public event EventHandler DeleteObject;
        public event EventHandler OpenObject;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenObject?.Invoke(((Button)sender).Tag, new EventArgs());
        }
    }
}
