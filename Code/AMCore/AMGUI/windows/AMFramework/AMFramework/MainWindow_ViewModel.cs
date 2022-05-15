﻿using System;
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
    internal class MainWindow_ViewModel:Interfaces.ViewModel_Interface
    {
        private List<Components.Scripting.Scripting_ViewModel> _openScripts = new List<Components.Scripting.Scripting_ViewModel>();
        public List<Components.Scripting.Scripting_ViewModel> OpenScripts { get { return _openScripts; } }



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

            _openScripts.Add(new Components.Scripting.Scripting_ViewModel());
            result.Content = _openScripts[^1].get_text_editor();
            result.Tag = _openScripts[^1];

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