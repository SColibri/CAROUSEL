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

namespace AMFramework.Views.Case
{
    /// <summary>
    /// Interaction logic for Case_general.xaml
    /// </summary>
    public partial class Case_general : UserControl
    {
        public Case_general()
        {
            InitializeComponent();
        }

        public Case_general(ref Controller.Controller_Cases caseController)
        {
            InitializeComponent();
            DataContext = caseController;
            caseController.ShowPopup = false;
        }

        ~Case_general() 
        {

        }

        private void Phase_select_handle(object sender, RoutedEventArgs e)
        {
            Controller.Controller_Cases controllerCase = (Controller.Controller_Cases)DataContext;
            controllerCase.PopupView.ContentPage.Children.Add(new Views.Phase.Phase_List(controllerCase.get_project_controller()));
            controllerCase.PopupView.Title = "Phases";
            controllerCase.PopupView.clear_buttons();

            Components.Button.AM_button saveSelection = new();
            saveSelection.IconName = "Check";
            saveSelection.GradientTransition = "green";
            saveSelection.ClickButton += Accept_phase_selection;

            controllerCase.PopupView.add_button(saveSelection);
            

            controllerCase.ShowPopup = true;
            controllerCase.PopupView.PopupWindowClosed += Close_popup_handle;

            PopupFrame.Navigate(controllerCase.PopupView);
        }
        private void Phase_select_handle(object sender, EventArgs e)
        {
            Controller.Controller_Cases controllerCase = (Controller.Controller_Cases)DataContext;
            controllerCase.PopupView.ContentPage.Children.Add(new Views.Phase.Phase_List(controllerCase.get_project_controller()));
            controllerCase.PopupView.Title = "Phases";
            controllerCase.PopupView.clear_buttons();

            Components.Button.AM_button saveSelection = new();
            saveSelection.IconName = "Check";
            saveSelection.GradientTransition = "green";
            saveSelection.ClickButton += Accept_phase_selection;

            controllerCase.PopupView.add_button(saveSelection);


            controllerCase.ShowPopup = true;
            controllerCase.PopupView.PopupWindowClosed += Close_popup_handle;

            PopupFrame.Navigate(controllerCase.PopupView);
        }

        private void Accept_phase_selection(object sender, EventArgs e)
        {
            Controller.Controller_Cases controllerCase = (Controller.Controller_Cases)DataContext;
            controllerCase.get_project_controller().Case_load_equilibrium_phase_fraction(controllerCase.SelectedCase);

            if (controllerCase.SelectedCase.EquilibriumPhaseFractions.Count > 0 ||
                controllerCase.SelectedCase.ScheilPhaseFractions.Count > 0) 
            {

                if (System.Windows.Forms.MessageBox.Show("Changing this selection will delete all calculations, do you want to proceed?",
                                                        "Phase selection", System.Windows.Forms.MessageBoxButtons.YesNo) != System.Windows.Forms.DialogResult.Yes)
                { return; }

            }

            controllerCase.get_project_controller().set_phase_selection_to_current_case();
        }

        private void Button_Add_PrecipitationPhase_Click(object sender, RoutedEventArgs e)
        {
            Controller.Controller_Cases controllerCase = (Controller.Controller_Cases)DataContext;
            controllerCase.SelectedCase.PrecipitationPhases.Add(new());
            controllerCase.SelectedCase.PrecipitationPhases[controllerCase.SelectedCase.PrecipitationPhases.Count - 1].IDCase = controllerCase.SelectedCase.ID;

            Views.Precipitation.PrecipitationPhase_general windowPPhase = new() { DataContext = controllerCase.SelectedCase.PrecipitationPhases[controllerCase.SelectedCase.PrecipitationPhases.Count - 1] };
            controllerCase.PopupView.ContentPage.Children.Clear();
            controllerCase.PopupView.ContentPage.Children.Add(windowPPhase);
            controllerCase.PopupView.Title = "precipitation Phase";
            controllerCase.PopupView.clear_buttons();

            Components.Button.AM_button saveSelection = new();
            saveSelection.IconName = "Check";
            saveSelection.GradientTransition = "green";

            controllerCase.PopupView.add_button(saveSelection);

            controllerCase.ShowPopup = true;
            controllerCase.PopupView.PopupWindowClosed += Close_popup_handle;

            PopupFrame.Navigate(controllerCase.PopupView);
        }

        private void Show_popup_precipitate_phase(Model.Model_PrecipitationPhase model)
        {
            Controller.Controller_Cases controllerCase = (Controller.Controller_Cases)DataContext;

            Views.Precipitation.PrecipitationPhase_general windowPPhase = new() { DataContext = model };
            controllerCase.PopupView.ContentPage.Children.Clear();
            controllerCase.PopupView.ContentPage.Children.Add(windowPPhase);
            controllerCase.PopupView.Title = model.Name;
            controllerCase.PopupView.clear_buttons();

            Components.Button.AM_button saveSelection = new();
            saveSelection.IconName = "Check";
            saveSelection.GradientTransition = "green";
            saveSelection.ClickButton += controllerCase.PrecipitationPhaseController.Handle_ClickOnSave_AMButton;
            saveSelection.ClickButton += Close_popup_handle;

            controllerCase.PopupView.add_button(saveSelection);

            controllerCase.ShowPopup = true;
            controllerCase.PopupView.PopupWindowClosed += Close_popup_handle;

            PopupFrame.Navigate(controllerCase.PopupView);
        }


        private void Close_popup_handle(object sender, EventArgs e) 
        {
            Controller.Controller_Cases controllerCase = (Controller.Controller_Cases)DataContext;
            controllerCase.ShowPopup = false;
            controllerCase.PopupView.ContentPage.Children.Clear();
            controllerCase.PopupView.PopupWindowClosed -= Close_popup_handle;
        }


        #region Handles
        private void Handle_edit_precipitation_phase(object sender, EventArgs e) 
        {
            if (!sender.GetType().Equals(typeof(Model.Model_PrecipitationPhase))) return;

            Model.Model_PrecipitationPhase phase = (Model.Model_PrecipitationPhase)sender;

            Show_popup_precipitate_phase((Model.Model_PrecipitationPhase)sender);
        }


        #endregion

    }
}
