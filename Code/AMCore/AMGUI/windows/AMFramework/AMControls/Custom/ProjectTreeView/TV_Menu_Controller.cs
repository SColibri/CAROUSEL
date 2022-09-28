using AMControls.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMControls.Custom.ProjectTreeView
{
    public class TV_Menu_Controller: INotifyPropertyChanged
    {
        private List<TV_TopView> _main_Nodes = new();
        public List<TV_TopView> Main_Nodes
        {
            get { return _main_Nodes; }
            set
            {
                _main_Nodes = value;
                OnPropertyChanged("Main_Nodes");
            }
        }

        private string _searchText = "";
        public string SearchText
        {
            get {return _searchText; }
            set 
            {
                _searchText = value;
                OnPropertyChanged("SearchText");
                Search();
            }
        }

        public void Search() 
        {
            foreach (var item in _main_Nodes)
            {
                if (item.DataContext is not ISearchable) continue;
                ISearchable tempRef = (ISearchable)item.DataContext;

                if (SearchText.Length == 0) continue;
                tempRef.Search(_searchText);
            }
        }

        #region interface
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
