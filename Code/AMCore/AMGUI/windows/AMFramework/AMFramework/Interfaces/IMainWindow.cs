using AMFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFramework.Interfaces
{
    public interface IMainWindow
    {
        //---------------------------------------------------------------------------------
        // VIEW DISPLAY
        //---------------------------------------------------------------------------------
        public void Show_Project_PlotView(Model.Model_Projects modelObject);
        public void Show_Project_EditView(Model.Model_Projects modelObject);
        public void Show_Case_PlotView(Model.Model_Case modelObject);
        public void Show_Case_EditWindow(Model.Model_Case modelObject);
        public void Show_HeatTreatment_PlotView(Model.Model_HeatTreatment modelObject);
        public void Show_HeatTreatment_EditWindow(Model.Model_HeatTreatment modelObject);

        //---------------------------------------------------------------------------------
        // CORE COMMUNICATION
        //---------------------------------------------------------------------------------
        public ref IAMCore_Comm Get_socket();

        //---------------------------------------------------------------------------------
        // CONTROLLERS
        //---------------------------------------------------------------------------------
        public Controller.Controller_Plot get_plot_Controller();
        public Controller.Controller_DBS_Projects get_project_controller();

    }
}
