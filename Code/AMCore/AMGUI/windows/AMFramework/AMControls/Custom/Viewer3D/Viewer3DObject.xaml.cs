using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HelixToolkit.Wpf;
using Microsoft.Win32;
using SharpDX;
using _3DTools;
using System.Security.Policy;
using SharpDX.Direct3D9;
using System.Data.SqlTypes;
using System.DirectoryServices.ActiveDirectory;

namespace AMControls.Custom.Viewer3D
{
    /// <summary>
    /// Interaction logic for Viewer3DObject.xaml
    /// </summary>
    public partial class Viewer3DObject : UserControl
    {
        public Viewer3DObject()
        {
            InitializeComponent();
        }

        private void LoadFile(string filename) 
        {
            if (File.Exists(filename))
            {
                var importer = new ModelImporter();
                importer.DefaultMaterial = Materials.Red;

                var model = importer.Load(filename, Application.Current.Dispatcher);
                var visualModel = new ModelVisual3D();
                visualModel.Content = model;
                MainViewport.Children.Add(visualModel);

                var light = new DirectionalLight();

                // Set the properties of the light
                light.Direction = new Vector3D(-1, -1, -1);
                light.Color = Colors.White;
                var visualModel2 = new ModelVisual3D();
                visualModel2.Content = light;
                MainViewport.Children.Add(visualModel2);

                MakeVoxels(filename);

            }
            else 
            {
                MessageBox.Show($"The file {filename} was not found!", "File not found", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            
        }

        private void MakeVoxels(string filename) 
        {
            // Load the STL file
            var importer = new StLReader();
            var model = importer.Read(filename);

            // Define the voxel grid
            var gridSize = new Vector3D(10, 10, 10);
            var voxelSize = new Vector3D(1, 1, 1);
            var voxelGrid = new bool[(int)gridSize.X, (int)gridSize.Y, (int)gridSize.Z];

            // Raycasting
            var origin = new Point3D(0, 0, 0);
            var direction = new Vector3D(1, 0, 0);
            var step = 0.1;
            
            for (var x = 0; x < gridSize.X; x++)
            {
                for (var y = 0; y < gridSize.Y; y++)
                {
                    for (var z = 0; z < gridSize.Z; z++)
                    {
                        var voxelPosition = new Point3D(x * voxelSize.X, y * voxelSize.Y, z * voxelSize.Z);
                        var ray = new Ray3D(origin, voxelPosition - origin);
                        //model.Children.con
                        //var intersection = model.IntersectsWith(ray);
                        //RayHitTestParameters rayparams = new RayHitTestParameters(voxelPosition, ray);

                        //if (intersection.HasValue)
                        //{
                        //    var distance = intersection.Value.DistanceTo(ray.Origin);
                        //    if (distance < voxelSize.Length / 2)
                        //    {
                        //        voxelGrid[x, y, z] = true;
                        //    }
                        //}
                    }
                }
            }
        }
    private void Border_MouseDown(object sender, MouseButtonEventArgs e)
    {
        OpenFileDialog dialog = new OpenFileDialog();
        if ((bool)dialog.ShowDialog())
        {
            string filename = dialog.FileName;
            LoadFile(filename);
        }
    }
    }
}
