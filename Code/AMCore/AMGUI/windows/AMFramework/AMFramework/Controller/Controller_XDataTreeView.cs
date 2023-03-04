using AMControls.Custom.ProjectTreeView;
using AMFramework.Components.Button;
using AMFramework_Lib.Controller;
using AMFramework_Lib.Model;
using AMFramework_Lib.Model.Model_Controllers;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AMFramework.Controller
{
    public class Controller_XDataTreeView : ControllerAbstract
    {

        #region FTR
        private TV_TopView_controller _dtv_Controller = new();
        public TV_TopView_controller DTV_Controller
        {
            get { return _dtv_Controller; }
            set
            {
                _dtv_Controller = value;
                OnPropertyChanged(nameof(DTV_Controller));
            }
        }

        public void Refresh_DTV(ControllerM_Project? selectedProject)
        {

            _dtv_Controller.Title = selectedProject.MCObject.ModelObject.Name;
            _dtv_Controller.ID = selectedProject.MCObject.ModelObject.ID;

            List<object> listy = new();

            WrapPanel ToolPanel = new()
            {
                Orientation = Orientation.Horizontal,
                FlowDirection = FlowDirection.RightToLeft,
                Margin = new Thickness(3, 3, 3, 3)
            };

            AM_button plotAccess = new()
            {
                IconName = "AreaChart",
                Width = 20,
                Height = 20,
                GradientColor_2 = "White",
                ForegroundIcon = "DodgerBlue",
                GradientTransition = "SteelBlue",
                Margin = new Thickness(2, 2, 2, 2),
                CornerRadius = "2",
                ModelTag = selectedProject.MCObject.ModelObject
            };
            plotAccess.ClickButton += OnMouseClick_Plot_Handle;
            ToolPanel.Children.Add(plotAccess);

            AM_button editAccess = new()
            {
                IconName = "Edit",
                Width = 20,
                Height = 20,
                GradientColor_2 = "White",
                ForegroundIcon = "DodgerBlue",
                GradientTransition = "SteelBlue",
                Margin = new Thickness(2, 2, 2, 2),
                CornerRadius = "2",
                ModelTag = selectedProject.MCObject.ModelObject
            };
            editAccess.ClickButton += OnMouseClick_Edit_Handle;
            ToolPanel.Children.Add(editAccess);

            listy.Add(ToolPanel);

            _dtv_Controller.Clear_Items();
            listy.Add(new TV_TopView(dtv_Add_elements(selectedProject)));
            listy.Add(new TV_TopView(dtv_Add_activePhases()));
            listy.Add(new TV_TopView(dtv_Add_singlePixelCases(selectedProject)));
            listy.Add(new TV_TopView(dtv_Add_object()));
            _dtv_Controller.Items = listy;

            OnPropertyChanged(nameof(DTV_Controller));
        }

        private void OnMouseClick_Plot_Handle(object? sender, EventArgs e)
        {
            if (sender is not AM_button) return;
            AM_button sRef = (AM_button)sender;

            if (sRef.ModelTag is not Model_Projects) return;
            Model_Projects mRef = (Model_Projects)sRef.ModelTag;

            Controller_Global.MainControl?.Show_Project_PlotView(mRef);
        }

        private void OnMouseClick_Edit_Handle(object? sender, EventArgs e)
        {
            if (sender is not AM_button) return;
            AM_button sRef = (AM_button)sender;

            if (sRef.ModelTag is not Model_Projects) return;
            Model_Projects mRef = (Model_Projects)sRef.ModelTag;

            Controller_Global.MainControl?.Show_Project_EditView(mRef);
        }

        /// <summary>
        /// Add elements tree item
        /// </summary>
        /// <param name="selectedProject"></param>
        /// <returns></returns>
        private TV_TopView_controller dtv_Add_elements(ControllerM_Project? selectedProject)
        {
            TV_TopView_controller TC_proj = new()
            {
                Title = "Selected Elements",
                IconObject = FontAwesome.WPF.FontAwesomeIcon.Slack
            };

            WrapPanel sPanel = new()
            {
                Orientation = Orientation.Horizontal
            };

            foreach (var item in selectedProject.MCObject.ModelObject.SelectedElements)
            {
                sPanel.Children.Add(dtv_ElementFormat(item.ModelObject.ElementName));
            }

            TC_proj.Add_Item(sPanel);

            return TC_proj;
        }

        /// <summary>
        /// Returns the activephases tree item
        /// </summary>
        /// <returns></returns>
        private TV_TopView_controller dtv_Add_activePhases()
        {
            TV_TopView_controller TC_proj = new()
            {
                Title = "Active phases",
                IconObject = FontAwesome.WPF.FontAwesomeIcon.Clipboard
            };

            return TC_proj;
        }

        /// <summary>
        /// Returns the singlepixelcases tree item
        /// </summary>
        /// <param name="selectedProject"></param>
        /// <returns></returns>
        private TV_TopView_controller dtv_Add_singlePixelCases(ControllerM_Project? selectedProject)
        {
            TV_TopView_controller TC_proj = new()
            {
                Title = "Single pixel cases",
                IconObject = FontAwesome.WPF.FontAwesomeIcon.SquareOutline
            };

            WrapPanel ToolPanel = new()
            {
                Orientation = Orientation.Horizontal,
                FlowDirection = FlowDirection.RightToLeft,
                Margin = new Thickness(3, 3, 3, 3)
            };

            AM_button plotAccess = new()
            {
                IconName = "AreaChart",
                Width = 20,
                Height = 20,
                GradientColor_2 = "White",
                ForegroundIcon = "DodgerBlue",
                GradientTransition = "SteelBlue",
                Margin = new Thickness(2, 2, 2, 2),
                CornerRadius = "2",
                ModelTag = new Model_Case()
            };
            plotAccess.ClickButton += OnMouseClick_CasePixel_View_Handle;
            ToolPanel.Children.Add(plotAccess);
            TC_proj.Add_Item(ToolPanel);

            foreach (var item in selectedProject.MCObject.ModelObject.Cases)
            {
                TC_proj.Add_Item(new TV_TopView(dtv_Add_CaseSingle(item.ModelObject)));
            }

            return TC_proj;
        }

        private void OnMouseClick_CasePixel_View_Handle(object? sender, EventArgs e)
        {
            if (sender is not AM_button) return;
            AM_button sRef = (AM_button)sender;

            if (sRef.ModelTag is not Model_Case) return;
            Model_Case mRef = (Model_Case)sRef.ModelTag;

            Controller_Global.MainControl?.Show_Case_PlotView(mRef);
        }

        private TV_TopView_controller dtv_Add_CaseSingle(Model_Case casey)
        {
            TV_TopView_controller TC_proj = new()
            {
                Title = "Case " + casey.ID,
                IconObject = FontAwesome.WPF.FontAwesomeIcon.EllipsisH
            };

            WrapPanel ToolPanel = new()
            {
                Orientation = Orientation.Horizontal,
                FlowDirection = FlowDirection.RightToLeft,
                Margin = new Thickness(3, 3, 3, 3)
            };

            AM_button plotAccess = new()
            {
                IconName = "AreaChart",
                Width = 20,
                Height = 20,
                GradientColor_2 = "White",
                ForegroundIcon = "DodgerBlue",
                GradientTransition = "SteelBlue",
                Margin = new Thickness(2, 2, 2, 2),
                CornerRadius = "2",
                ModelTag = casey
            };
            //plotAccess.ClickButton += OnMouseClick_Plot_Handle;
            ToolPanel.Children.Add(plotAccess);

            AM_button editAccess = new()
            {
                IconName = "Edit",
                Width = 20,
                Height = 20,
                GradientColor_2 = "White",
                ForegroundIcon = "DodgerBlue",
                GradientTransition = "SteelBlue",
                Margin = new Thickness(2, 2, 2, 2),
                CornerRadius = "2",
                ModelTag = casey
            };
            editAccess.ClickButton += OnMouseClick_Case_Edit_Handle;
            ToolPanel.Children.Add(editAccess);
            TC_proj.Add_Item(ToolPanel);

            // Add case composition
            TV_TopView_controller TC_Composition = new()
            {
                Title = "Element Composition",
                IconObject = FontAwesome.WPF.FontAwesomeIcon.PuzzlePiece
            };

            WrapPanel sPanel = new()
            {
                Orientation = Orientation.Horizontal
            };

            foreach (var item in casey.ElementComposition)
            {
                sPanel.Children.Add(dtv_ElementFormat(item.ModelObject.ElementName + " : " + item.ModelObject.Value));
            }
            TC_Composition.Add_Item(sPanel);

            TC_proj.Add_Item(new TV_TopView(TC_Composition));

            // Add Case Precipitationn kinetics
            TC_proj.Add_Item(new TV_TopView(dtv_Add_Precipitation_kinetics(casey)));


            return TC_proj;
        }

        private void OnMouseClick_Case_Edit_Handle(object? sender, EventArgs e)
        {
            if (sender is not AM_button) return;
            AM_button sRef = (AM_button)sender;

            if (sRef.ModelTag is not Model_Case) return;
            Model_Case mRef = (Model_Case)sRef.ModelTag;

            Controller_Global.MainControl?.Show_Case_EditWindow(mRef);
        }

        private TV_TopView_controller dtv_Add_Precipitation_kinetics(Model_Case casey)
        {
            TV_TopView_controller TC_Kinetics = new()
            {
                Title = "Precipitation kinetics",
                IconObject = FontAwesome.WPF.FontAwesomeIcon.SnowflakeOutline
            };

            // Heat treatments
            TV_TopView_controller TC_HT = new()
            {
                Title = "Heat treatments",
                IconObject = FontAwesome.WPF.FontAwesomeIcon.Thermometer
            };

            foreach (var item in casey.HeatTreatments)
            {
                TV_TopView_controller TC_HT_Item = new()
                {
                    Title = item.ModelObject.ID + " : " + item.ModelObject.Name,
                    IconObject = FontAwesome.WPF.FontAwesomeIcon.None
                };

                WrapPanel ToolPanel = new()
                {
                    Orientation = Orientation.Horizontal,
                    FlowDirection = FlowDirection.RightToLeft,
                    Margin = new Thickness(3, 3, 3, 3)
                };

                AM_button plotAccess = new()
                {
                    IconName = "AreaChart",
                    Width = 20,
                    Height = 20,
                    GradientColor_2 = "White",
                    ForegroundIcon = "DodgerBlue",
                    GradientTransition = "SteelBlue",
                    Margin = new Thickness(2, 2, 2, 2),
                    CornerRadius = "2",
                    ModelTag = item
                };
                plotAccess.ClickButton += OnMouseClick_HeatTreatment_View_Handle;
                ToolPanel.Children.Add(plotAccess);
                TC_HT_Item.Add_Item(ToolPanel);

                TC_HT.Add_Item(new TV_TopView(TC_HT_Item));
            }

            TC_Kinetics.Add_Item(new TV_TopView(TC_HT));

            // Precipitation phases
            TV_TopView_controller TC_PR = new()
            {
                Title = "Precipitation phases",
                IconObject = FontAwesome.WPF.FontAwesomeIcon.Circle
            };

            foreach (var item in casey.PrecipitationPhases)
            {
                TV_TopView_controller TC_PR_Item = new()
                {
                    Title = item.ModelObject.Name,
                    IconObject = FontAwesome.WPF.FontAwesomeIcon.None
                };

                TC_PR.Add_Item(new TV_TopView(TC_PR_Item));
            }

            TC_Kinetics.Add_Item(new TV_TopView(TC_PR));

            // Precipitation Domain
            TV_TopView_controller TC_DO = new()
            {
                Title = "Precipitation domain",
                IconObject = FontAwesome.WPF.FontAwesomeIcon.DotCircleOutline
            };

            foreach (var item in casey.PrecipitationDomains)
            {
                TV_TopView_controller TC_DO_Item = new()
                {
                    Title = item.ModelObject.Name,
                    IconObject = FontAwesome.WPF.FontAwesomeIcon.None
                };

                TC_DO.Add_Item(new TV_TopView(TC_DO_Item));
            }

            TC_Kinetics.Add_Item(new TV_TopView(TC_DO));

            return TC_Kinetics;
        }

        private void OnMouseClick_HeatTreatment_View_Handle(object? sender, EventArgs e)
        {
            if (sender is not AM_button) return;
            AM_button sRef = (AM_button)sender;

            if (sRef.ModelTag is not ModelController<Model_HeatTreatment>) return;
            ModelController<Model_HeatTreatment> mRef = (ModelController<Model_HeatTreatment>)sRef.ModelTag;

            Controller_Global.MainControl?.Show_HeatTreatment_PlotView(mRef.ModelObject);
        }

        private TV_TopView_controller dtv_Add_object()
        {
            TV_TopView_controller TC_proj = new()
            {
                Title = "Object",
                IconObject = FontAwesome.WPF.FontAwesomeIcon.Cube
            };

            return TC_proj;
        }

        private Border dtv_ElementFormat(string content)
        {
            Border Belement = new()
            {
                Background = new SolidColorBrush(System.Windows.Media.Colors.WhiteSmoke),
                BorderBrush = new SolidColorBrush(System.Windows.Media.Colors.Silver)
            };

            TextBlock tBlock = new()
            {
                Text = content,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(5, 5, 5, 5)
            };
            Belement.Child = tBlock;

            return Belement;

        }
        #endregion


    }
}
