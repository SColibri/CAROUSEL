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
using AMFramework_Lib.Model;

namespace AMFramework.Views.Scheil
{
    /// <summary>
    /// Interaction logic for Scheil_configuration_General.xaml
    /// </summary>
    public partial class Scheil_configuration_General : UserControl
    {
        private Controller.Controller_ScheilConfiguration _scheilController;
        public Scheil_configuration_General()
        {
            InitializeComponent();
            UpdateControls();
        }

        public Scheil_configuration_General(Controller.Controller_ScheilConfiguration ScheilController)
        {
            InitializeComponent();
            _scheilController = ScheilController;
            DataContext = _scheilController;

            UpdateControls();
        }

        private void ButtonSelectPhase_ClickButton(object sender, EventArgs e)
        {
            if (_scheilController == null) return;
            ((Components.Button.ConfirmButton)sender).Visibility = Visibility.Collapsed;

            // update model with current Dependent phase
            foreach (var item in _scheilController.CaseController.SelectedPhases)
            {
                if(_scheilController.Model.DependentPhase == item.IDPhase) 
                { item.IsDependentPhase = true; }
                else 
                { item.IsDependentPhase = false; }
            }

            ExpanderHolder.Visibility = Visibility.Visible;
            ExpanderPhase.IsExpanded = true;
        }

        private void ButtonSavePhase_ClickButton(object sender, EventArgs e)
        {
            if (_scheilController == null) return;
            Model_SelectedPhases tempRef = _scheilController.CaseController.SelectedPhases.Find(e => e.IsDependentPhase);
            if (tempRef == null) 
            {
                MainWindow.notify.ShowBalloonTip(5000, "No phase selected!", "Please select a dependent phase", System.Windows.Forms.ToolTipIcon.Warning);
                return;
            }

            _scheilController.Model.DependentPhase = tempRef.IDPhase;
            _scheilController.save(_scheilController.Model);
            MainWindow.notify.ShowBalloonTip(5000, "Dependent Phase Saved!", "Dependent phase was updated, nice work!", System.Windows.Forms.ToolTipIcon.Info);

            ExpanderHolder.Visibility = Visibility.Collapsed;
            ButtonSelectPhase.Visibility = Visibility.Visible;

            UpdateControls();
        }

        private void UpdateControls() 
        {
            if (_scheilController == null) return;
            ButtonSelectPhase.Text = _scheilController.Model.DependentPhaseName;

            if(_scheilController.CaseController.ScheilPhaseFraction.Count > 0) 
            {
                ScheilCalcCheckIcon.Icon = FontAwesome.WPF.FontAwesomeIcon.CheckCircle;
                ScheilCalcCheckIcon.Foreground = new SolidColorBrush(Colors.Green);
                ScheilCalcCheckIcon.ToolTip = "Data is available";
                ExportButton.Visibility = Visibility.Visible;
                ViewTable.Visibility = Visibility.Visible;
            }
            else 
            {
                ScheilCalcCheckIcon.Icon = FontAwesome.WPF.FontAwesomeIcon.CheckSquareOutline;
                ScheilCalcCheckIcon.Foreground = new SolidColorBrush(Colors.Silver);
                ScheilCalcCheckIcon.ToolTip = "No calculations have been done";
                ExportButton.Visibility= Visibility.Collapsed;
                ViewTable.Visibility = Visibility.Collapsed;
            }

            if (_scheilController.CaseController.PrecipitationPhaseController.PrecipitationPhases.Count > 0)
            {
                PrecipitateDistributionCheck.Icon = FontAwesome.WPF.FontAwesomeIcon.CheckCircle;
                PrecipitateDistributionCheck.Foreground = new SolidColorBrush(Colors.Green);
                PrecipitateDistributionCheck.ToolTip = "Data is available";
                ExportPrecipitateDistributionButton.Visibility = Visibility.Visible;
                ViewPrecipitateDistribution.Visibility = Visibility.Visible;
            }
            else 
            {
                PrecipitateDistributionCheck.Icon = FontAwesome.WPF.FontAwesomeIcon.CheckSquareOutline;
                PrecipitateDistributionCheck.Foreground = new SolidColorBrush(Colors.Silver);
                PrecipitateDistributionCheck.ToolTip = "No calculations have been done";
                ExportPrecipitateDistributionButton.Visibility= Visibility.Collapsed;
                ViewPrecipitateDistribution.Visibility= Visibility.Collapsed;
            }
        }

        private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (DataContext == null) return;
            if (DataContext.GetType().Equals(typeof(Controller.Controller_ScheilConfiguration))) 
            {
                _scheilController = DataContext as Controller.Controller_ScheilConfiguration;
                UpdateControls();
            }
        }
    }
}
