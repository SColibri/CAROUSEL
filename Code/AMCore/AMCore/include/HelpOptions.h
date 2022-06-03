#pragma once

#include <string>
#include <vector>
#include <map>
#include <fstream>
#include <exception>
#include <iostream>

#include "ftxui/component/captured_mouse.hpp"  
#include "ftxui/component/component.hpp"     
#include "ftxui/component/component_base.hpp"  
#include "ftxui/component/component_options.hpp"  
#include "ftxui/component/screen_interactive.hpp"  
#include "ftxui/dom/elements.hpp" 
#include "ftxui/util/ref.hpp"  
#include "ftxui/dom/table.hpp" 
#include "ftxui/component/event.hpp" 

/** \addtogroup AMCore
  *  @{
  */

/// <summary>
/// Gets and displays all available options.
/// To add a ne option you have to declare a new getter,
/// add the flag to the definition in load_definitions() and
/// add a description in the Options.txt (inside the include folder)
/// </summary>
class HelpOptions {

public:

#pragma region Cons_des
	HelpOptions() 
	{
		load_definitions();
		Show_help();
	}

	HelpOptions(int argc, char* argv[])
	{
		load_definitions();
		set_definitions(argc, argv);
	}
#pragma endregion

	/// <summary>
	/// Prints the options table on the console
	/// </summary>
	void Show_help() {
		using namespace ftxui;
		Elements WindowContent;
		
		if (_error.length() > 0)
		{
			WindowContent.push_back(paragraph("Error: " + _error) | color(Color::Red));
		}
		load_help_option(WindowContent);

		Element HelpDisplay = window(text("AMFramework Help options") | color(Color::Orange1), vbox(std::move(WindowContent)));
		Screen screen = Screen::Create(Dimension::Fit(HelpDisplay));
		Render(screen, HelpDisplay);
		screen.Print();
	}

	/// <summary>
	/// Optional parameters that will show in red a text with the
	/// error message on top of the options table
	/// </summary>
	/// <param name="errorString"></param>
	void set_error(std::string errorString)
	{
		_error = errorString;
	}

#pragma region getters
	std::string get_configuration() {return _optionMap["-cf"];}
	std::string get_luafile() { return _optionMap["-l"]; }
	std::string get_script() { return _optionMap["-sc"]; }
	std::string get_help(){ return _optionMap["-h"]; }
	std::string get_terminal() { return _optionMap["-t"]; }
	std::string get_socket() { return _optionMap["-s"]; }
#pragma endregion

private:
	std::map<std::string, std::string> _optionMap{};
	std::string _error{};

	/// <summary>
	/// loads all flag definitions into the map
	/// </summary>
	void load_definitions() 
	{
		_optionMap.insert({ "-cf","EMPTY" }); // Configuration file
		_optionMap.insert({ "-l","EMPTY" }); // lua file script
		_optionMap.insert({ "-sc","EMPTY" }); // external script run by api
		_optionMap.insert({ "-h","EMPTY" }); // help
		_optionMap.insert({ "-t","EMPTY" }); // terminal
		_optionMap.insert({ "-s","EMPTY" }); // socket
	}

	/// <summary>
	/// Assigns all mapped values using the arguments statement.
	/// </summary>
	/// <param name="argc">No. of arguments</param>
	/// <param name="argv">Array of arguments</param>
	void set_definitions(int argc, char* argv[])
	{
        
		if (argc == 1) { _optionMap["-t"] = "TRUE"; }
		else if (argc > 1) 
		{
			for (int n1 = 1; n1 < argc; n1++) 
			{
				if (strcmp(argv[n1], "-h") == 0) { _optionMap["-h"] = "TRUE"; std::cout << "-h flag" << std::endl; }
				else if (strcmp(argv[n1], "-t") == 0) { _optionMap["-t"] = "TRUE"; }
				else if(_optionMap.count(argv[n1]))
				{
					std::cout << "option: " << argv[n1] << ", value: " << argv[n1 + 1] << std::endl;
					_optionMap[argv[n1]] = argv[n1 + 1];
					n1++;
				}
			}
		}

	}

	/// <summary>
	/// Loads textfile that contains the descriptions for all flags.
	/// flag | description | usage
	/// </summary>
	/// <returns></returns>
	std::vector<std::string> load_help_options() 
	{
		std::vector<std::string> optinList;

		try 
		{
			std::fstream optionsFile;
			optionsFile.open("Options.txt");

			if (optionsFile.is_open())
			{
				std::string var{ "" };
				while (!optionsFile.eof() && optionsFile.good())
				{
					std::getline(optionsFile, var);
					optinList.push_back(var);
				}
			}
			optionsFile.close();

		} 
		catch(const std::exception e)
		{
			//_error = e.what;
		}		

		return optinList;
	}
	
	/// <summary>
	/// Adds table of flags with description and usage to exiting
	/// ftxui element user interfacee.
	/// </summary>
	/// <param name="element">ftxui element</param>
	void load_help_option(ftxui::Elements& element)
	{
		using namespace ftxui;
		std::vector<std::string> options = load_help_options();
		std::vector<std::vector<ftxui::Element>> optionL;

		for each (std::string opty in options)
		{
			optionL.push_back(split(opty, ':'));
		}

		Table table(optionL);
		table.SelectColumn(0).Border(LIGHT);
		table.SelectRow(0).Decorate(bold);
		table.SelectRow(0).SeparatorVertical(LIGHT);
		table.SelectRow(0).Border(ftxui::DOUBLE);
		table.SelectColumn(1).DecorateCells(center);

		TableSelection rowItems = table.SelectRows(1, -1);
		rowItems.DecorateCellsAlternateRow(color(Color::Cyan));
		
		element.push_back(table.Render());
	}

	/// <summary>
	/// Splits a string and puts eacch string into an ftxui element
	/// </summary>
	/// <param name="x">string to be splitted</param>
	/// <param name="delim">char used for splitting</param>
	/// <returns>vector with paragraph elements</returns>
	std::vector<ftxui::Element> split(std::string& x, char delim = ' ')
	{
		x += delim; //includes a delimiter at the end so last word is also read
		std::vector<ftxui::Element> splitted;
		std::string temp = "";
		for (int i = 0; i < x.length(); i++)
		{
			if (x[i] == delim)
			{
				splitted.push_back(ftxui::paragraph(temp)); //store words in "splitted" vector
				temp = "";
				i++;
			}
			temp += x[i];
		}
		return splitted;
	}

};
/** @}*/
