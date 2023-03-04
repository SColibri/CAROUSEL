using AMFramework.Controller;
using AMFramework_Lib.Controller;
using AMFramework_Lib.Interfaces;
using ScintillaNET;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Windows.Input;

namespace AMFramework.Components.ScriptingEditor
{
    /// <summary>
    /// Scripting viewmodel
    /// </summary>
    public class Scripting_ViewModel : ViewModel_Interface
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public Scripting_ViewModel()
        {

            // Initialize scripting editor
            _scriptingEditor = new()
            {
                DataContext = this
            };

        }

        private bool _changesMade = false;
        /// <summary>
        /// Returns true if document has modifications
        /// </summary>
        public bool ChangesMade
        {
            get { return _changesMade; }
            set
            {
                _changesMade = value;
            }
        } // returns true if changes have been made to the document

        private string _filename = "";
        /// <summary>
        /// Filename of document
        /// </summary>
        public string Filename
        {
            get { return _filename; }
            set
            {
                _filename = value;
            }
        } // path to file

        private string _autocompleteSelection = "";
        public string autocompleteSelection { get { return _autocompleteSelection; } set { _autocompleteSelection = value; } } // current autocomplete selection

        private static List<string> _autocomplete = new();
        public static List<string> Autocomplete
        {
            get
            {
                if (_autocomplete.Count == 0) { load_autocomplete(); }
                return _autocomplete;
            }
            set { _autocomplete = value; }
        } // autocomplete list

        private Scripting_editor _scriptingEditor;
        /// <summary>
        /// Scintilla scripting editor
        /// </summary>
        public Scripting_editor ScriptingEditor => _scriptingEditor;

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
                    Result = false;
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
                    StreamWriter sw = new(_filename);
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
                if (File.Exists(path))
                {
                    //_filename = System.IO.Path.GetFileName(path);
                    scintilla.Text = File.ReadAllText(path);
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Looks like the file does not exist or it is not compatible. Extension needed '.lua' ");
                }
            }

        }

        /// <summary>
        /// Load file using filename path
        /// </summary>
        /// <param name="filename"></param>
        public void Load(string filename)
        {
            //First check if the open file has modifications
            if (before_closing(ScriptingEditor.Scripting))
            {
                if (File.Exists(filename))
                {
                    ScriptingEditor.Scripting.Text = File.ReadAllText(filename);
                    Filename =  filename;
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

            System.Windows.Forms.SaveFileDialog saveFileDialog = new()
            {
                Filter = "LUA file (*.lua)|*.lua",
                DefaultExt = "lua"
            };

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
                StreamReader sr = new(pathToAutocomplete);
                string result = sr.ReadToEnd();

                string[] ListyTemp = result.Replace("\r", "").Split("\n");

                foreach (string stringy in ListyTemp)
                {
                    _autocomplete.Add(stringy);
                }

                sr.Close();
            }
        }
        #endregion

        #region Commands
        #region RunScript

        private ICommand? _runcriptCommand;
        /// <summary>
        /// Command for opening a new script file
        /// close command
        /// </summary>
        public ICommand? RunScriptCommand
        {
            get
            {
                return _runcriptCommand ??= new RelayCommand(param => RunScriptCommand_Action(), param => RunScriptCommand_Check());
            }
        }

        private void RunScriptCommand_Action()
        {
            // Save file before execute
            Save();

            // Notify user working on command
            Controller_Global.MainControl?.Show_loading(true);
            Controller_Global.MainControl?.Set_Core_Output("Running script");

            // create thread and execute in background
            System.Threading.Thread TH01 = new(Run_script_async);
            TH01.Start();

            // TODO: get progress info from core and update to user
            // Core should implement a char buffer[500] or similar 
        }

        private bool RunScriptCommand_Check()
        {
            return true;
        }

        /// <summary>
        /// Runs core command in the background
        /// </summary>
        private void Run_script_async()
        {
            Controller_Global.MainControl?.Set_Core_Output(Controller_Global.ApiHandle.run_lua_command("run_lua_script ", Filename));
            Controller_Global.MainControl?.Show_loading(false);
        }
        #endregion
        #endregion

        #region Interface
        public bool Save()
        {
            if (_scriptingEditor != null)
            {
                if (save(_scriptingEditor.Scripting) == 0)
                {
                    _changesMade = false;
                    return true;
                }
                return false;
            }
            else { return false; }
        }
        public bool Close()
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
