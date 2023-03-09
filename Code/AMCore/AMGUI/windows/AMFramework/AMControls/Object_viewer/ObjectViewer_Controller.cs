using AMControls.Implementations.Commands;
using HelixToolkit.SharpDX.Core.Assimp;
using Microsoft.Win32;
using System.Windows.Input;

namespace AMControls.Object_viewer
{
    internal class ObjectViewer_Controller
    {

        #region LoadFile
        private string OpenFileFilter = $"{Importer.SupportedFormatsString}";
        private string ExportFileFilter = $"{HelixToolkit.SharpDX.Core.Assimp.Exporter.SupportedFormatsString}";

        private ICommand _loadFile;
        public ICommand LoadFile
        {
            get
            {
                if (_loadFile == null)
                {
                    _loadFile = new RelayCommand(
                        param => this.LoadFile_Action(),
                        param => this.LoadFile_Check()
                    );
                }
                return _loadFile;
            }
        }
        private void LoadFile_Action()
        {
            HelixToolkit.Wpf.ModelImporter loader = new();

            OpenFileDialog ofd = new();
            ofd.Filter = OpenFileFilter;

            if (ofd.ShowDialog() == false) return;
            var scene = loader.Load(ofd.FileName);


        }
        private bool LoadFile_Check()
        {
            return true;
        }
        #endregion

    }
}
