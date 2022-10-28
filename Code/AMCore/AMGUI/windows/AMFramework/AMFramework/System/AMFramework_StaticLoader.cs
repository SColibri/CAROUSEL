using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMFramework.AMSystem
{
    /// <summary>
    /// Static loader, loads all static data needed for running the program. e.g. Models require the list of available commands 
    /// that are available for each model.
    /// </summary>
    public class AMFramework_StaticLoader
    {
        #region Models
        /// <summary>
        /// Loads all Model commands, based on core commands and specs. If core adds commands this will be reflected here, however if you don't handle the command
        /// it just gets ignored. (e.g. for MCE, we have declared the Save command but for example core might also offer the load project function, which on the gui side is not needed, however
        /// if I want to add it, I would just create a new class named MCE_loadProject. Further commands can be found in the lua_functions.txt, there you cand find a brief description on what it does,
        /// for more infromation please refer the documentation.
        /// 
        /// NOTE: If this is not loaded at startup of the application, commands will return as null, so just don't forget to run this static function
        /// </summary>
        /// <param name="comm"></param>
        public static void Load_Model_Commands(ref Core.IAMCore_Comm comm) 
        { 
            // clear all commands
            Model.ModelAbstract.CommandList.Clear();

            // Check if file was created!
            if (!System.IO.File.Exists("lua_functions.txt")) return;

            // open file and read its content
            System.IO.StreamReader sR = new("lua_functions.txt");
            string? lineRead = null;

            while((lineRead = sR.ReadLine()) != null) 
            {
                // Load line with descriptions 
                List<string> LineCommand = lineRead.Split(",").ToList();
                if (LineCommand.Count == 3) continue;

                // split specifications and check if it was specified
                List<string> CommandSpecs = LineCommand[3].Split("||").ToList();
                if (CommandSpecs.Count < 2) continue;

                // Get MCE types and Model types for the commands based on object names.
                Type? ExecutorT = Model.ModelCoreExecutors.MCE_Factory.Get_Type(CommandSpecs[1]);
                Type? ModelT = Model.ModelFactory.Get_Type(CommandSpecs[0]);
                if (ExecutorT == null || ModelT == null) continue;

                Model.ModelCoreCommand tempCommand = new() { Command_instruction = LineCommand[0],
                                                                     ObjectType = ModelT,
                                                                     Executor_Type = ExecutorT};

                // Add command to static list
                Model.ModelAbstract.CommandList.Add(tempCommand);
            }

        }
        #endregion



    }
}
