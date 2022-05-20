#include "../include/MenuOption_Main.h"

void MenuOption_Main::init() {
    if (_depth == MenuOption::HOME) menu_home();
    if (_depth == MenuOption::CONFIGURATIONS) menu_home();
}

void MenuOption_Main::load_available_commands(std::vector<std::vector<std::string>>& Listlua)
{
    _luaCommandList = Listlua;
}

void MenuOption_Main::set_output(std::string& newoutput)
{
    _out = newoutput;
}

void MenuOption_Main::set_luaInterpreter(IAM_API* lua)
{
    _api_lua = lua;
}


#pragma region GUI
/// <summary>
/// HOME menu design
/// </summary>
void MenuOption_Main::menu_home()
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
                vbox({
                    paragraphAlignCenter(" +-+ +-+   +-+ +-+ +-+ +-+ +-+ +-+ +-+ +-+ +-+") | color(Color::Orange1),
                    paragraphAlignCenter(" |A| |M|   |F| |r| |a| |m| |e| |w| |o| |r| |k|") | color(Color::Orange1),
                    paragraphAlignCenter(" +-+ +-+   +-+ +-+ +-+ +-+ +-+ +-+ +-+ +-+ +-+") | color(Color::Orange1),
                    paragraphAlignCenter("v0.0.55"),
                    }),

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
                }) | border,
                filler(),
                }));

        children.push_back(component_commandsAvail());

        children.push_back(
            vbox({ window(text("output"), hflow(paragraph(_out)) | color(Color::Wheat1)) | color(Color::Yellow3Bis), }));

        if (_quit == true)
        {
            _screen.ExitLoopClosure();
        }


        return vbox(std::move(children));
        });

    renderer |= CatchEvent([&](Event event) {return Handle_KeyPress(event); });


    Render(_screen, document);
    _screen.Loop(renderer);
}

/// <summary>
/// GUI for menu options, this allows the user to navigate through the menu options
/// </summary>
/// <returns></returns>
ftxui::Component MenuOption_Main::get_options_menu() {
    ftxui::Component options = Collapsible("Navigate", Container::Horizontal({
        Button("Configurations", [&] {_depth = MenuOption::CONFIGURATIONS; }),
        Button("Quit", _screen.ExitLoopClosure())
        }));

    return options;
}

/// <summary>
/// HEADER design of the menu
/// </summary>
/// <returns></returns>
Element MenuOption_Main::get_header_menu()
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
Element MenuOption_Main::component_commandsAvail() {
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
#pragma endregion

#pragma region Commands
/// <summary>
/// compares _command with _commandPrevious and updates available commands
/// </summary>
/// <returns></returns>
bool MenuOption_Main::set_commandPrevious()
{
    if (_command.compare(_commandPrevious))
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
void MenuOption_Main::get_available_commands(std::string& partialName)
{
    _commandsAvail.clear();
    _commandsName.clear();

    for each (std::vector<std::string> commy in _luaCommandList)
    {
        if (commy[0].find(partialName) != std::string::npos)
        {
            _commandsAvail.push_back(commy[0] + " || " +
                commy[1] + " || " +
                commy[2]);

            _commandsName.push_back(commy[0]);
        }
    }
}
/// <summary>
/// Validate entered commands
/// </summary>
void MenuOption_Main::validateCommand()
{
    _commandValidated = false;
    if (_command.length() <= 1) return;
    if (_api_lua == nullptr) _out = "No interpreter available!";
    else if (_commandsName.size() == 0) _out = "Command not recognized!";
    else if (_commandsName.size() > 1) _out = "There are multiple options, which one do you want to use?";
    else _commandValidated = true;
}

/// <summary>
/// exeecute commands using the lua interpreter
/// </summary>
void MenuOption_Main::executeCommand()
{
    if (_commandValidated)
    {
        if (_parameters.length() > 0)
        {
            std::vector<std::string> Parameters;
            SplitString(_parameters, Parameters);
            _out = _api_lua->run_lua_command(_commandsName[0], Parameters);
        }
        else
        {
            _out = _api_lua->run_lua_command(_commandsName[0]);
        }

        _command = "*";
        _parameters = "";
    }
}
#pragma endregion

#pragma region Handles
/// <summary>
/// Handle keypress event
/// </summary>
bool MenuOption_Main::Handle_KeyPress(Event& event)
{
    //Handle the new line key, check if the command is valid.
    //If only one valid command exists, the system will select
    //The only available option.
    if (std::strcmp(event.character().c_str(), "\n") == 0)
    {
        validateCommand();
        executeCommand();
    }

    return false;
}
#pragma endregion

#pragma region Helpers
/// <summary>
/// Split string using a delimiter
/// </summary>
/// <param name="s"></param>
/// <param name="v"></param>
void MenuOption_Main::SplitString(std::string s, std::vector<std::string>& v) {

    std::string temp = "";
    for (int i = 0; i < s.length(); ++i) {

        if (s[i] == ' ') {
            v.push_back(temp);
            temp = "";
        }
        else {
            temp.push_back(s[i]);
        }

    }
    v.push_back(temp);

}
#pragma endregion
