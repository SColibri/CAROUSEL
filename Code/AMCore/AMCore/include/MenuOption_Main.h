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
	/// Display GUI
	/// </summary>
	void init() {
        if(_depth == MenuOption::HOME) menu_home();
        if (_depth == MenuOption::CONFIGURATIONS) menu_home();
	}

    void load_available_commands(std::vector<std::vector<std::string>>& Listlua)
    {
        _luaCommandList = Listlua;
    }

    void set_output(std::string& newoutput)
    {
        _out = newoutput;
    }


private:
    std::vector<std::vector<std::string>> _luaCommandList;
    std::string _command{}; // command input
    std::string _commandPrevious{}; // previous command input
    std::string _parameters{}; // flags or parameters to be used in the command
    std::vector<std::string> _commandsAvail{}; // list of available commands
    std::string _out{}; // output console

    /// <summary>
    /// compares _command with _commandPrevious and updates available commands
    /// </summary>
    /// <returns></returns>
    bool set_commandPrevious() 
    {   
        if(_command.compare(_commandPrevious))
        {
            _commandPrevious = _command;
            get_available_commands(_commandPrevious);
            validateCommand();
            return true;
        }
        return false;
    }

    /// <summary>
    /// gets available commands
    /// </summary>
    void get_available_commands(std::string& partialName) 
    {
        _commandsAvail.clear();
        
        for each (std::vector<std::string> commy in _luaCommandList)
        {
            if(commy[0].find(partialName) != std::string::npos)
            {
                _commandsAvail.push_back(commy[0] + " || " + 
                                         commy[1] + " || " + 
                                         commy[2]);
            }
        }
    }

    /// <summary>
    /// HOME menu design
    /// </summary>
    void menu_home() 
    {
        
        Element document = hbox({
          text("Welcome to ") | bold | color(Color::White),
          text("AMFramework") | bold | color(Color::Orange1),
        });

        Component options = get_options_menu();

        _command = "";
        _parameters = "";
        Component input_command = Input(&_command, "command");
        Component input_parameters = Input(&_parameters, "parameters");

        auto component = Container::Horizontal({
          input_command,
          input_parameters,
          options,
         });

        std::vector<Event> keys;

        auto renderer = Renderer(component, [&] {
            Elements children;

            children.push_back(
                vbox({
                    filler(),
                    paragraphAlignCenter(" +-+ +-+   +-+ +-+ +-+ +-+ +-+ +-+ +-+ +-+ +-+") | color(Color::Orange1),
                    paragraphAlignCenter(" |A| |M|   |F| |r| |a| |m| |e| |w| |o| |r| |k|") | color(Color::Orange1),
                    paragraphAlignCenter(" +-+ +-+   +-+ +-+ +-+ +-+ +-+ +-+ +-+ +-+ +-+") | color(Color::Orange1),
                    paragraphAlignCenter("v0.0.55"),
                    filler(),
                    document,
                    filler(),
                    separatorHeavy(),
                    hbox({options->Render()}),
                    filler(),
                    hbox({
                           text("  AM >> ") | color(Color::Orange3),
                           separator(),
                           hbox(text(" command : "), input_command->Render()),
                           hbox(text(" "), input_parameters->Render()),
                    }) | border
                }));

            children.push_back(component_commandsAvail());
            
            if (_out.length() > 0) 
            {
                children.push_back(
                    window(text("output"), paragraph(_out) | color(Color::Wheat1)) | color(Color::Yellow3Bis)
                );
            }
            
            if (_quit == true)
            {
               _screen.ExitLoopClosure();
            }
            

            return vbox(std::move(children));
        });

        
        
        Render(_screen, document);
        _screen.Loop(renderer);
    }

    /// <summary>
    /// GUI for menu options, this allows the user to navigate through the menu options
    /// </summary>
    /// <returns></returns>
    Component get_options_menu() {
        Component options = Collapsible("Navigate", Container::Horizontal({
            Button("Configurations", [&] {_depth = MenuOption::CONFIGURATIONS; }),
            Button("Quit", _screen.ExitLoopClosure())
        }));

        return options;
    }

    /// <summary>
    /// HEADER design of the menu
    /// </summary>
    /// <returns></returns>
    Element get_header_menu() 
    {
        Element document = hbox({
          text("Welcome to ") | bold | color(Color::White),
          text("AMFramework") | bold | color(Color::Orange1),
        });

        return document;
    }

    /// <summary>
    /// GUI for showing available commands available
    /// </summary>
    /// <returns></returns>
    Element component_commandsAvail() {
        Elements docy;


        if (set_commandPrevious()) {

            docy.push_back(text("Available commands:") | color(Color::Yellow1));
            docy.push_back(separatorDouble());
            for each (std::string commAvail in _commandsAvail)
            {
                docy.push_back(text(commAvail));
                docy.push_back(filler());
            }
        }


        Element document = vbox(
            std::move(docy)
        );
        
        return document;
    }

    
private:
    ScreenInteractive _screen = ScreenInteractive::TerminalOutput();
    bool _quit{ false }; // quit terminal AMFramework
    MenuOption _depth{ MenuOption::HOME }; // Current menu selection

    void validateCommand() 
    {
        
    }
};