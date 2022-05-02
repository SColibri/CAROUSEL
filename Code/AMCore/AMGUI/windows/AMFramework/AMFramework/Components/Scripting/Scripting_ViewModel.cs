using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using ScintillaNET;
using System.Windows.Forms.Integration;

namespace AMFramework.Components.Scripting
{
    /// <summary>
    /// Scripting viewmodel
    /// </summary>
    internal class Scripting_ViewModel : Interfaces.ViewModel_Interface
    {
        private bool _changesMade = false; 
        public bool ChangesMade { get { return _changesMade; } set { _changesMade = value; } } // returns true if changes have been made to the document

        private string _filename = "";
        public string Filename { get { return _filename; } set { _filename = value; } } // path to file

        private string _autocompleteSelection = "";
        public string autocompleteSelection { get { return _autocompleteSelection; } set { _autocompleteSelection = value; } } // current autocomplete selection

        private static List<string> _autocomplete = new List<string>();
        public static List<string> Autocomplete 
        { 
            get {
                if(_autocomplete.Count == 0) {load_autocomplete();}
                return _autocomplete; 
            } 
            set { _autocomplete = value; }
        } // autocomplete list

        private Scripting_editor _scriptingEditor;

        #region Methods
        /// <summary>
        /// Check if file needs to be saved.
        /// </summary>
        /// <param name="scintilla"></param>
        /// <returns>returns 0 if okay</returns>
        public bool before_closing(Scintilla scintilla)
        {
            bool Result = true;

            if (_changesMade)
            {
                System.Windows.Forms.DialogResult dialogResult = System.Windows.Forms.MessageBox.Show("Do you want " +
                    "to save the changes done to the current file before loading this one?",
                    "Load file", System.Windows.Forms.MessageBoxButtons.YesNoCancel);
                if (dialogResult == DialogResult.Yes)
                {
                    int saveResult = save(scintilla);
                    if (saveResult != 0)
                    {
                        Result = false;
                        System.Windows.Forms.MessageBox.Show("There was an error while saving your file, please try again",
                                "Error",
                                System.Windows.Forms.MessageBoxButtons.OK,
                                System.Windows.Forms.MessageBoxIcon.Warning);
                    }
                }
                else if (dialogResult == DialogResult.Cancel)
                {
                    Result = true;
                }
            }

            return Result;
        }

        /// <summary>
        /// Save current document
        /// </summary>
        /// <param name="scintilla"></param>
        /// <returns></returns>
        public int save(Scintilla scintilla)
        {
            int Result = 0;

            if (File.Exists(_filename) == false)
            {
                _filename = create_new_file();

                if (Directory.Exists(System.IO.Path.GetDirectoryName(_filename)))
                {
                    File.Create(_filename).Close();
                }
                else
                {
                    Result = 1;
                }
            }

            if (Result == 0)
            {
                try
                {
                    StreamWriter sw = new StreamWriter(_filename);
                    sw.Write(scintilla.Text);
                    sw.Close();
                    ChangesMade = false;
                }
                catch
                {
                    Result++;
                }
            }

            return Result;
        }

        /// <summary>
        /// load a document
        /// </summary>
        /// <param name="scintilla"></param>
        /// <param name="path"></param>
        public void load(Scintilla scintilla, string path)
        {
            if (before_closing(scintilla))
            {
                if (File.Exists(path) && path.Contains(".lua") == true)
                {
                    _filename = System.IO.Path.GetFileName(path);
                    scintilla.Text = File.ReadAllText(path);
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Looks like the file does not exist or it is not compatible. Extension needed '.lua' ");
                }
            }

        }

        /// <summary>
        /// create new lua file
        /// </summary>
        /// <returns></returns>
        public string create_new_file()
        {
            string Result = "";

            System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            saveFileDialog.Filter = "LUA file (*.lua)|*.lua";
            saveFileDialog.DefaultExt = "lua";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                Result = saveFileDialog.FileName;
            }

            return Result;
        }

        /// <summary>
        /// loads autocomple from folder Templates
        /// </summary>
        public static void load_autocomplete()
        {
            string templatesPath = "Components/Scripting/Templates";
            string[] filesInDir = Directory.GetFiles(templatesPath, "*.autocomplete");

            foreach (string pathy in filesInDir)
            {
                load_autocomplete(pathy);
            }
        }

        /// <summary>
        /// load autocomplete from path
        /// </summary>
        /// <param name="pathToAutocomplete"></param>
        public static void load_autocomplete(string pathToAutocomplete)
        {
            if (File.Exists(pathToAutocomplete))
            {
                StreamReader sr = new StreamReader(pathToAutocomplete);
                string result = sr.ReadToEnd();

                string[] ListyTemp = result.Replace("\r", "").Split("\n");

                foreach (string stringy in ListyTemp)
                {
                    _autocomplete.Add(stringy);
                }

                sr.Close();
            }
        }

        /// <summary>
        /// gets an editor for scripting
        /// </summary>
        /// <returns></returns>
        public Scripting_editor get_text_editor()
        {
            Scripting_editor Resut;

            if (_scriptingEditor == null)
            {
                _scriptingEditor = new Scripting_editor();
                _scriptingEditor.DataContext = this;
                Resut = _scriptingEditor;
            }
            else
            {
                Resut = _scriptingEditor;
            }
            
            return Resut;
        }



        #endregion

        #region Interface
        public bool save()
        {
            if(_scriptingEditor != null)
            {
                if(save(_scriptingEditor.Scripting) == 0) { return true; }
                return false;
            }
            else { return false; }
        }

        public bool close()
        {
            if (_scriptingEditor != null)
            {
                return before_closing(_scriptingEditor.Scripting);
            }
            else { return false; }
        }
        #endregion
    }
}
