using System.Windows;
using System.Windows.Controls;

namespace AMControls.Custom
{
    public class TreeViewItem_custom_AM : TreeViewItem
    {
        public static readonly DependencyProperty IDProperty =
        DependencyProperty.Register("ID",
                                    typeof(int),
                                    typeof(TreeViewItem_custom_AM),
                                    new PropertyMetadata(default(int), OnIDPropertyChanged));

        private static void OnIDPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // do nothing
        }

        private int _id = -1;
        public int ID
        {
            get { return (int)GetValue(IDProperty); }
            set
            {
                _id = value;
                SetValue(IDProperty, value);
            }
        }
    }
}
