using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AMFramework
{
    public class MainWindow_ViewModel:Interfaces.ViewModel_Interface
    {
        private List<Components.Scripting.Scripting_ViewModel> _openScripts = new List<Components.Scripting.Scripting_ViewModel>();
        public List<Components.Scripting.Scripting_ViewModel> OpenScripts { get { return _openScripts; } }

        private Views.Projects.Project_ViewModel _projects = new Views.Projects.Project_ViewModel();
        public Views.Projects.Project_ViewModel Projects { get { return _projects; } }

        private Views.Case.Case_ViewModel _caseView = new Views.Case.Case_ViewModel();
        public Views.Case.Case_ViewModel CaseView { get { return _caseView; } }

        #region Interface
        public bool close()
        {
            throw new NotImplementedException();
        }

        public bool save()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Methods
        public TabItem get_new_lua_script(string filename = "")
        {
            TabItem result = new TabItem();

            string headerTitle = filename;
            Uri ImageUri = null; //TODO add lua Icon here
            if(headerTitle.Length == 0)
            {
                result.Header = get_TabHeader("New Lua script", ImageUri);
            }
            else
            {
                result.Header = get_TabHeader(headerTitle, ImageUri);
            }

            _openScripts.Add(new Components.Scripting.Scripting_ViewModel() { Filename=filename });
            
            result.Content = _openScripts[^1].get_text_editor();
            result.Tag = _openScripts[^1];

            if (System.IO.File.Exists(filename)) ((Components.Scripting.Scripting_editor)result.Content).loadFile(filename);

            return result;
        }

        public TabItem get_new_plot(string plotName = "")
        {
            TabItem result = new TabItem();

            string headerTitle = plotName;
            Uri ImageUri = null; //TODO add lua Icon here
            if (headerTitle.Length == 0)
            {
                result.Header = get_TabHeader("New Plot", ImageUri);
            }
            else
            {
                result.Header = get_TabHeader(headerTitle, ImageUri);
            }

            Components.Charting.ChartingWindow charty = new();
            Components.Charting.Axes Testy = new Components.Charting.Axes("Test");
            Testy.MinValue = 0;
            Testy.MaxValue = 10;

            charty.Add_Axe(Testy);
            charty.Add_Axe(new Components.Charting.Axes("Test"));
            charty.Add_Axe(new Components.Charting.Axes("Test"));
            charty.Add_Axe(new Components.Charting.Axes("Test"));
            charty.UpdateImage();

            result.Content = charty;
            result.Tag = new Components.Charting.Charting_ViewModel();

            return result;
        }

        public TabItem get_project_tab(Controller.Controller_DBS_Projects projectController) 
        {
            TabItem result = new TabItem();

            string headerTitle = projectController.SelectedProject.Name;
            Uri ImageUri = null; //TODO add lua Icon here
            if (headerTitle.Length == 0)
            {
                result.Header = get_TabHeader("Project without name", ImageUri);
            }
            else
            {
                result.Header = get_TabHeader(headerTitle, ImageUri);
            }

            result.Content = _projects.get_project_content(projectController);
            result.Tag = _projects;

            return result;
        }

        public TabItem get_case_list_tab(Controller.Controller_Plot plotController)
        {
            TabItem result = new TabItem();

            string headerTitle = plotController.SelectedProject.Name;
            Uri ImageUri = null; //TODO add lua Icon here
            if (headerTitle.Length == 0)
            {
                result.Header = get_TabHeader("Cases", ImageUri);
            }
            else
            {
                result.Header = get_TabHeader(headerTitle, ImageUri);
            }

            result.Content = _caseView.get_content(plotController);
            result.Tag = _caseView;

            return result;
        }

        public TabItem get_case_itemTab(Controller.Controller_Cases controllerCase)
        {
            TabItem result = new TabItem();

            string headerTitle = "Case: " + controllerCase.SelectedCase.Name;
            Uri ImageUri = null; //TODO add lua Icon here
            if (headerTitle.Length == 0)
            {
                result.Header = get_TabHeader("Cases", ImageUri);
            }
            else
            {
                result.Header = get_TabHeader(headerTitle, ImageUri);
            }

            result.Content = _caseView.get_item(controllerCase);
            result.Tag = _caseView;

            return result;
        }

        #endregion

        #region Formatting
        public Grid get_TabHeader(string TabTitle, Uri uriImage)
        {
            Grid grid = new Grid();
            ColumnDefinition CDef_01 = new ColumnDefinition();
            CDef_01.Width = new GridLength(25);
            ColumnDefinition CDef_02 = new ColumnDefinition();
            CDef_01.Width = new GridLength(1, GridUnitType.Star);

            grid.ColumnDefinitions.Add(CDef_01);
            grid.ColumnDefinitions.Add(CDef_02);

            Image image = new Image();
            if (uriImage != null)
            {
                ImageSource imS = new BitmapImage(uriImage);
                image.Source = imS;
            }

            TextBlock textBlock = new TextBlock();
            textBlock.FontWeight = FontWeights.DemiBold;
            textBlock.Text = TabTitle;

            Grid.SetColumn(image, 0);
            Grid.SetColumn(textBlock, 0);
            grid.Children.Add(textBlock);
            grid.Children.Add(image);

            return grid;
        }

        #endregion

    }
}
