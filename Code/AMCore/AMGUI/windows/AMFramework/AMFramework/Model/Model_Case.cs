﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace AMFramework.Model
{
    public class Model_Case : INotifyPropertyChanged
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

        private int _id_project = -1;
        public int IDProject
        {
            get { return _id_project; }
            set
            {
                _id_project = value;
                OnPropertyChanged("IDProject");
            }
        }

        private int _id_group = -1;
        public int IDGroup
        {
            get { return _id_group; }
            set
            {
                _id_group = value;
                OnPropertyChanged("IDGroup");
            }
        }

        private string _name = "";
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        private string _script = "";
        public string Script
        {
            get { return _script; }
            set
            {
                _script = value;
                OnPropertyChanged("Script");
            }
        }

        private string _date = "";
        public string Date
        {
            get { return _date; }
            set
            {
                _date = value;
                OnPropertyChanged("Date");
            }
        }

        private double _pos_x = 0.0;
        public double PosX
        {
            get { return _pos_x; }
            set
            {
                _pos_x = value;
                OnPropertyChanged("PosX");
            }
        }

        private double _pos_y = 0.0;
        public double PosY
        {
            get { return _pos_y; }
            set
            {
                _pos_y = value;
                OnPropertyChanged("PosY");
            }
        }

        private double _pos_z = 0.0;
        public double PosZ
        {
            get { return _pos_z; }
            set
            {
                _pos_z = value;
                OnPropertyChanged("PosZ");
            }
        }

        #region Other
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

        private List<Model.Model_SelectedPhases> _SelectedPhases = new();
        public List<Model.Model_SelectedPhases> SelectedPhases
        {
            get { return _SelectedPhases; }
            set
            {
                _SelectedPhases = value;
                OnPropertyChanged("SelectedPhases");
            }
        }

        List<Model.Model_ElementComposition> _elementComposition = new();
        public List<Model.Model_ElementComposition> ElementComposition
        {
            get { return _elementComposition; }
            set
            {
                _elementComposition = value;
                OnPropertyChanged("ElementComposition");
            }
        }

        List<Model.Model_EquilibriumPhaseFraction> _equilibriumPhaseFractions = new();
        public List<Model.Model_EquilibriumPhaseFraction> EquilibriumPhaseFractions
        {
            get { return _equilibriumPhaseFractions; }
            set
            {
                _equilibriumPhaseFractions = value;
                OnPropertyChanged("EquilibriumPhaseFractions");
            }
        }

        List<Model.Model_ScheilPhaseFraction> _scheilPhaseFractions = new();
        public List<Model.Model_ScheilPhaseFraction> ScheilPhaseFractions
        {
            get { return _scheilPhaseFractions; }
            set
            {
                _scheilPhaseFractions = value;
                OnPropertyChanged("ScheilPhaseFractions");
            }
        }

        List<Model.Model_EquilibriumConfiguration> _equilibriumConfiguration = new();
        public List<Model.Model_EquilibriumConfiguration> EquilibriumConfiguration
        {
            get { return _equilibriumConfiguration; }
            set
            {
                _equilibriumConfiguration = value;
                OnPropertyChanged("EquilibriumConfiguration");
            }
        }
        #endregion

        #region Interfaces
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

    }
}