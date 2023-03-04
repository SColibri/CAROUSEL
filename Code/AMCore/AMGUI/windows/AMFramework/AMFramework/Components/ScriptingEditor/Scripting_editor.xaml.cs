using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using ScintillaNET;
using AMFramework_Lib.AMSystem;
using AMFramework_Lib.Controller;
using System.Windows.Controls;

namespace AMFramework.Components.ScriptingEditor
{
    /// <summary>
    /// Interaction logic for Scripting_editor.xaml
    /// Use Scripting_ViewModel as datacontext or call the get_text_editor() method from the viewmodel.
    /// </summary>
    public partial class Scripting_editor : UserControl
    {
        private LUA_FileParser AMParser = new();
        public Scripting_editor()
        {
            InitializeComponent();
            
            string filename = "Components/Scripting/Templates/NewScript.AMFramework";
            if (!System.IO.File.Exists(filename)) return;
            Scripting.Text = System.IO.File.ReadAllText(filename);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            setupMain(Scripting);
        }

        public void loadFile(string filename) 
        {
            if (File.Exists(filename)) 
            {
                ((Scripting_ViewModel)DataContext).load(Scripting as Scintilla, filename);
                Scripting.FoldAll(FoldAction.Contract);
                Update_Highlight(Scripting as Scintilla);
            }
        }
        #region Initialization
        private void setupMain(Scintilla scintilla)
        {
            scintilla.Technology = Technology.DirectWrite;
            // Extracted from the Lua Scintilla lexer and SciTE .properties file
            var alphaChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ_";
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
            scintilla.Styles[ScintillaNET.Style.Lua.Word6].ForeColor = System.Drawing.Color.DodgerBlue;
            scintilla.Styles[ScintillaNET.Style.Lua.Word6].Bold = true;
            scintilla.Styles[ScintillaNET.Style.Lua.Word7].ForeColor = System.Drawing.Color.DodgerBlue;
            scintilla.Styles[ScintillaNET.Style.Lua.Word7].Bold = true;
            scintilla.Styles[ScintillaNET.Style.Lua.Word8].ForeColor = System.Drawing.Color.LightGreen;
            scintilla.Styles[ScintillaNET.Style.Lua.Word8].Bold = true;
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
            scintilla.SetFoldFlags(FoldFlags.LineAfterContracted);

            // Images
            scintilla.RegisterRgbaImage(0, AMsystem.AMFramework_ImageSource.Get_faIcon_bitmap("variable"));
            scintilla.RegisterRgbaImage(1, AMsystem.AMFramework_ImageSource.Get_faIcon_bitmap("math-function"));
            scintilla.RegisterRgbaImage(2, AMsystem.AMFramework_ImageSource.Get_faIcon_bitmap("tool"));
            scintilla.RegisterRgbaImage(3, AMsystem.AMFramework_ImageSource.Get_faIcon_bitmap("box"));
            scintilla.AutoCIgnoreCase = false;

            //Enable Drag drop
            scintilla.AllowDrop = true;
            scintilla.DragEnter += ScintillaDragEnter_handle;
            scintilla.DragDrop += ScintillaDragDrop_handle;
            scintilla.CharAdded += AutoCompleter;
            scintilla.MouseMove += MouseMove_Scintilla;
            UpdateLineNumbers(0);
        }

        public static System.Drawing.Color IntToColor(int rgb)
        {
            return System.Drawing.Color.FromArgb(255, (byte)(rgb >> 16), (byte)(rgb >> 8), (byte)rgb);
        }

        private void ScintillaDragEnter_handle(object? sender, System.Windows.Forms.DragEventArgs e)
        {
            if (e.Data == null) return;
            if (e.Data.GetDataPresent(System.Windows.Forms.DataFormats.FileDrop))
                e.Effect = System.Windows.Forms.DragDropEffects.Copy;
            else
                e.Effect = System.Windows.Forms.DragDropEffects.None;
        }

        private void ScintillaDragDrop_handle(object? sender, System.Windows.Forms.DragEventArgs e)
        {
            if (e.Data == null) return;
            // get file drop
            if (e.Data.GetDataPresent(System.Windows.Forms.DataFormats.FileDrop))
            {

                Array a = (Array)e.Data.GetData(System.Windows.Forms.DataFormats.FileDrop);
                if (a != null)
                {

                    string path = a.GetValue(0).ToString();

                    if (path == null) return;
                    ((Scripting_ViewModel)DataContext).load(Scripting as Scintilla, path);
                }
            }
        }


