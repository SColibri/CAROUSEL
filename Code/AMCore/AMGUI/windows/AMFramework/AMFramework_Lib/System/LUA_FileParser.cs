namespace AMFramework_Lib.AMSystem
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
                keywords += item.Name + "?3 ";

                foreach (var item2 in item.Parameters)
                {
                    keywords += item.Name + "." + item2 + "?2 ";
                }

                foreach (var item2 in item.functions)
                {
                    keywords += item.Name + ":" + item2.Name + "?1 ";
                }
            }

            keywords = keywords.Trim();
            return keywords;
        }

        public string Get_Functions_keywords()
        {
            string keywords = "";

            List<ParseObject> refTemp = AMParser.FindAll(e => e.ObjectType == ParseObject.PTYPE.FUNCTION);
            foreach (var item in refTemp)
            {
                keywords += item.Name + "?1 ";
            }

            keywords = keywords.Trim();
            return keywords;
        }

        public string Get_Global_variable_keywords()
        {
            string keywords = "";

            List<ParseObject> refTemp = AMParser.FindAll(e => e.ObjectType == ParseObject.PTYPE.GLOBAL_VARIABLE);
            foreach (var item in refTemp)
            {
                keywords += item.Name + "?0 ";
            }

            keywords = keywords.Trim();
            return keywords;
        }

        public string Get_Global_variable_parameters(List<string> variableLine)
        {
            if (variableLine.Count == 0) return "";

            string keywords = "";
            ParseObject refTemp = AMParser.Find(e => e.Name.CompareTo(variableLine[0]) == 0);
            if (refTemp is null) return "";

            if (variableLine.Count > 1)
            {
                keywords = Get_recursive_parameters_keywords(refTemp, variableLine, 1);
            }
            else
            {
                foreach (var item in refTemp.Parameters)
                {
                    keywords += item.Name + "?2 ";
                }
            }

            keywords = keywords.Trim();
            return keywords;
        }

        /// <summary>
        /// From a string that has a variable trail form, finds all parameters.
        /// </summary>
        /// <param name="ClassObject"></param>
        /// <param name="VariableTrail"></param>
        /// <param name="Index"></param>
        /// <returns></returns>
        public string Get_recursive_parameters_keywords(ParseObject ClassObject, List<string> VariableTrail, int Index)
        {

            // Check if parameter is contained in current class object
            string CurrentParameter = VariableTrail[Index];
            Remove_bracket_from_parameter(ref CurrentParameter);

            ParseObject? parseObject = ClassObject.Parameters.Find(e => e.Name.CompareTo(CurrentParameter) == 0);
            if (parseObject == null) return "";

            // Check if current parameter is a class
            ParseObject? parseClasses = AMParser.Find(e => e.Name.CompareTo(parseObject.ParametersType) == 0);
            if (parseClasses == null) return "";

            // check if the trail has more entries and if not, return list of parameters for the current
            // class
            if (VariableTrail.Count - 1 > Index)
            {
                return Get_recursive_parameters_keywords(parseClasses, VariableTrail, Index++);
            }
            else
            {
                string keywords = "";
                foreach (var item in parseClasses.Parameters)
                {
                    keywords += item.Name + "?2 ";
                }
                return keywords;
            }
        }

        private void Remove_bracket_from_parameter(ref string ParamName)
        {
            int IndexOpen = ParamName.IndexOf('[');
            int IndexClose = ParamName.IndexOf(']');
            if (IndexOpen == -1 || IndexClose == -1 || IndexClose <= IndexOpen) return;

            ParamName = ParamName.Substring(0, IndexOpen);
        }

        public string Get_Global_variable_functions(List<string> variableLine)
        {
            if (variableLine.Count == 0) return "";

            string keywords = "";
            ParseObject refTemp = AMParser.Find(e => e.Name.CompareTo(variableLine[0]) == 0);
            if (refTemp is null) return "";

            if (variableLine.Count > 1)
            {
                ParseObject ref_leve2 = null;
                string currentClass = variableLine[0];

                foreach (var item in variableLine.Skip(1))
                {
                    ref_leve2 = refTemp.functions.Find(e => e.Name.CompareTo(currentClass) == 0);
                    if (ref_leve2 == null) break;
                    currentClass = ref_leve2.Name;
                }

                if (ref_leve2 == null) return "";
                foreach (var item in ref_leve2.functions)
                {
                    keywords += item.Name + "?1 ";
                }
            }
            else
            {
                foreach (var item in refTemp.functions)
                {
                    keywords += item.Name + "?1 ";
                }

                ParseObject refTemp_Class = AMParser.Find(e => e.Name.CompareTo(refTemp.ParametersType) == 0);
                if (refTemp_Class != null)
                {
                    foreach (var item in refTemp_Class.functions)
                    {
                        keywords += item.Name + "?1 ";
                    }
                }
            }

            keywords = keywords.Trim();
            return keywords;
        }

        public string Remove_Icon_tags(string keywords)
        {
            return keywords.Replace("?0", "").Replace("?1", "").Replace("?2", "").Replace("?3", "");
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

            List<string> file_global_variables = fileRows.FindAll(e => e.Contains("=") == true &&
                                                                       e.Contains(":new") == true &&
                                                                       e.Contains("o.") == false &&
                                                                       e.Contains("self.") == false &&
                                                                       e.Contains("local") == false &&
                                                                       e.Contains("function") == false);

            List<string> file_local_variables = fileRows.FindAll(e => e.Contains("=") == true &&
                                                                      e.Contains(":new") == true &&
                                                                      e.Contains("o.") == false &&
                                                                      e.Contains("self.") == false &&
                                                                      e.Contains("local") == true &&
                                                                      e.Contains("function") == false);

            List<string> file_parameter_types = fileRows.FindAll(e => e.Contains("--@TYPE", StringComparison.OrdinalIgnoreCase) == true);

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
                if (IndexP1 == IndexP2) continue;

                // extract class name, check if it is already contained
                string className = Get_class_name(item);
                if (className.Length == 0) continue;
                if (Parser.AMParser.Find(e => e.Name.CompareTo(item) == 0) != null) continue; // avoid repeated classes (this might conflict with something)

                // Create class item
                ParseObject tempParse = new()
                {
                    ModuleName = modName,
                    ObjectType = ParseObject.PTYPE.CLASS,
                    Name = Get_class_name(item),
                    ParametersType = Get_Class_parameters_type(item)
                };
                tempParse.Parameters = Get_parameter_names(tempParse.ParametersType);
                Get_parameter_type(file_parameter_types, tempParse.Parameters);
                tempParse.Description = Get_description(item);
                Parser.AMParser.Add(tempParse);

                // get all functions of class
                List<string> classFunctions = file_classFunctions.FindAll(e => e.Contains(tempParse.Name));
                if (classFunctions == null) continue;
                foreach (var classF in classFunctions)
                {
                    ParseObject tempFunction = new()
                    {
                        ObjectType = ParseObject.PTYPE.FUNCTION,
                        Name = Get_classFunction_name(classF),
                        Description = Get_description(classF),
                        ParametersType = Get_function_parameters(classF)
                    };
                    tempFunction.Parameters = Get_parameter_names(tempFunction.ParametersType);
                    tempParse.functions.Add(tempFunction);
                }
            }

            // functions
            foreach (var item in file_functions)
            {
                string functionName = Get_function_name(item);
                if (functionName.Length == 0) continue;
                if (Parser.AMParser.Find(e => e.Name.CompareTo(item) == 0) != null) continue;

                ParseObject tempParse = new()
                {
                    ModuleName = modName,
                    ObjectType = ParseObject.PTYPE.FUNCTION,
                    Name = Get_function_name(item),
                    ParametersType = Get_function_parameters(item)
                };
                tempParse.Parameters = Get_parameter_names(tempParse.ParametersType);
                tempParse.Description = Get_description(item);
                Parser.AMParser.Add(tempParse);
            }

            // Global variables
            foreach (var item in file_global_variables)
            {
                // o. is used in class definitions for all generic tables
                if (item.Contains("o.")) continue;

                string className = Get_class_name(item);
                if (Parser.AMParser.Find(e => e.Name.CompareTo(item) == 0) != null) continue;

                ParseObject tempParse = new()
                {
                    ModuleName = modName,
                    ObjectType = ParseObject.PTYPE.GLOBAL_VARIABLE,
                    Name = Get_class_name(item),
                    ParametersType = Get_variable_className(item)
                };

                tempParse.Parameters = Parser.AMParser.FindAll(e => e.Name.CompareTo(tempParse.ParametersType) == 0 &&
                                                               e.ObjectType == ParseObject.PTYPE.CLASS).Select(e => e.Parameters).ToList().SelectMany(e => e).ToList();

                tempParse.functions = Parser.AMParser.FindAll(e => e.Name.CompareTo(tempParse.ParametersType) == 0 &&
                                                               e.ObjectType == ParseObject.PTYPE.CLASS).Select(e => e.functions).ToList().SelectMany(e => e).ToList();

                tempParse.Description = Get_description(item);
                Parser.AMParser.Add(tempParse);
            }

            // local variables
            foreach (var item in file_local_variables)
            {
                if (Parser.AMParser.Find(e => e.Name.CompareTo(item) == 0) != null) continue;

                ParseObject tempParse = new()
                {
                    ModuleName = modName,
                    ObjectType = ParseObject.PTYPE.LOCAL_VARIABLE,
                    Name = Get_class_name(item),
                    ParametersType = Get_variable_className(item)
                };

                tempParse.Parameters = Parser.AMParser.FindAll(e => e.Name.CompareTo(tempParse.ParametersType) == 0 &&
                                                               e.ObjectType == ParseObject.PTYPE.CLASS).Select(e => e.Parameters).ToList().SelectMany(e => e).ToList();

                tempParse.functions = Parser.AMParser.FindAll(e => e.Name.CompareTo(tempParse.ParametersType) == 0 &&
                                                               e.ObjectType == ParseObject.PTYPE.CLASS).Select(e => e.functions).ToList().SelectMany(e => e).ToList();

                ParseObject? tempObject = Parser.AMParser.Find(e => e.Name.CompareTo(tempParse.ParametersType) == 0);
                if (tempObject != null) tempParse.Description = tempObject.Description;

                tempParse.Description += Get_description(item);
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
            copyRowLine = copyRowLine.Replace("local", "").Replace(" ", "");

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
            List<int> closingBrakets = Find_all_occurrences(copyRowLine, "}");
            List<int> openingBrakets = Find_all_occurrences(copyRowLine, "{");

            int IndexStart = openingBrakets[0];
            int IndexEnd = closingBrakets[closingBrakets.Count - 1];
            if (IndexStart == -1 || IndexEnd == -1 || IndexEnd <= IndexStart) return "";

            // get parameters only
            copyRowLine = copyRowLine.Substring(IndexStart + 1, IndexEnd - (IndexStart +1));

            return Get_parameters(copyRowLine);
        }

        /// <summary>
        /// finds all occurrences in a string 
        /// </summary>
        /// <param name="content"></param>
        /// <param name="ToFind"></param>
        /// <returns>List of indexes</returns>
        public static List<int> Find_all_occurrences(string content, string ToFind)
        {
            List<int> ocurrences = new();

            for (int n1 = 0; n1 < content.Length; n1++)
            {
                n1 = content.IndexOf(ToFind, n1);
                if (n1 == -1) break;

                ocurrences.Add(n1);
            }

            if (ocurrences.Count == 0) ocurrences.Add(-1);
            return ocurrences;
        }

        /// <summary>
        /// Using the output of the function Get_Class_parameters_type as input
        /// we obtain the parameter names.
        /// </summary>
        /// <param name="parameters_type"></param>
        /// <returns></returns>
        private static List<ParseObject> Get_parameter_names(string parameters_type)
        {
            // get parameter row
            List<string> parameterRow = parameters_type.Split(",").ToList();

            // return value
            List<ParseObject> classParameters = new();

            // get parameter names
            foreach (string item in parameterRow)
            {
                int IndexEqSgn = item.IndexOf('=');
                if (IndexEqSgn == -1) continue;

                ParseObject tempObj = new()
                {
                    ObjectType = ParseObject.PTYPE.LOCAL_VARIABLE,
                    Name = item.Substring(0, IndexEqSgn).Trim()
                };

                classParameters.Add(tempObj);
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
                if (IndexOfEqual == -1)
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
        /// Updates the vector Params with the parameter type. Type is specified in the lua script
        /// of the object wit the --@TYPE "Parameter type"
        /// </summary>
        /// <param name="LineContent"></param>
        /// <param name="Params"></param>
        private static void Get_parameter_type(List<string> LineContent, List<ParseObject> Params)
        {
            foreach (var item in LineContent)
            {
                // indexes for star and end of word search and size
                int WordStart, WordEnd, WordSize;

                // -----------------------------------------------------------------
                //                   Find the parameter name
                // -----------------------------------------------------------------
                WordStart = item.IndexOf("self.");
                WordEnd = item.IndexOf("=");
                if (WordEnd <= WordStart || (WordStart == -1 || WordEnd == -1)) continue;
                WordStart += 5; // start after self.

                WordSize = WordEnd - WordStart;
                string parameterName = item.Substring(WordStart, WordSize).Trim();

                // -----------------------------------------------------------------
                //                   Find the Type name
                // -----------------------------------------------------------------
                WordStart = item.ToUpper().IndexOf("--@TYPE");
                WordEnd = item.Length;
                if (WordEnd <= WordStart || (WordStart == -1 || WordEnd == -1)) continue;
                WordStart += 7; // start after TYPE.

                WordSize = WordEnd - WordStart;
                string parameterType = item.Substring(WordStart, WordSize).Trim();

                // -----------------------------------------------------------------
                //        Search for parameter name and update the type
                // -----------------------------------------------------------------
                ParseObject? parObject = Params.Find(e => e.Name.CompareTo(parameterName) == 0);
                if (parObject == null) continue;
                parObject.ObjectType = ParseObject.PTYPE.CLASS;
                parObject.ParametersType = parameterType;

            }

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
            if (Index01 == -1 || Index02 == -1 || Index02 < Index01 + 1) return "";



            return copyRowLine.Substring(Index01 + 1, Index02 - Index01 -1).Trim();
        }

    }
}
