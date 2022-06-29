﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace AMFramework.Controller
{
    public class Controller_Selected_Elements : INotifyPropertyChanged
    {

        #region Socket
        private Core.AMCore_Socket _AMCore_Socket;
        private Controller.Controller_DBS_Projects _ProjectController;
        public Controller_Selected_Elements(ref Core.AMCore_Socket socket, Controller.Controller_DBS_Projects projectController)
        {
            _AMCore_Socket = socket;
            _ProjectController = projectController;
        }
        
        #endregion

        #region Interfaces
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Data
        private List<Model.Model_SelectedElements> _Elements = new();
        public List<Model.Model_SelectedElements> Elements 
        { 
            get { return _Elements; } 
        }

        #endregion

        #region Methods
        public void refresh() 
        {
            load_elements();
        }
        private void load_elements() 
        {
            string Query = "database_table_custom_query SELECT SelectedElements.*, Element.Name FROM SelectedElements INNER JOIN Element ON Element.ID=SelectedElements.IDElement WHERE IDProject = " + _ProjectController.SelectedProject.ID.ToString();
            string outy = _AMCore_Socket.send_receive(Query);
            List<string> rowItems = outy.Split("\n").ToList();
            _Elements.Clear();

            foreach (string item in rowItems)
            {
                List<string> columnItems = item.Split(",").ToList();


                if (columnItems.Count > 3)
                {
                    Model.Model_SelectedElements model = new Model.Model_SelectedElements();
                    model.ID = Convert.ToInt32(columnItems[0]);
                    model.IDProject = Convert.ToInt32(columnItems[1]);
                    model.IDElement = Convert.ToInt32(columnItems[2]);
                    model.ISReferenceElement = Convert.ToUInt16(columnItems[3]);
                    model.ElementName = columnItems[4];

                    _Elements.Add(model);
                }
            }

            OnPropertyChanged("Elements");
        }
        #endregion

    }
}