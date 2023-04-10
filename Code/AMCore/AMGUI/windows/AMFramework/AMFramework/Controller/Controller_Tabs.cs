using AMFramework.Components.ScriptingEditor;
using AMFramework_Lib.Controller;
using AMFramework_Lib.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AMFramework.Controller
{
    public class Controller_Tabs : ControllerAbstract
    {
        public Controller_Tabs()
        {

        }

        #region Properties
        private bool _tabControl_Visible = true;
        /// <summary>
        /// Gets/sets if that container is visible
        /// </summary>
        public bool TabControlVisible
        {
            get { return _tabControl_Visible; }
            set
            {
                _tabControl_Visible = value;
                OnPropertyChanged(nameof(TabControlVisible));
            }
        }

        private TabItem? _selectedTab;
        /// <summary>
        /// sets/gets the selected tab item
        /// </summary>
        public TabItem? SelectedTab
        {
            get { return _selectedTab; }
            set
            {
                _selectedTab = value;
                OnPropertyChanged(nameof(SelectedTab));
            }
        }

        private ObservableCollection<TabItem> _tabItems = new();
        /// <summary>
        /// List of all available tab items
        /// </summary>
        public ObservableCollection<TabItem> TabItems
        {
            get { return _tabItems; }
            set
            {
                _tabItems = value;
                OnPropertyChanged(nameof(TabItems));
            }
        }

        private bool _dataHasChanged = false;
        public bool DataHasChanged
        {
            get => _dataHasChanged;
            set
            {
                _dataHasChanged = value;
                OnPropertyChanged(nameof(DataHasChanged));
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Adds a new tab item
        /// </summary>
        /// <param name="itemy"></param>
        public void Add_Tab_Item(TabItem itemy)
        {
            TabItems.Add(itemy);
            SelectedTab = itemy;
            DataHasChanged = true;
        }

        /// <summary>
        /// Removes a tab item
        /// </summary>
        /// <param name="itemy"></param>
        public void Remove_tab_Item(TabItem itemy)
        {
            TabItems.Remove(itemy);
            UpdateTabItems();
            DataHasChanged = true;
            TabClosed?.Invoke(itemy, EventArgs.Empty);
        }

        /// <summary>
        /// Removes tab based on the Tag property
        /// </summary>
        /// <param name="objType"></param>
        public void Remove_ByTagType(Type objType)
        {
            bool tagFound = true;
            while (tagFound)
            {
                tagFound = false;
                for (int i = 0; i < TabItems.Count; i++)
                {
                    if (TabItems[i].Tag == null) continue;
                    if (TabItems[i].Tag.GetType().Equals(objType) &&
                        !TabItems[i].Tag.GetType().Equals(typeof(Scripting_ViewModel)))
                    {
                        // Note removing tabs and WPF cause a binding error, however,
                        // this is caused by a windows wpf. harmless
                        TabItems.RemoveAt(i);
                        tagFound = true;
                    }
                }
            }


            //TabItems.RemoveAll(e => e.Tag.GetType().Equals(objType));
            UpdateTabItems();
        }

        /// <summary>
        /// This implementation just avoids using the IAbservable list
        /// 
        /// </summary>
        private void UpdateTabItems()
        {
            if (SelectedTab == null && TabItems.Count > 0)
            {
                SelectedTab = TabItems.Last();
            }
        }

        /// <summary>
        /// Returns the tabItem Template
        /// </summary>
        /// <param name="itemView"></param>
        /// <param name="modelObject"></param>
        /// <param name="tabTitle"></param>
        /// <returns></returns>
        public TabItem Create_Tab(object itemView, object? modelObject, string tabTitle)
        {
            TabItem result = new();

            string headerTitle = tabTitle;
            Uri ImageUri = null; //TODO add lua Icon here
            if (headerTitle.Length == 0)
            {
                result.Header = Get_TabHeader(tabTitle, ImageUri);
            }
            else
            {
                result.Header = Get_TabHeader(headerTitle, ImageUri);
            }

            //Remove existing tabs of existing type, tabs with null are
            //Ignored
            if (modelObject != null)
            {
                Remove_ByTagType(modelObject.GetType());
            }

            result.Content = itemView;
            result.Tag = modelObject;

            Add_Tab_Item(result);
            return result;
        }
        #endregion

        #region StaticMethods
        /// <summary>
        /// Returns the header template
        /// </summary>
        /// <param name="TabTitle"></param>
        /// <param name="uriImage"></param>
        /// <returns></returns>
        private Grid Get_TabHeader(string TabTitle, Uri uriImage)
        {
            Grid grid = new();
            ColumnDefinition CDef_01 = new()
            {
                Width = new GridLength(25)
            };
            ColumnDefinition CDef_02 = new();
            CDef_01.Width = new GridLength(1, GridUnitType.Star);

            grid.ColumnDefinitions.Add(CDef_01);
            grid.ColumnDefinitions.Add(CDef_02);

            Image image = new();
            if (uriImage != null)
            {
                ImageSource imS = new BitmapImage(uriImage);
                image.Source = imS;
            }

            TextBlock textBlock = new()
            {
                FontWeight = FontWeights.DemiBold,
                Text = TabTitle
            };

            Grid.SetColumn(image, 0);
            Grid.SetColumn(textBlock, 1);
            grid.Children.Add(textBlock);
            grid.Children.Add(image);

            return grid;
        }
        #endregion

        #region Command

        #region CloseTab

        private ICommand? _closeTabCommand;
        /// <summary>
        /// Command for closing a tab, it uses ViewModel_Interface for calling the
        /// close command
        /// </summary>
        public ICommand? CloseTabCommand
        {
            get
            {
                return _closeTabCommand ??= new RelayCommand(param => CloseTabCommand_Action(param), param => CloseTabCommand_Check());
            }
        }

        private void CloseTabCommand_Action(object? vmInterface)
        {
            // Check if selected tab is not null
            if (vmInterface is TabItem tabItem)
            {
                if (tabItem.Tag is ViewModel_Interface vmObject)
                {
                    // Calls the close action
                    if (vmObject.Close())
                        Remove_tab_Item(tabItem);
                }
                else
                {
                    // closes the tab without calling the close action
                    Remove_tab_Item(tabItem);
                }
            }
        }

        private bool CloseTabCommand_Check()
        {
            return true;
        }
		#endregion
		#endregion

		#region Events
		public event EventHandler TabClosed;

        #endregion
    }
}
