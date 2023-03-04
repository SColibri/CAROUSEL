using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Editing;
using ICSharpCode.AvalonEdit.Folding;
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

namespace AMControls.Custom.Scripting
{
    /// <summary>
    /// Interaction logic for ScriptingControl.xaml
    /// </summary>
    public partial class ScriptingControl : UserControl
    {
        public ScriptingControl()
        {
            InitializeComponent();
            DataContext = this;

            var foldingVar = FoldingManager.Install(editor.TextArea);
            var foldingStrategy = new XmlFoldingStrategy();
            foldingStrategy.UpdateFoldings(foldingVar, editor.Document);

            editor.FontFamily = new FontFamily("Segoe UI");
            editor.FontSize = 14.0;

        }

        public string HighlightValue { get; set; } = "Lua";
    }
}