        #endregion

        #region Handles
        private void Folding_Styles(object sender)
        {
            var scintilla = ((Scintilla)sender);

            foreach (var item in scintilla.Lines)
            {
                item.FoldLevelFlags = 0;
                item.FoldLevel = 1024;
            }

            var startList = scintilla.Lines.ToList().FindAll(e => e.Text.ToLower().Contains("-- region"));
            var endList = scintilla.Lines.ToList().FindAll(e => e.Text.ToLower().Contains("-- endregion"));

            for (int i = 0; i < startList.Count; i++)
            {
                if (endList.Count <= i) continue;
                AddFoldRegion(scintilla, startList[i].Index, endList[i].Index, 1024);
                //tartList[i].Expanded = false;
                //startList[i].FoldLine(FoldAction.Contract);
            }
        }
        private void FoldGroups_loadedText(Scintilla sender) 
        {
            foreach (var item in sender.Lines)
            {
                item.FoldLine(FoldAction.Contract);
            }
        }

        #region Code from source

        /// <summary>
        ///  Add-remove functions copy from source https://github.com/jacobslusser/ScintillaNET/issues/328, Author Deepchand-Calm
        ///  This fraction of the code is an implementation for the code folding using scintillanet. Note. I just modifed de signature, for
        ///  more info, refer to the link. (yeah, copy and paste is not the best practice)
        /// </summary>
        /// <param name="TextEditor"></param>
        /// <param name="StartLine"></param>
        /// <param name="EndLine"></param>
        /// <param name="CurrentLevel"></param>
        private void AddFoldRegion(Scintilla TextEditor, int StartLine, int EndLine, int CurrentLevel)
        {
            int Start = StartLine, End = EndLine;

            if (StartLine > EndLine)
            {
                Start = EndLine;
                End = StartLine;
            }
            TextEditor.Lines[Start].FoldLevelFlags = FoldLevelFlags.Header;
            TextEditor.Lines[Start].FoldLevel = CurrentLevel;
            for (int i = Start + 1; i < End; ++i)
            {
                TextEditor.Lines[i].FoldLevel = TextEditor.Lines[Start].FoldLevel + 1;
                TextEditor.Lines[i].FoldLevelFlags = FoldLevelFlags.White;
            }
            TextEditor.Lines[End].FoldLevel = TextEditor.Lines[Start].FoldLevel + 1;
        }
        private void RemoveFoldRegion(Scintilla TextEditor, int StartLine, int EndLine, int CurrentLevel)
        {
            int Start = StartLine, End = EndLine;

            if (StartLine > EndLine)
            {
                Start = EndLine;
                End = StartLine;
            }
            TextEditor.Lines[Start].FoldLevelFlags = 0;
            TextEditor.Lines[Start].FoldLevel = CurrentLevel;
            for (int i = Start + 1; i < End; ++i)
            {
                TextEditor.Lines[i].FoldLevel = TextEditor.Lines[Start].FoldLevel;
                TextEditor.Lines[i].FoldLevelFlags = 0;
            }
            TextEditor.Lines[End].FoldLevel = TextEditor.Lines[Start].FoldLevel;
        }
        #endregion
        private void Scripting_KeyDown(object? sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (sender == null) return;

            //Folding_Styles(sender);
            if (e.Modifiers == System.Windows.Forms.Keys.Control && 
                e.KeyCode == System.Windows.Forms.Keys.Space) 
            {
                var pos = ((Scintilla)sender).CurrentPosition;
                var word = ((Scintilla)sender).GetWordFromPosition(pos);
                Show_autocomplete(word, (Scintilla)sender);
                e.SuppressKeyPress = true; e.Handled = true; 
            }
            else if (e.Modifiers == System.Windows.Forms.Keys.Control && 
                e.KeyCode == System.Windows.Forms.Keys.S)
            {
                e.SuppressKeyPress = true;
                e.Handled = true;
                ((Components.ScriptingEditor.Scripting_ViewModel)DataContext).Save();
            }
            else if (e.Modifiers == System.Windows.Forms.Keys.Control && 
                     e.KeyCode == System.Windows.Forms.Keys.O)
            {
                e.SuppressKeyPress = true;
                e.Handled = true;
                Scripting.FoldAll(FoldAction.Toggle);
            }
            else if (e.KeyCode == System.Windows.Forms.Keys.Back) 
            {
                string CurrentLineT = ((Scintilla)sender).Lines[((Scintilla)sender).CurrentLine].Text;

                if (CurrentLineT.Length == 0) return;
                if (CurrentLineT[CurrentLineT.Length - 1] == '.' || 
                    CurrentLineT[CurrentLineT.Length - 1] == ':') 
                { 
                    if (Anchor_StringBuild.Count > 0) 
                    {
                        Anchor_StringBuild.RemoveAt(Anchor_StringBuild.Count - 1);
                    }

                    if (CurrentLineT.LastIndexOf('.') != -1)
                    {
                        string VariablesNames = AMParser.Get_Global_variable_parameters(Anchor_StringBuild);
                        AutoCompleteList.AddRange(VariablesNames.Split(" ").ToList());
                        Anchor_Parameters = true;
                    }
                    else 
                    {
                        string FunctionNames = AMParser.Get_Global_variable_functions(Anchor_StringBuild);
                        AutoCompleteList.AddRange(FunctionNames.Split(" ").ToList());
                        Anchor_Functions = true;
                    }
                }
                else if(CurrentLineT.Contains('.') == false && CurrentLineT.Contains(':') == false) 
                {
                    Anchor_Functions = false;
                    Anchor_Parameters = false;
                    Anchor_StringBuild.Clear();
                }
            }
            else if (e.Modifiers == System.Windows.Forms.Keys.Control && e.KeyCode == System.Windows.Forms.Keys.Space) 
            {
                var pos = ((Scintilla)sender).CurrentPosition;
                var word = ((Scintilla)sender).GetWordFromPosition(pos);
                Show_autocomplete(word, (Scintilla)sender);
                e.Handled = true;
            }
            else if (e.KeyCode == System.Windows.Forms.Keys.Space) { return; }
            else if (e.KeyCode == System.Windows.Forms.Keys.Enter)
            {
                Update_Highlight(sender);
                return;
            }
            
            ((Scripting_ViewModel)DataContext).ChangesMade = true;
        }

