using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ScintillaNET;

namespace AMFramework.Components.Scripting
{
    /// <summary>
    /// Interaction logic for Scripting_editor.xaml
    /// </summary>
    public partial class Scripting_editor : System.Windows.Controls.UserControl
    {
        private string _autocompleteSelection = "";
        private List<string> _autocomplete = new List<string>()
        {
            "Empty",
            "for",
            "while",
            "repeat_until",
            "break",
            "Hello.World"
        };

        private readonly
        Dictionary<string, string> dict = new Dictionary<string, string>{ {"if", "end"},
        {"for", "next"},{"while", "do"},{"do", "end"}, {"else", "end"}, {"elseif __ then", "end"}};

        public Scripting_editor()
        {
            _autocomplete.Sort();
            InitializeComponent();
        }


        private void Scripting_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.Space) { e.SuppressKeyPress = true;  e.Handled = true; }
            else if(e.KeyCode == Keys.Space) { return; }

            var scintilla = sender as Scintilla;

            var pos = scintilla.CurrentPosition;

            var word = scintilla.GetWordFromPosition(pos);

            if (word == string.Empty) return;

            var list = _autocomplete.FindAll(delegate (string item)
            {
                return item.StartsWith(word);
            });

            string TextList = "";
            foreach (var item in list)
            {
                if (TextList.Length != 0) { TextList += scintilla.AutoCSeparator.ToString(); }
                TextList += item;
            }

