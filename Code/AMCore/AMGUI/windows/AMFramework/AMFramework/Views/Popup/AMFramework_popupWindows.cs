using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFramework.Views.Popup
{
    internal class AMFramework_popupWindows
    {
        public static Components.Windows.AM_popupWindow popupConfigurations(Controller.Controller_Config config)
        {
            Views.Config.Configuration Pg = new();
            Pg.DataContext = config; // new Controller.Controller_Config(_coreSocket);

            Components.Windows.AM_popupWindow Pw = new() { Title = "Configurations" };
            Pw.ContentPage.Children.Add(Pg);

            Components.Button.AM_button nbutt = new()
            {
                IconName = FontAwesome.WPF.FontAwesomeIcon.Save.ToString(),
                Margin = new System.Windows.Thickness(3),
                CornerRadius = "20",
                GradientTransition = "DodgerBlue"
            };
            nbutt.ClickButton += ((Controller.Controller_Config)Pg.DataContext).saveClickHandle;

            Pw.add_button(nbutt);
            return Pw;
        }

        public static Components.Windows.AM_popupWindow popupProject(int ID, Controller.Controller_DBS_Projects projectController)
        {
            Views.Projects.Project_general Pg = new(projectController, ID);
            Components.Windows.AM_popupWindow Pw = new() { Title = "New project" };
            Pw.ContentPage.Children.Add(Pg);

            Components.Button.AM_button nbutt = new()
            {
                IconName = "Save",
                Margin = new System.Windows.Thickness(3),
                CornerRadius = "20",
                GradientTransition = "DodgerBlue"
            };
            nbutt.ClickButton += Pg.saveClickHandle;
            nbutt.ClickButton += Pw.AM_Close_Window_Event;

            Pw.add_button(nbutt);
            return Pw;
        }

        public static Components.Windows.AM_popupWindow popupProjectList(int ID, Controller.Controller_DBS_Projects projectController)
        {
            Views.Projects.Project_list Pg = new(projectController);
            Components.Windows.AM_popupWindow Pw = new() { Title = "Open" };
            Pw.ContentPage.Children.Add(Pg);

            Components.Button.AM_button nbutt = new()
            {
                IconName = FontAwesome.WPF.FontAwesomeIcon.Upload.ToString(),
                Margin = new System.Windows.Thickness(3),
                CornerRadius = "20",
                GradientTransition = "DodgerBlue"
            };
            nbutt.ClickButton += projectController.Select_project_Handle;
            nbutt.ClickButton += Pw.AM_Close_Window_Event;

            Pw.add_button(nbutt);
            return Pw;
        }

        public static Components.Windows.AM_popupWindow popupElementsList(Controller.Controller_DBS_Projects projectController)
        {
            Views.Elements.ElementsList Pg = new(projectController);
            Pg.DataContext = projectController; // new Controller.Controller_Config(_coreSocket);

            Components.Windows.AM_popupWindow Pw = new() { Title = "Database element list" };
            Pw.ContentPage.Children.Add(Pg);

            Components.Button.AM_button nbutt = new()
            {
                IconName = FontAwesome.WPF.FontAwesomeIcon.Check.ToString(),
                Margin = new System.Windows.Thickness(3),
                CornerRadius = "20",
                GradientTransition = "DodgerBlue"
            };
            nbutt.ClickButton += ((Controller.Controller_DBS_Projects)Pg.DataContext).Save_elementSelection_Handle;
            nbutt.ClickButton += Pw.AM_Close_Window_Event;

            Pw.add_button(nbutt);
            return Pw;
        }

        public static Components.Windows.AM_popupWindow popupCaseWindow(Controller.Controller_Cases caseController, int IDProject)
        {
            caseController.SelectedCaseOLD = new Model.Model_Case();
            caseController.SelectedCaseOLD.IDProject = IDProject;
            caseController.SelectedCaseOLD.Date = DateTime.Now.ToString("dd/MM/yyyy");

            Views.Case.Case_newitem Pg = new(caseController.SelectedCaseOLD);

            Components.Windows.AM_popupWindow Pw = new() { Title = "Case item" };
            Pw.ContentPage.Children.Add(Pg);

            Components.Button.AM_button nbutt = new()
            {
                IconName = FontAwesome.WPF.FontAwesomeIcon.Check.ToString(),
                Margin = new System.Windows.Thickness(3),
                CornerRadius = "20",
                GradientTransition = "DodgerBlue"
            };
            nbutt.ClickButton += caseController.save_Handle;
            nbutt.ClickButton += Pw.AM_Close_Window_Event;

            Pw.add_button(nbutt);
            return Pw;
        }

    }
}
