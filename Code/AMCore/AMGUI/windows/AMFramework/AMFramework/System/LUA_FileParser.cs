using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFramework.AMSystem
{
    public class LUA_FileParser
    {
        public List<string> ModuleFile; // Stores all module filenames
        public List<ParseObject> AMParser;


        /// <summary>
        /// Constructor
        /// </summary>
        public LUA_FileParser()
        {
            ModuleFile = new();
            AMParser = new();
        }

        /// <summary>
        /// Remove all content
        /// </summary>
        public void Clear()
        {
            ModuleFile.Clear();
            AMParser.Clear();
        }

        /// <summary>
        /// Load Module information
        /// </summary>
        /// <param name="filename"></param>
        public void Load(string filename)
        {
            if (!filename.Contains("lua")) return; // not a valid file extension
            if (!System.IO.File.Exists(filename)) return; // non existing file
            if (this.ModuleFile.Contains(filename)) return; // module is already loaded

            // add filename into loaded modules
            ModuleFile.Add(filename);

            // read file and pass to parser
            string fileContent = System.IO.File.ReadAllText(filename);
            File_parse(fileContent, this, filename, "lua/");
        }

        public string Get_Classes_keywords() 
        {
            string keywords = "";

            List<ParseObject> refTemp = AMParser.FindAll(e => e.ObjectType == ParseObject.PTYPE.CLASS);
            foreach (var item in refTemp)
            {
                keywords += item.Name + " ";

                foreach (var item2 in item.Parameters)
                {
                    keywords += item.Name + "." + item2 + " ";
                }

                foreach (var item2 in item.functions)
                {
                    keywords += item.Name + ":" + item2.Name + " ";
                }
            }

            return keywords;
        }

        public string Get_Functions_keywords()
        {
            string keywords = "";

            List<ParseObject> refTemp = AMParser.FindAll(e => e.ObjectType == ParseObject.PTYPE.FUNCTION);
            foreach (var item in refTemp)
            {
                keywords += item.Name + " ";
            }

            return keywords;
        }

        public string Get_Global_variable_keywords()
        {
            string keywords = "";

            List<ParseObject> refTemp = AMParser.FindAll(e => e.ObjectType == ParseObject.PTYPE.GLOBAL_VARIABLE);
            foreach (var item in refTemp)
            {
                keywords += item.Name + " ";

                foreach (var item2 in item.Parameters)
                {
                    keywords += item.Name + "." + item2 + " ";
                }

                foreach (var item2 in item.functions)
                {
                    keywords += item.Name + ":" + item2.Name + " ";
                }
            }

            return keywords;
        }


        /// <summary>
        /// File parse finds all classes, functions and it's parameters.
        /// 
        /// Note on formatting:
        /// Classes in lua are defined as tables e.g. -> class = {param1 = value, param2 = value}
        /// functions have the keyword "function"
        /// Class functions have the keyword "function" but have an additional keyword ":" e.g. -> class::function(object, param1, param2)
        /// Descriptions are added on the same line where the function or class is defined with the "--@Description" tag
        /// </summary>
        /// <param name="fileContent"></param>
        /// <returns></returns>
        public static void File_parse(string fileContent, LUA_FileParser Parser, string modName, string workingDir = "lua/")
        {
            // split content by line
            List<string> fileRows = fileContent.Split("\n").ToList();

            // find all lines
            List<string> file_require = fileRows.FindAll(e => e.Contains("require") == true);
            List<string> file_classes = fileRows.FindAll(e => e.Contains("= {") == true || e.Contains("={") == true);
            List<string> file_classFunctions = fileRows.FindAll(e => e.Contains("function") == true && e.Contains(':') == true);
            List<string> file_functions = fileRows.FindAll(e => e.Contains("function") == true && e.Contains(':') == false);
            List<string> file_global_variables = fileRows.FindAll(e => e.Contains("=") == true && e.Contains(":new") == true && e.Contains("local") == false && e.Contains("function") == false);

            // Modules
            foreach (var item in file_require)
            {
                int IndexStart = item.IndexOf("\"");
                int IndexEnd = item.LastIndexOf("\"");
                if (IndexStart == -1 || IndexEnd == -1) continue;
                string filename = workingDir + item.Substring(IndexStart + 1, IndexEnd - IndexStart - 1).Replace(".", "/") + ".lua";
                Parser.Load(filename);
            }

            // Classes
            foreach (var item in file_classes)
            {
                // check if it defines parameter names or if it is just a table
                int IndexP1 = item.IndexOf('=');
                int IndexP2 = item.LastIndexOf('=');
                if(IndexP1 == IndexP2) continue;

                // extract class name, check if it is already contained
                string className = Get_class_name(item);
                if (Parser.AMParser.Find(e => e.Name.CompareTo(item) == 0) != null) continue; // avoid repeated classes (this might conflict with something)

                // Create class item
                ParseObject tempParse = new ParseObject();
                tempParse.ModuleName = modName;
                tempParse.ObjectType = ParseObject.PTYPE.CLASS;
                tempParse.Name = Get_class_name(item);
                tempParse.ParametersType = Get_Class_parameters_type(item);
                tempParse.Parameters = Get_parameter_names(tempParse.ParametersType);
                tempParse.Description = Get_description(item);
                Parser.AMParser.Add(tempParse);

                // get all functions of class
                List<string> classFunctions = file_classFunctions.FindAll(e => e.Contains(tempParse.Name));
                if (classFunctions == null) continue;
                foreach (var classF in classFunctions)
                {
                    ParseObject tempFunction = new ParseObject();
                    tempFunction.ObjectType = ParseObject.PTYPE.FUNCTION;
                    tempFunction.Name = Get_classFunction_name(classF);
                    tempFunction.Description= Get_description(classF);
                    tempFunction.ParametersType= Get_function_parameters(classF);
                    tempFunction.Parameters = Get_parameter_names(tempFunction.ParametersType);
                    tempParse.functions.Add(tempFunction);
                }
            }

            // functions
            foreach (var item in file_functions)
            {
                string className = Get_class_name(item);
                if (Parser.AMParser.Find(e => e.Name.CompareTo(item) == 0) != null) continue;

                ParseObject tempParse = new ParseObject();
                tempParse.ModuleName = modName;
                tempParse.ObjectType = ParseObject.PTYPE.FUNCTION;
                tempParse.Name = Get_function_name(item);
                tempParse.ParametersType = Get_function_parameters(item);
                tempParse.Parameters = Get_parameter_names(tempParse.ParametersType);
                tempParse.Description = Get_description(item);
                Parser.AMParser.Add(tempParse);
            }

            // functions
            foreach (var item in file_global_variables)
            {
                string className = Get_class_name(item);
                if (Parser.AMParser.Find(e => e.Name.CompareTo(item) == 0) != null) continue;

                ParseObject tempParse = new ParseObject();
                tempParse.ModuleName = modName;
                tempParse.ObjectType = ParseObject.PTYPE.GLOBAL_VARIABLE;
                tempParse.Name = Get_class_name(item);
                tempParse.ParametersType = Get_variable_className(item);

                tempParse.Parameters = Parser.AMParser.FindAll(e => e.Name.CompareTo(tempParse.ParametersType) == 0 && 
                                                               e.ObjectType == ParseObject.PTYPE.CLASS).Select(e => e.Parameters).ToList().SelectMany(e => e).ToList();

                tempParse.functions = Parser.AMParser.FindAll(e => e.Name.CompareTo(tempParse.ParametersType) == 0 &&
                                                               e.ObjectType == ParseObject.PTYPE.FUNCTION).Select(e => e.functions).ToList().SelectMany(e => e).ToList();
                
                tempParse.Description = Get_description(item);
                Parser.AMParser.Add(tempParse);
            }

        }

        /// <summary>
        /// Removes module function and classes declarations
        /// </summary>
        /// <param name="modName">file path to module</param>
        /// <param name="Parser"></param>
        public static void Remove_module(string modName, LUA_FileParser Parser) 
        {
            Parser.AMParser.RemoveAll(e => e.ModuleName.CompareTo(modName) == 0);
        }


        /// <summary>
        /// Obtains the class name from a string object (LUA sctripting)
        /// </summary>
        /// <param name="rowLine"></param>
        /// <returns></returns>
        private static string Get_class_name(string rowLine)
        {
            // copy string and remove spaces
            string copyRowLine = rowLine;
            copyRowLine.Replace(" ", "");

            // find key where class name ends
            int Index = copyRowLine.IndexOf('=');
            if (Index == -1) return "";

            return copyRowLine.Substring(0, Index--).Trim();
        }

        /// <summary>
        /// Obtains class function name from a string object (LUA Scripting)
        /// </summary>
        /// <param name="rowLine"></param>
        /// <returns></returns>
        private static string Get_classFunction_name(string rowLine)
        {
            // copy string and remove spaces
            string copyRowLine = rowLine;
            copyRowLine.Replace(" ", "");

            // find key where class name ends
            int Indexstart = copyRowLine.IndexOf(":");
            int IndexEnd = copyRowLine.IndexOf("(");
            if (Indexstart == -1 || IndexEnd == -1) return "";

            // Jump to function name by removing the ":" keyword
            int keyLength = 1;
            Indexstart += keyLength;

            return copyRowLine.Substring(Indexstart, IndexEnd - Indexstart).Trim();
        }

        /// <summary>
        /// Obtains the description from the tag --@Description
        /// </summary>
        /// <param name="rowLine"></param>
        /// <returns></returns>
        private static string Get_description(string rowLine)
        {
            // find key where class name ends
            int Index = rowLine.IndexOf("--@Description");
            if (Index == -1) return "";

            // Amount of char elements on the description tag
            int keylength = 14;

            return rowLine.Substring(Index + keylength, rowLine.Length - (Index + keylength));
        }

        /// <summary>
        /// Obtains the class parameters and addss its datatype
        /// </summary>
        /// <param name="rowLine"></param>
        /// <returns></returns>
        private static string Get_Class_parameters_type(string rowLine)
        {
            // copy string and remove spaces
            string copyRowLine = rowLine;
            copyRowLine.Replace(" ", "");

            // find parameter values
            int IndexStart = copyRowLine.IndexOf('{');
            int IndexEnd = copyRowLine.IndexOf('}');
            if (IndexStart == -1 || IndexEnd == -1) return "";

            // get parameters only
            copyRowLine = copyRowLine.Substring(IndexStart + 1, IndexEnd);

            return Get_parameters(copyRowLine);
        }

        /// <summary>
        /// Using the output of the function Get_Class_parameters_type as input
        /// we obtain the parameter names.
        /// </summary>
        /// <param name="parameters_type"></param>
        /// <returns></returns>
        private static List<string> Get_parameter_names(string parameters_type)
        {
            // get parameter row
            List<string> parameterRow = parameters_type.Split(",").ToList();

            // return value
            List<string> classParameters = new();

            // get parameter names
            foreach (string item in parameterRow) 
            {
                int IndexEqSgn = item.IndexOf('=');
                if (IndexEqSgn == -1) continue;
                classParameters.Add(item.Substring(0, IndexEqSgn).Trim());
            }

            return classParameters;
        }

        /// <summary>
        /// Obatins function parameters from string object
        /// </summary>
        /// <param name="rowLine"></param>
        /// <returns></returns>
        private static string Get_function_parameters(string rowLine)
        {
            // copy string and remove spaces
            string copyRowLine = rowLine;
            copyRowLine.Replace(" ", "");

            // find parameter values
            int IndexStart = copyRowLine.IndexOf('(');
            int IndexEnd = copyRowLine.IndexOf(')');
            if (IndexStart == -1 || 
                IndexEnd == -1 || 
                IndexStart + 1 == IndexEnd) return "";

            // get parameters only
            copyRowLine = copyRowLine.Substring(IndexStart + 1, IndexEnd - IndexStart);

            return Get_parameters(copyRowLine); ;
        }

        /// <summary>
        /// Obtains functio name from string object
        /// </summary>
        /// <param name="rowLine"></param>
        /// <returns></returns>
        private static string Get_function_name(string rowLine)
        {
            // copy string and remove spaces
            string copyRowLine = rowLine;
            copyRowLine.Replace(" ", "");

            // find key where class name ends
            int Indexstart = copyRowLine.IndexOf("function");
            int IndexEnd = copyRowLine.IndexOf("(");
            if (Indexstart == -1 || IndexEnd == -1 || IndexEnd <= Indexstart) return "";

            // Jump to function name by removing the "function" keyword
            int keyLength = 8;
            Indexstart += keyLength;

            return copyRowLine.Substring(Indexstart, IndexEnd - Indexstart);
        }

        /// <summary>
        /// Obtains the parameters from string section that only contains the
        /// parameterss sepparated by the "," key
        /// </summary>
        /// <param name="parameterSection"></param>
        /// <returns></returns>
        private static string Get_parameters(string parameterSection)
        {
            // get parameter list
            List<string> ParameterLines = parameterSection.Split(",").ToList();

            // if there are no parameters stop
            if (ParameterLines[0].Length == 0) return "";

            // Change content by its type
            List<string> NewParameters = new();
            foreach (var item in ParameterLines)
            {
                // Get parameter line
                int IndexOfEqual = item.IndexOf('=');
                if(IndexOfEqual == -1) 
                {
                    NewParameters.Add(item);
                    continue;
                }

                string DataOfClass = item.Substring(IndexOfEqual, item.Length - IndexOfEqual).Trim();
                string ClassDataType = "NI"; // defult "Not Identified"

                // get data type
                if (DataOfClass.All(char.IsDigit))
                { ClassDataType = "Number"; }
                else if (DataOfClass.Contains("\""))
                { ClassDataType = "String"; }
                else
                { ClassDataType = item.Substring(0, IndexOfEqual); }

                // Add parameter to list
                NewParameters.Add(item.Substring(0, IndexOfEqual) + " = " + ClassDataType);
            }

            // convert list to string
            string NewParameterContent = "";
            foreach (var item in NewParameters)
            {
                if (NewParameterContent.Length > 0) { NewParameterContent += ", "; }
                NewParameterContent += item;
            }

            return NewParameterContent;
        }

        /// <summary>
        /// Get variable names from declaration of valid objects
        /// </summary>
        /// <param name="rowLine"></param>
        /// <returns></returns>
        private static string Get_variable_className(string rowLine) 
        {
            // copy string and remove spaces
            string copyRowLine = rowLine;
            copyRowLine.Replace(" ", "");

            // find key where class name ends
            int Index01 = copyRowLine.IndexOf('=');
            int Index02 = copyRowLine.IndexOf(':');
            if (Index01 == -1 || Index02 == -1) return "";



            return copyRowLine.Substring(Index01 + 1, Index02 - Index01 -1).Trim();
        }

    }
}
