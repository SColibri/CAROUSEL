using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AMFramework.Views.Projects
{
    /// <summary>
    /// Interaction logic for Project_contents.xaml
    /// </summary>
    public partial class Project_contents : UserControl
    {
        public Project_contents()
        {
            InitializeComponent();
        }

        public Project_contents(ref Controller.Controller_DBS_Projects pController)
        {
            InitializeComponent();
            DataContext = pController;
        }

        private void ConfirmButton_ClickButton(object sender, EventArgs e)
        {
            Popup.Visibility = Visibility.Visible;

            Components.Windows.AM_popupWindow newPopup = Views.Popup.AMFramework_popupWindows.popupElementsList((Controller.Controller_DBS_Projects)DataContext);
            newPopup.PopupWindowClosed += Close_popup_Handle;
            PopupFrame.Navigate(newPopup);
        }

        private void AddNewCase_ClickButton(object sender, EventArgs e)
        {
            Popup.Visibility = Visibility.Visible;

            Components.Windows.AM_popupWindow newPopup = Views.Popup.AMFramework_popupWindows.popupCaseWindow(((Controller.Controller_DBS_Projects)DataContext).ControllerCases,
                                                                                                              ((Controller.Controller_DBS_Projects)DataContext).SelectedProject.ID);
            newPopup.PopupWindowClosed += Close_popup_Handle;
            PopupFrame.Navigate(newPopup);
        }

        private void Close_popup_Handle(object sender, EventArgs e) 
        {
            Popup.Visibility = Visibility.Collapsed;
        }

        
    }
}
