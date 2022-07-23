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
    /// Use Scripting_ViewModel as datacontext or call the get_text_editor() method from the viewmodel.
    /// </summary>
    public partial class Scripting_editor : System.Windows.Controls.UserControl
    {
        private AMSystem.LUA_FileParser AMParser = new();
        public Scripting_editor()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            setupMain(Scripting);
        }

        public void loadFile(string filename) 
        {
            if (File.Exists(filename)) 
            {
                ((Scripting_ViewModel)DataContext).load(Scripting, filename);
            }
        }
        #region Initialization
        private void setupMain(Scintilla scintilla)
        {
            // Extracted from the Lua Scintilla lexer and SciTE .properties file
            var alphaChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ.:";
            var numericChars = "0123456789";
            var accentedChars = "ŠšŒœŸÿÀàÁáÂâÃãÄäÅåÆæÇçÈèÉéÊêËëÌìÍíÎîÏïÐðÑñÒòÓóÔôÕõÖØøÙùÚúÛûÜüÝýÞþßö";

            // Configuring the default style with properties
            // we have common to every lexer style saves time.
            scintilla.StyleResetDefault();
            scintilla.Styles[ScintillaNET.Style.Default].Font = "Consolas";
            scintilla.Styles[ScintillaNET.Style.Default].Size = 10;
            scintilla.Styles[ScintillaNET.Style.Default].BackColor = IntToColor(0x212121);
            scintilla.Styles[ScintillaNET.Style.Default].ForeColor = IntToColor(0xFFFFFF);
            scintilla.StyleClearAll();

            // Configure the Lua lexer styles
            scintilla.Styles[ScintillaNET.Style.Lua.Default].ForeColor = System.Drawing.Color.Silver;
            scintilla.Styles[ScintillaNET.Style.Lua.Comment].ForeColor = System.Drawing.Color.Green;
            scintilla.Styles[ScintillaNET.Style.Lua.CommentLine].ForeColor = System.Drawing.Color.Green;
            scintilla.Styles[ScintillaNET.Style.Lua.Number].ForeColor = System.Drawing.Color.Olive;
            scintilla.Styles[ScintillaNET.Style.Lua.Word].ForeColor = System.Drawing.Color.Blue;
            scintilla.Styles[ScintillaNET.Style.Lua.Word2].ForeColor = System.Drawing.Color.BlueViolet;
            scintilla.Styles[ScintillaNET.Style.Lua.Word3].ForeColor = System.Drawing.Color.LightPink;
            scintilla.Styles[ScintillaNET.Style.Lua.Word4].ForeColor = System.Drawing.Color.LightPink;
            scintilla.Styles[ScintillaNET.Style.Lua.Word5].ForeColor = System.Drawing.Color.LightPink;
            scintilla.Styles[ScintillaNET.Style.Lua.String].ForeColor = System.Drawing.Color.Red;
            scintilla.Styles[ScintillaNET.Style.Lua.Character].ForeColor = System.Drawing.Color.Red;
            scintilla.Styles[ScintillaNET.Style.Lua.LiteralString].ForeColor = System.Drawing.Color.Red;
            scintilla.Styles[ScintillaNET.Style.Lua.StringEol].BackColor = System.Drawing.Color.Pink;
            scintilla.Styles[ScintillaNET.Style.Lua.Operator].ForeColor = System.Drawing.Color.Purple;
            scintilla.Styles[ScintillaNET.Style.Lua.Preprocessor].ForeColor = System.Drawing.Color.Maroon;
            scintilla.Lexer = Lexer.Lua;
            scintilla.WordChars = alphaChars + numericChars + accentedChars;

            scintilla.Styles[ScintillaNET.Style.Lua.Identifier].ForeColor = IntToColor(0xD0DAE2);
            scintilla.Styles[ScintillaNET.Style.Lua.Comment].ForeColor = IntToColor(0xBD758B);
            scintilla.Styles[ScintillaNET.Style.Lua.CommentLine].ForeColor = IntToColor(0x40BF57);
            scintilla.Styles[ScintillaNET.Style.Lua.CommentDoc].ForeColor = IntToColor(0x2FAE35);
            scintilla.Styles[ScintillaNET.Style.Lua.Number].ForeColor = IntToColor(0xFFFF00);
            scintilla.Styles[ScintillaNET.Style.Lua.String].ForeColor = IntToColor(0xFFFF00);
            scintilla.Styles[ScintillaNET.Style.Lua.Character].ForeColor = IntToColor(0xE95454);
            scintilla.Styles[ScintillaNET.Style.Lua.Preprocessor].ForeColor = IntToColor(0x8AAFEE);
            scintilla.Styles[ScintillaNET.Style.Lua.Operator].ForeColor = IntToColor(0xE0E0E0);
            scintilla.Styles[ScintillaNET.Style.Lua.Word].ForeColor = IntToColor(0x48A8EE);
            scintilla.Styles[ScintillaNET.Style.Lua.Word2].ForeColor = IntToColor(0xF98906);

            scintilla.Styles[ScintillaNET.Style.LineNumber].BackColor = IntToColor(0x212121);
            scintilla.Styles[ScintillaNET.Style.LineNumber].ForeColor = IntToColor(0xFFFFFF);
            scintilla.Styles[ScintillaNET.Style.IndentGuide].ForeColor = IntToColor(0xFFFFFF);
            scintilla.Styles[ScintillaNET.Style.IndentGuide].BackColor = IntToColor(0x212121);
            scintilla.CaretForeColor = System.Drawing.Color.White;
            scintilla.SetFoldMarginColor(true, IntToColor(0x212121));
            scintilla.SetFoldMarginHighlightColor(true, IntToColor(0x212121));
            scintilla.SetSelectionForeColor(true, System.Drawing.Color.White);
            scintilla.SetSelectionBackColor(true, System.Drawing.Color.DimGray);
            
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
            scintilla.Margins[0].Type = MarginType.RightText;
            scintilla.Margins[0].Width = 25;
            scintilla.Margins[2].Type = MarginType.Symbol;
            scintilla.Margins[2].Mask = Marker.MaskFolders;
            scintilla.Margins[2].Sensitive = true;
            scintilla.Margins[2].Width = 20;

            scintilla.Margins[3].Width = 2;

            // Set colors for all folding markers
            for (int i = 25; i <= 31; i++)
            {
                scintilla.Markers[i].SetForeColor(IntToColor(0xFFFFFF));
                scintilla.Markers[i].SetBackColor(IntToColor(0x212121));
            }

            // Configure folding markers with respective symbols
            scintilla.Markers[Marker.Folder].Symbol = MarkerSymbol.BoxPlus;
            scintilla.Markers[Marker.FolderOpen].Symbol = MarkerSymbol.BoxMinus;
            scintilla.Markers[Marker.FolderEnd].Symbol = MarkerSymbol.BoxPlusConnected;
            scintilla.Markers[Marker.FolderMidTail].Symbol = MarkerSymbol.TCorner;
            scintilla.Markers[Marker.FolderOpenMid].Symbol = MarkerSymbol.BoxMinusConnected;
            scintilla.Markers[Marker.FolderSub].Symbol = MarkerSymbol.VLine;
            scintilla.Markers[Marker.FolderTail].Symbol = MarkerSymbol.LCorner;

            scintilla.Markers[Marker.Folder].SetBackColor(System.Drawing.Color.Black);
            scintilla.Markers[Marker.FolderOpen].SetBackColor(System.Drawing.Color.Black);
            scintilla.Markers[Marker.FolderEnd].SetBackColor(System.Drawing.Color.Black);
            scintilla.Markers[Marker.FolderMidTail].SetBackColor(System.Drawing.Color.White);
            scintilla.Markers[Marker.FolderOpenMid].SetBackColor(System.Drawing.Color.Black);
            scintilla.Markers[Marker.FolderSub].SetBackColor(System.Drawing.Color.White);
            scintilla.Markers[Marker.FolderTail].SetBackColor(System.Drawing.Color.White);

            scintilla.Markers[Marker.Folder].SetForeColor(System.Drawing.Color.LightGray);
            scintilla.Markers[Marker.FolderOpen].SetForeColor(System.Drawing.Color.LightGray);
            scintilla.Markers[Marker.FolderEnd].SetForeColor(System.Drawing.Color.LightGray);
            scintilla.Markers[Marker.FolderOpenMid].SetForeColor(System.Drawing.Color.LightGray);

            scintilla.Markers[2].Symbol = MarkerSymbol.Circle;
            scintilla.Markers[2].SetBackColor(IntToColor(0x212121));
            scintilla.Markers[2].SetForeColor(IntToColor(0xFFFFFF));
            scintilla.Markers[2].SetAlpha(100);

            // Enable automatic folding
            scintilla.AutomaticFold = (AutomaticFold.Show | AutomaticFold.Click | AutomaticFold.Change);

            //Enable Drag drop
            scintilla.AllowDrop = true;
            scintilla.DragEnter += ScintillaDragEnter_handle;
            scintilla.DragDrop += ScintillaDragDrop_handle;
            scintilla.CharAdded += AutoCompleter;
            scintilla.MouseMove += MouseMove;
            UpdateLineNumbers(0);
        }


        public static System.Drawing.Color IntToColor(int rgb)
        {
            return System.Drawing.Color.FromArgb(255, (byte)(rgb >> 16), (byte)(rgb >> 8), (byte)rgb);
        }

        private void ScintillaDragEnter_handle(object sender, System.Windows.Forms.DragEventArgs e)
        {
            if (e.Data.GetDataPresent(System.Windows.Forms.DataFormats.FileDrop))
                e.Effect = System.Windows.Forms.DragDropEffects.Copy;
            else
                e.Effect = System.Windows.Forms.DragDropEffects.None;
        }

        private void ScintillaDragDrop_handle(object sender, System.Windows.Forms.DragEventArgs e)
        {

            // get file drop
            if (e.Data.GetDataPresent(System.Windows.Forms.DataFormats.FileDrop))
            {

                Array a = (Array)e.Data.GetData(System.Windows.Forms.DataFormats.FileDrop);
                if (a != null)
                {

                    string path = a.GetValue(0).ToString();

                    ((Scripting_ViewModel)DataContext).load(Scripting, path);

                }
            }
        }


        #endregion

        #region Handles
        private void Scripting_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.Space) { e.SuppressKeyPress = true; e.Handled = true; }
            else if(e.Modifiers == Keys.Control && e.KeyCode == Keys.S) 
            { 
                e.SuppressKeyPress = true; 
                e.Handled = true;
                ((Components.Scripting.Scripting_ViewModel)DataContext).save();
            }
            else if (e.KeyCode == Keys.Space) { return; }
            else if (e.KeyCode == Keys.Enter) 
            {
                AMSystem.LUA_FileParser.Remove_module("Local", AMParser);
                AMSystem.LUA_FileParser.File_parse(((Scintilla)sender).Text, AMParser, "Local");

                ((Scintilla)sender).SetKeywords(4, AMParser.Get_Classes_keywords());
                ((Scintilla)sender).SetKeywords(5, AMParser.Get_Functions_keywords());

                return;
            }
            /*
            var scintilla = sender as Scintilla;

            var pos = scintilla.CurrentPosition;

            var word = scintilla.GetWordFromPosition(pos);

            if (word == string.Empty) return;

            var list = Scripting_ViewModel.Autocomplete.FindAll(delegate (string item)
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
                scintilla.CallTipShow(1, "HelloWoel");
                //scintilla.AutoCShow(word.Length, TextList);
            }
            else
            {
                scintilla.AutoCCancel();
            }
            */
            ((Scripting_ViewModel)DataContext).ChangesMade = true;
        }


        private void Scripting_AutoCCompleted(object sender, AutoCSelectionEventArgs e)
        {
            if (((Scripting_ViewModel)DataContext).autocompleteSelection.Length > 0)
            {
                Scintilla scintilla = (Scintilla)sender;
                //scintilla.AppendText("fortext");
                ((Scripting_ViewModel)DataContext).autocompleteSelection = "";
                scintilla.CurrentPosition += 7;
            }
        }

        private List<string> AutoCompleteList = new();
        public void AutoCompleter(object sender, ScintillaNET.CharAddedEventArgs e)
        {
            var scintilla = sender as Scintilla;
            var pos = scintilla.CurrentPosition;
            var word = scintilla.GetWordFromPosition(pos);

            AutoCompleteList.Clear();
            if (word == string.Empty) return;
            if (word.Contains("."))
            {
                int IndexPoint = word.IndexOf('.');
                List<AMSystem.ParseObject> words = AMParser.AMParser.FindAll(e => e.Name.CompareTo(word.Substring(0, IndexPoint)) == 0).ToList();
                foreach (var item in words)
                {
                    AutoCompleteList.AddRange(item.Parameters);
                }
                for (int i = 0; i < AutoCompleteList.Count; i++)
                {
                    AutoCompleteList[i] = word.Substring(0, IndexPoint) + "." + AutoCompleteList[i];
                }

            }
            else if (word.Contains(":"))
            {
                int IndexPoint = word.IndexOf(':');
                List<AMSystem.ParseObject> words = AMParser.AMParser.FindAll(e => e.Name.CompareTo(word.Substring(0, IndexPoint)) == 0).ToList();
                foreach (var item in words)
                {
                    AutoCompleteList.AddRange(item.functions.Select(e => e.Name).ToList());
                }
                for (int i = 0; i < AutoCompleteList.Count; i++)
                {
                    AutoCompleteList[i] = word.Substring(0, IndexPoint) + ":" + AutoCompleteList[i];
                }
            }
            else
            {
                foreach (var item in AMParser.AMParser)
                {
                    AutoCompleteList.Add(item.Name);
                }
            }

            AutoCompleteList.Sort();

            var list = AutoCompleteList.FindAll(delegate (string item)
            {
                return item.StartsWith(word);
            });

            string TextList = "";
            foreach (var item in AutoCompleteList)
            {
                if (TextList.Length != 0) { TextList += scintilla.AutoCSeparator.ToString(); }
                TextList += item;
            }

            if (AutoCompleteList.Count > 0)
            {
                scintilla.AutoCShow(word.Length, TextList);
            }
            else
            {
                scintilla.AutoCCancel();
            }
        }

        public void MouseMove(object sender, System.Windows.Forms.MouseEventArgs e) 
        {
            System.Drawing.Point point = System.Windows.Forms.Control.MousePosition; 
            var cor = ((Scintilla)sender).PointToClient(point); 
            int pos = ((Scintilla)sender).CharPositionFromPoint(cor.X, cor.Y);

            if (pos == -1) return;
            var word = ((Scintilla)sender).GetWordFromPosition(pos);

            string infoData = "";
            if (word.Contains(".")) { }
            else if (word.Contains(":")) { }
            else 
            {
                AMSystem.ParseObject referenceP = AMParser.AMParser.Find(e => e.Name.CompareTo(word) == 0);
                if (referenceP == null) return;
                infoData = referenceP.Description;
            }

            if (infoData.Length == 0) return;
            ((Scintilla)sender).CallTipShow(pos, infoData);

        }

        private int maxLineNumberCharLength;

        private string calltipText = "HelloWorld";
        
        private void scintilla_TextChanged(object sender, EventArgs e)
        {
            

            // Did the number of characters in the line number display change?
            // i.e. nnn VS nn, or nnnn VS nn, etc...
            var maxLineNumberCharLength = Scripting.Lines.Count.ToString().Length;
            if (maxLineNumberCharLength == this.maxLineNumberCharLength)
                return;

            // Calculate the width required to display the last line number
            // and include some padding for good measure.
            const int padding = 2;
            Scripting.Margins[0].Width = Scripting.TextWidth(ScintillaNET.Style.LineNumber, new string('9', maxLineNumberCharLength + 1)) + padding;
            this.maxLineNumberCharLength = maxLineNumberCharLength;

        }

        private void UpdateLineNumbers(int startingAtLine)
        {
            // Starting at the specified line index, update each
            // subsequent line margin text with a hex line number.
            for (int i = startingAtLine; i < Scripting.Lines.Count; i++)
            {
                Scripting.Lines[i].MarginStyle = ScintillaNET.Style.LineNumber;
                Scripting.Lines[i].MarginText = i.ToString();
            }
        }

        private void scintilla_Insert(object sender, ModificationEventArgs e)
        {
            // Only update line numbers if the number of lines changed
            if (e.LinesAdded != 0)
                UpdateLineNumbers(Scripting.LineFromPosition(e.Position));
        }

        private void scintilla_Delete(object sender, ModificationEventArgs e)
        {
            // Only update line numbers if the number of lines changed
            if (e.LinesAdded != 0)
                UpdateLineNumbers(Scripting.LineFromPosition(e.Position));
        }
        #endregion

        #region events

        #endregion

        #region Loaders
        
        #endregion

    }
}
