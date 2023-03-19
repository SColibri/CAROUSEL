using AMFramework.Controller;
using System.Windows.Controls;

namespace AMFramework.Components.LogDisplay
{
    /// <summary>
    /// Interaction logic for MessageLogControl.xaml
    /// </summary>
    public partial class MessageLogControl : UserControl
    {
        public MessageLogControl()
        {
            InitializeComponent();
        }

		private void MenuItem_Click(object sender, System.Windows.RoutedEventArgs e)
		{
            if (DataContext is ControllerCallbacks controller) 
            { 
                controller.SelectAllMessages();
                InvalidateVisual();
            }
        }
    }
}
