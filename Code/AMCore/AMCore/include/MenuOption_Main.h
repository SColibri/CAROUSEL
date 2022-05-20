#pragma once

#include <array>       // for array
#include <chrono>      // for milliseconds
#include <functional>  // for function
#include <memory>      // for shared_ptr, __shared_ptr_access, allocator
#include <vector>      // for vector
#include <string>  // for char_traits, operator+, string, basic_string

#include "ftxui/component/captured_mouse.hpp"  // for ftxui
#include "ftxui/component/component.hpp"       // for Input, Renderer, Vertical
#include "ftxui/component/component_base.hpp"  // for ComponentBase
#include "ftxui/component/component_options.hpp"  // for InputOption
#include "ftxui/component/screen_interactive.hpp"  // for Component, ScreenInteractive
#include "ftxui/dom/elements.hpp"  // for text, hbox, separator, Element, operator|, vbox, border
#include "ftxui/util/ref.hpp"  // for Ref
#include "ftxui/dom/table.hpp" 
#include "ftxui/component/event.hpp" 

#include "../../AMLib/include/AM_lua_interpreter.h"
#include "../../AMLib//interfaces/IAM_API.h"


using namespace ftxui;

/// <summary>
/// Terminal GUI for user interaction, this menu allows calling core functions.
/// </summary>
class MenuOption_Main {
    
public:

    /// <summary>
    /// Options for menu
    /// </summary>
    enum MenuOption {
        HOME,
        CONFIGURATIONS,
        SOCKET
    };

	/// <summary>
	/// Start display GUI
	/// </summary>
    void init();

    /// <summary>
    /// load available lua commands for user reference
    /// </summary>
    /// <param name="Listlua"></param>
    void load_available_commands(std::vector<std::vector<std::string>>& Listlua);

    /// <summary>
    /// Set output text in window
    /// </summary>
    /// <param name="newoutput"></param>
    void set_output(std::string& newoutput);

    /// <summary>
    /// set lua interpreter, used for executing commands
    /// </summary>
    /// <param name="lua"></param>
    void set_luaInterpreter(IAM_API* lua);


private:
    std::vector<std::vector<std::string>> _luaCommandList;
    std::string _command{}; // command input
    std::string _commandPrevious{}; // previous command input
    std::string _parameters{}; // flags or parameters to be used in the command
    std::vector<std::string> _commandsAvail{}; // list of available commands
    std::vector<std::string> _commandsName{}; // list of available commands
    std::string _out{}; // output console

    ScreenInteractive _screen = ScreenInteractive::TerminalOutput(); // ftxui screen
    bool _quit{ false }; // quit terminal AMFramework
    bool _commandValidated{ false };
    MenuOption _depth{ MenuOption::HOME }; // Current menu selection
    IAM_API* _api_lua{ nullptr }; // lua interpreter

#pragma region GUI
    /// <summary>
    /// HOME menu design
    /// </summary>
    void menu_home();

    /// <summary>
    /// GUI for menu options, this allows the user to navigate through the menu options
    /// </summary>
    /// <returns></returns>
    Component get_options_menu();

    /// <summary>
    /// HEADER design of the menu
    /// </summary>
    /// <returns></returns>
    Element get_header_menu();

    /// <summary>
    /// GUI for showing available commands available
    /// </summary>
    /// <returns></returns>
    Element component_commandsAvail();

#pragma endregion

#pragma region Commands
    /// <summary>
    /// compares _command with _commandPrevious and updates available commands
    /// </summary>
    /// <returns></returns>
    bool set_commandPrevious();

    /// <summary>
    /// gets available commands
    /// </summary>
    void get_available_commands(std::string& partialName);

    /// <summary>
    /// Validate entered commands
    /// </summary>
    void validateCommand();

    /// <summary>
    /// exeecute commands using the lua interpreter
    /// </summary>
    void executeCommand();
#pragma endregion
    
#pragma region Handles
    /// <summary>
    /// Handle keypress event
    /// </summary>
    bool Handle_KeyPress(Event& event);
#pragma endregion

#pragma region Helpers
    /// <summary>
    /// Split string using a delimiter
    /// </summary>
    /// <param name="s"></param>
    /// <param name="v"></param>
    void SplitString(std::string s, std::vector<std::string>& v);
#pragma endregion


};