using ICSharpCode.AvalonEdit.Folding;
using System.Windows.Controls;
using System.Windows.Media;

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
