﻿using AMControls.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AMControls.Custom.ProjectTreeView
{
    public class TV_TopView_controller: INotifyPropertyChanged, ISearchable
    {
        private int _id = -1;
        public int ID 
        { 
            get { return _id; }
            set 
            { 
                _id = value;
                OnPropertyChanged("ID");
            }
        }

        private FontAwesome.WPF.FontAwesomeIcon _iconObject = FontAwesome.WPF.FontAwesomeIcon.ObjectGroup;
        public FontAwesome.WPF.FontAwesomeIcon IconObject
        {
            get { return _iconObject; }
            set
            {
                _iconObject = value;
                OnPropertyChanged("IconObject");
            }
        }

        private string _title = "New title";
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged("Title");
            }
        }

        private bool _isExpanded = false;
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                if(Items.Count > 0) _isExpanded = value;
                else _isExpanded = false;
                
                OnPropertyChanged("IsExpanded");
            }
        }

        private bool _isSelected = false;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }

        private List<object> _items = new();
        public List<object> Items
        {
            get
            {
                return _items;
            }
            set
            {
                _items = value;
                OnPropertyChanged("Items");
            }
        }

        private List<object> _tools = new();
        public List<object> Tools
        {
            get
            {
                return _tools;
            }
            set
            {
                _tools = value;
                OnPropertyChanged("Tools");
            }
        }

        #region Methods
        public void Add_Item(object itemToAdd) 
        { 
            Items.Add(itemToAdd);
            OnPropertyChanged("Items");
        }

        public void Clear_Items()
        {
            Items.Clear();
            OnPropertyChanged("Items");
        }


        public bool Search_Tree(List<int> IDElements, int Index) 
        {
            bool isContained = false;

            if (IDElements.Count >= Index) 
            {
                IsExpanded = false;
                IsSelected = false;
            }
            if (ID == IDElements[Index]) 
            {
                if(IDElements.Count == Index) isContained = true;
                else if (IDElements.Count > Index) 
                {
                    foreach (var item in Items)
                    {
                        if (item is not TV_TopView) continue;
                        TV_TopView topView = (TV_TopView)item;

                        if (topView.DataContext is not TV_TopView_controller) continue;
                        TV_TopView_controller topController = (TV_TopView_controller)topView.DataContext;

                        if (topController.Search_Tree(IDElements, Index++)) isContained = true;
                    }
                }

                if (isContained) 
                {
                    IsExpanded = true;
                    IsSelected = true;
                }
    
            }


            return isContained;
        }

        public bool Search(object searchObject)
        {
            if (searchObject is string) return Search_string((string)searchObject);
            else if (searchObject is List<int>) return Search_Tree((List<int>)searchObject, 0);

            throw new NotImplementedException();
        }
        public bool Search_string(string textString)
        {
            bool isContained = false;

            if (Title.ToLower().Contains(textString.ToLower())) isContained = true;
       
            foreach (var item in Items)
            {
                if (item is not TV_TopView) continue;
                TV_TopView topView = (TV_TopView)item;

                if (topView.DataContext is not TV_TopView_controller) continue;
                TV_TopView_controller topController = (TV_TopView_controller)topView.DataContext;

                if (topController.Search_string(textString.ToLower())) isContained = true;
            }

            if (isContained)
            {
                IsExpanded = true;
                IsSelected = true;
            }
            else 
            {
                IsExpanded = false;
                IsSelected = false;
            }

            return isContained;
        }
        #endregion

        #region interface
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

    }
}
