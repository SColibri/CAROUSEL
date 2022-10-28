using AMFramework_Lib.Core;
using AMFramework_Lib.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFramework_Lib.Interfaces
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
        public void Show_Popup(object pWindow);
        public void Show_loading(bool showLoading);
        public void Show_Notification(string Title, string Content, int IconType,
                                      Struct_Color? IconForeground, Struct_Color? ContentBackground, Struct_Color? TitleBackground);

        //---------------------------------------------------------------------------------
        // OUTPUT
        //---------------------------------------------------------------------------------
        public void Set_Core_Output(string outputString);

        //---------------------------------------------------------------------------------
        // CORE COMMUNICATION
        //---------------------------------------------------------------------------------
        public ref IAMCore_Comm Get_socket();


        //---------------------------------------------------------------------------------
        // Parameters
        //---------------------------------------------------------------------------------

        // Additional information: Text that offers additional information on current control or action
        public bool AdditionalInformationIsExpanded { get; set; }
        public string ContentAdditionalInformation { get; set; }
        public string TitleAdditionalInformation { get; set; }


    }
}
