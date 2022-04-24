using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AMFramework.Controller
{
    public class Controller_Config: INotifyPropertyChanged
    {

        #region Cons_Des
            public Controller_Config()
            {

            }
        #endregion

        #region Interfaces
            public event PropertyChangedEventHandler PropertyChanged;

            protected void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        #endregion

        #region Model
            private String _APIPath = "NoPath";
            public String APIPath
            {
                get { return _APIPath; }
                set
                {
                    _APIPath = value;
                    OnPropertyChanged("APIPath");
                }
            }
        #endregion

        #region Logic

            private ICommand _Search_API_Path;
            public ICommand Search_API_Path
            {
                get
                {
                    if (_Search_API_Path == null)
                    {
                        _Search_API_Path = new RelayCommand(
                            param => this.Search_API_path_controll(),
                            param => this.Can_Change_API_path()
                        );
                    }
                    return _Search_API_Path;
                }
            }

            private void Search_API_path_controll()
            {
                OpenFileDialog OFD = new OpenFileDialog();
                OFD.Multiselect = false;
                OFD.Filter = "API library|*.dll";
                OFD.Title = "Select the CALPHAD API library";
                OFD.CheckFileExists = true;

                if (OFD.ShowDialog() == true)
                {
                    APIPath = OFD.FileName;
                }
            }

            private bool Can_Change_API_path()
            {
                return true;
            }

        #endregion


    }
}