        private void Update_Highlight(object sender) 
        {
            LUA_FileParser.Remove_module("Local", AMParser);
            LUA_FileParser.File_parse(((Scintilla)sender).Text, AMParser, "Local");

            ((Scintilla)sender).SetKeywords(4, AMParser.Remove_Icon_tags(AMParser.Get_Classes_keywords().Replace("?3", "")));
            ((Scintilla)sender).SetKeywords(5, AMParser.Remove_Icon_tags(AMParser.Get_Functions_keywords().Replace("?1", "")));
            ((Scintilla)sender).SetKeywords(6, AMParser.Remove_Icon_tags(AMParser.Get_Global_variable_keywords().Replace("?0", "")));
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
        private bool Anchor_Parameters = false;
        private bool Anchor_Functions = false;
        private int Anchor_line = -1;
        private List<string> Anchor_StringBuild = new();
        public void AutoCompleter(object sender, ScintillaNET.CharAddedEventArgs e)
        {
            var scintilla = sender as Scintilla;
            var pos = scintilla.CurrentPosition;
            var word = scintilla.GetWordFromPosition(pos);

            // Check if there is something to complete or not
            if (word.Length == 0 && (e.Char != 46 && e.Char != 58)) return;

            if (Anchor_line != scintilla.CurrentLine) 
            {
                Anchor_Parameters = false;
                Anchor_Functions = false;
                Anchor_StringBuild.Clear();
                Anchor_line = scintilla.CurrentLine;
            }

            if (e.Char == '.') 
            {
                AutoCompleteList.Clear();
                word = scintilla.GetWordFromPosition(pos-1);
                if (Anchor_StringBuild.Contains(word) == false) Anchor_StringBuild.Add(word);

                string VariablesNames = AMParser.Get_Global_variable_parameters(Anchor_StringBuild);
                AutoCompleteList.AddRange(VariablesNames.Split(" ").ToList());
                Anchor_Parameters = true;
            }
            else if (e.Char == ':')
            {
                AutoCompleteList.Clear();
                word = scintilla.GetWordFromPosition(pos - 1);
                if (Anchor_StringBuild.Contains(word) == false) Anchor_StringBuild.Add(word);

                string FunctionNames = AMParser.Get_Global_variable_functions(Anchor_StringBuild);
                AutoCompleteList.AddRange(FunctionNames.Split(" ").ToList());
                Anchor_Functions = true;
            }
            else if (Anchor_Functions == true || Anchor_Parameters == true) 
            { 
            
            }
            else
            {
                AutoCompleteList.Clear();
                foreach (var item in AMParser.Get_Classes_keywords().Split(" ").ToList())
                {
                    if(AutoCompleteList.Contains(item) == false) AutoCompleteList.Add(item);
                }

                foreach (var item in AMParser.Get_Functions_keywords().Split(" ").ToList())
                {
                    if (AutoCompleteList.Contains(item) == false) AutoCompleteList.Add(item);
                }

                foreach (var item in AMParser.Get_Global_variable_keywords().Split(" ").ToList())
                {
                    if (AutoCompleteList.Contains(item) == false) AutoCompleteList.Add(item);
                }
            }

            Show_autocomplete(word, scintilla);
        }

        public void MouseMove_Scintilla(object? sender, System.Windows.Forms.MouseEventArgs e) 
        {
            if (sender == null) return;
            System.Drawing.Point point = System.Windows.Forms.Control.MousePosition; 
            var cor = ((Scintilla)sender).PointToClient(point); 
            int pos = ((Scintilla)sender).CharPositionFromPoint(cor.X, cor.Y);

            if (pos == -1) return;
            var cLine = ((Scintilla)sender).LineFromPosition(pos);
            var fullText = ((Scintilla)sender).Lines[cLine].Text;
            var word = ((Scintilla)sender).GetWordFromPosition(pos);

            if (word.Length == 0) return;
            string titleData = "";
            string infoData = "";
            ParseObject? referenceP = AMParser.AMParser.Find(e => e.Name.CompareTo(word) == 0);
            if (referenceP == null) 
            {
                int posIterate = pos - 2;
                ParseObject? mainClass = null;
                while (((Scintilla)sender).LineFromPosition(posIterate) == cLine && 
                                                                 posIterate >= 0 &&
                                                                 mainClass == null) 
                {
                    var tWord = ((Scintilla)sender).GetWordFromPosition(posIterate);
                    mainClass = AMParser.AMParser.Find(e => e.Name.CompareTo(tWord) == 0);
                    posIterate -= (tWord.Length + 1);
                }

                if (mainClass == null) return;



                referenceP = mainClass;
            }

            if (referenceP.ObjectType == ParseObject.PTYPE.CLASS || 
                referenceP.ObjectType == ParseObject.PTYPE.GLOBAL_VARIABLE ||
                referenceP.ObjectType == ParseObject.PTYPE.LOCAL_VARIABLE)
            {
                infoData = referenceP.Description;
                titleData = referenceP.ObjectType.ToString() + " " + referenceP.Name;
            }
            else if (referenceP.ObjectType == ParseObject.PTYPE.FUNCTION) 
            {
                ParseObject? mainClass = AMParser.AMParser.Find(e => e.functions.FindAll(e  => e.Name.CompareTo(referenceP.ParametersType) == 0).Count > 0 );

                if (mainClass == null) return;
            }


            if (infoData.Length == 0) return;

            if(Controller_Global.MainControl != null) 
            {
                Controller_Global.MainControl.TitleAdditionalInformation = titleData;
                Controller_Global.MainControl.ContentAdditionalInformation = infoData;
            }


            // ((Scintilla)sender).CallTipShow(pos, infoData);
            // Call tip is not as useful, this becomes annoying instead of helpful
        }

        private int maxLineNumberCharLength;
        
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

        #region Methods
        private void Show_autocomplete(string word, Scintilla scintilla) 
        {
            AutoCompleteList.Sort();

            var list = AutoCompleteList.FindAll(delegate (string item)
            {
                return item.StartsWith(word);
            });

            string TextList = "";
            foreach (var item in list)
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

        private string Get_previousWord(int position, Scintilla sender) 
        {
            int startPosition = sender.WordStartPosition(position - 1, true);
            int endPosition = sender.WordEndPosition(position - 1, true);

            return sender.GetTextRange(startPosition, endPosition - startPosition); ;
        }

        private string Get_currentWord(int position, Scintilla sender)
        {
            int startPosition = sender.WordStartPosition(position, true);
            int endPosition = sender.WordEndPosition(position, true);

            return sender.GetTextRange(startPosition, endPosition - startPosition); ;
        }
        #endregion

    }
}