            if (list.Count > 0)
            {
                scintilla.AutoCShow(word.Length, TextList);
            }
            else
            {
                scintilla.AutoCCancel();
            }
        }

        private void setupMain(Scintilla scintilla)
        {
            // Extracted from the Lua Scintilla lexer and SciTE .properties file
            var alphaChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var numericChars = "0123456789";
            var accentedChars = "ŠšŒœŸÿÀàÁáÂâÃãÄäÅåÆæÇçÈèÉéÊêËëÌìÍíÎîÏïÐðÑñÒòÓóÔôÕõÖØøÙùÚúÛûÜüÝýÞþßö";

            // Configuring the default style with properties
            // we have common to every lexer style saves time.
            scintilla.StyleResetDefault();
            scintilla.Styles[ScintillaNET.Style.Default].Font = "Consolas";
            scintilla.Styles[ScintillaNET.Style.Default].Size = 10;
            scintilla.StyleClearAll();

            // Configure the Lua lexer styles
            scintilla.Styles[ScintillaNET.Style.Lua.Default].ForeColor = System.Drawing.Color.Silver;
            scintilla.Styles[ScintillaNET.Style.Lua.Comment].ForeColor = System.Drawing.Color.Green;
            scintilla.Styles[ScintillaNET.Style.Lua.CommentLine].ForeColor = System.Drawing.Color.Green;
            scintilla.Styles[ScintillaNET.Style.Lua.Number].ForeColor = System.Drawing.Color.Olive;
            scintilla.Styles[ScintillaNET.Style.Lua.Word].ForeColor = System.Drawing.Color.Blue;
            scintilla.Styles[ScintillaNET.Style.Lua.Word2].ForeColor = System.Drawing.Color.BlueViolet;
            scintilla.Styles[ScintillaNET.Style.Lua.Word3].ForeColor = System.Drawing.Color.DarkSlateBlue;
            scintilla.Styles[ScintillaNET.Style.Lua.Word4].ForeColor = System.Drawing.Color.DarkSlateBlue;
            scintilla.Styles[ScintillaNET.Style.Lua.String].ForeColor = System.Drawing.Color.Red;
            scintilla.Styles[ScintillaNET.Style.Lua.Character].ForeColor = System.Drawing.Color.Red;
            scintilla.Styles[ScintillaNET.Style.Lua.LiteralString].ForeColor = System.Drawing.Color.Red;
            scintilla.Styles[ScintillaNET.Style.Lua.StringEol].BackColor = System.Drawing.Color.Pink;
            scintilla.Styles[ScintillaNET.Style.Lua.Operator].ForeColor = System.Drawing.Color.Purple;
            scintilla.Styles[ScintillaNET.Style.Lua.Preprocessor].ForeColor = System.Drawing.Color.Maroon;
            scintilla.Lexer = Lexer.Lua;
            scintilla.WordChars = alphaChars + numericChars + accentedChars;

            // Keywords
            scintilla.SetKeywords(0, "and break do else elseif end for function if in local nil not or repeat return then until while" + " false true" + " goto");
            // Basic Functions
            scintilla.SetKeywords(1, "assert collectgarbage dofile error _G getmetatable ipairs loadfile next pairs pcall print rawequal rawget rawset setmetatable tonumber tostring type _VERSION xpcall string table math coroutine io os debug" + " getfenv gcinfo load loadlib loadstring require select setfenv unpack _LOADED LUA_PATH _REQUIREDNAME package rawlen package bit32 utf8 _ENV");
            // String Manipulation & Mathematical
            scintilla.SetKeywords(2, "string.byte string.char string.dump string.find string.format string.gsub string.len string.lower string.rep string.sub string.upper table.concat table.insert table.remove table.sort math.abs math.acos math.asin math.atan math.atan2 math.ceil math.cos math.deg math.exp math.floor math.frexp math.ldexp math.log math.max math.min math.pi math.pow math.rad math.random math.randomseed math.sin math.sqrt math.tan" + " string.gfind string.gmatch string.match string.reverse string.pack string.packsize string.unpack table.foreach table.foreachi table.getn table.setn table.maxn table.pack table.unpack table.move math.cosh math.fmod math.huge math.log10 math.modf math.mod math.sinh math.tanh math.maxinteger math.mininteger math.tointeger math.type math.ult" + " bit32.arshift bit32.band bit32.bnot bit32.bor bit32.btest bit32.bxor bit32.extract bit32.replace bit32.lrotate bit32.lshift bit32.rrotate bit32.rshift" + " utf8.char utf8.charpattern utf8.codes utf8.codepoint utf8.len utf8.offset");
            // Input and Output Facilities and System Facilities
            scintilla.SetKeywords(3, "coroutine.create coroutine.resume coroutine.status coroutine.wrap coroutine.yield io.close io.flush io.input io.lines io.open io.output io.read io.tmpfile io.type io.write io.stdin io.stdout io.stderr os.clock os.date os.difftime os.execute os.exit os.getenv os.remove os.rename os.setlocale os.time os.tmpname" + " coroutine.isyieldable coroutine.running io.popen module package.loaders package.seeall package.config package.searchers package.searchpath" + " require package.cpath package.loaded package.loadlib package.path package.preload");

            // Instruct the lexer to calculate folding
            scintilla.SetProperty("fold", "1");
            scintilla.SetProperty("fold.compact", "1");

            // Configure a margin to display folding symbols
            scintilla.Margins[2].Type = MarginType.Symbol;
            scintilla.Margins[2].Mask = Marker.MaskFolders;
            scintilla.Margins[2].Sensitive = true;
            scintilla.Margins[2].Width = 20;

            // Set colors for all folding markers
            for (int i = 25; i <= 31; i++)
            {
                scintilla.Markers[i].SetForeColor(System.Drawing.SystemColors.ControlLightLight);
                scintilla.Markers[i].SetBackColor(System.Drawing.SystemColors.ControlDark);
            }

            // Configure folding markers with respective symbols
            scintilla.Markers[Marker.Folder].Symbol = MarkerSymbol.BoxPlus;
            scintilla.Markers[Marker.FolderOpen].Symbol = MarkerSymbol.BoxMinus;
            scintilla.Markers[Marker.FolderEnd].Symbol = MarkerSymbol.BoxPlusConnected;
            scintilla.Markers[Marker.FolderMidTail].Symbol = MarkerSymbol.TCorner;
            scintilla.Markers[Marker.FolderOpenMid].Symbol = MarkerSymbol.BoxMinusConnected;
            scintilla.Markers[Marker.FolderSub].Symbol = MarkerSymbol.VLine;
            scintilla.Markers[Marker.FolderTail].Symbol = MarkerSymbol.LCorner;

            // Enable automatic folding
            scintilla.AutomaticFold = (AutomaticFold.Show | AutomaticFold.Click | AutomaticFold.Change);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            setupMain(Scripting);
        }

        private void Scripting_AutoCSelection(object sender, AutoCSelectionEventArgs e)
        {
            _autocompleteSelection = e.Text;
            int pos = e.Position;
            Scintilla scintilla = (Scintilla)sender;
            scintilla.AddText("fortext");
        }

        private void Scripting_TextChanged(object sender, EventArgs e)
        {
            if(_autocompleteSelection.Length > 0)
            {
                Scintilla scintilla = (Scintilla)sender;
                scintilla.AppendText("fortext");
                _autocompleteSelection = "";
            }
        }
    }
}
