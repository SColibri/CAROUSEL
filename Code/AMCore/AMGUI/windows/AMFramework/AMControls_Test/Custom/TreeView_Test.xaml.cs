using AMControls.Custom.ProjectTreeView;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Documents.DocumentStructures;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AMControls_Test.Custom
{
    


    /// <summary>
    /// Interaction logic for TreeView_Test.xaml
    /// </summary>
    public partial class TreeView_Test : Window, INotifyPropertyChanged
    {
        public class ListItem: INotifyPropertyChanged
        {
            private string _name;
            public string Name
            {
                get { return _name; }
                set
                {
                    _name = value;
                    OnPropertyChanged("Name");
                }
            }
            private List<ListItem> _items;
            public List<ListItem> Items
            {
                get { return _items; }
                set
                {
                    _items = value;
                    OnPropertyChanged("Items");
                }
            }


            public ListItem()
            {
                Name = "New";
                Items = new();
            }

            public event PropertyChangedEventHandler? PropertyChanged;
            protected void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private List<object> _itemObj = new();
        public List<object> ItemObj
        {
            get { return _itemObj; }
            set
            {
                _itemObj = value;
                OnPropertyChanged(nameof(ItemObj));
            }
        }

        private TV_TopView_controller _tvController = new();

        public TV_TopView_controller TVController 
        { 
            get { return _tvController; }
            set 
            {
                _tvController = value;
                OnPropertyChanged("TVController");
            }
        }

        public TreeView_Test()
        {
            InitializeComponent();
            DataContext = this;
            tr.DataContext = TVController;

            _mainTreeList = new();

            for (int i = 0; i < 5; i++)
            {
                ListItem listItem = new ListItem();
                listItem.Name = "Main_" + i.ToString();

                for (int j = 0; j < 5; j++)
                {
                    ListItem L1Item = new ListItem();
                    L1Item.Name = "L1_" + i.ToString();

                    for (int k = 0; k < 5; k++)
                    {
                        ListItem L2Item = new ListItem();
                        L2Item.Name = "L2_" + i.ToString();

                        L1Item.Items.Add(L2Item);
                    }

                    listItem.Items.Add(L1Item);
                }

                _mainTreeList.Add(listItem);
            }

            OnPropertyChanged("MainTreeList");

            TVController.Items.Add(new TV_TopView(new()));
            TVController.Items.Add(new TextBlock() { Text = "Wohooo"});
        }

        private List<ListItem> _mainTreeList;
        public List<ListItem> MainTreeList
        {
            get { return _mainTreeList; }
            set 
            { 
                _mainTreeList = value;
                OnPropertyChanged("MainTreeList");
            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
