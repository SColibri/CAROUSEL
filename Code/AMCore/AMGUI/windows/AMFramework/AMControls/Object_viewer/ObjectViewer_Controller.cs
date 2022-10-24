using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelixToolkit.Wpf.SharpDX;
using HelixToolkit.Wpf.SharpDX.Animations;
using HelixToolkit.SharpDX.Core.Assimp;
using HelixToolkit.Wpf.SharpDX.Controls;
using HelixToolkit.Wpf.SharpDX.Model;
using HelixToolkit.Wpf.SharpDX.Model.Scene;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using SharpDX;
using Point3D = System.Windows.Media.Media3D.Point3D;
using AMControls.Implementations.Commands;

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
