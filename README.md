# AMFramework
The AMCF framework enables mapping of material properties by considering various parameters like composition, heat treatments, and others. It integrates with external CALPHAD software and databases using a unified scripting language, so that a single script can be utilized by any CALPHAD software. The framework offers rich visualization tools for convenient material design analysis.

## AMCore
AMCORE is a command-line interface that facilitates automation of tasks. It provides all backend functionalities without any graphical representation options.

## GUI
The graphical user interface offers all visualization tools needed for an efficient workflow. The GUI is built using c# and WPF .Net framework on visual studio.

# Installation
The framework is currently a windows desktop-only application.

## Requirements
This framework does not include or endorse any CALPHAD software, it also does not provide a valid license for any CALPHAD-based software. For now, only the MatCalc API has been implemented and in the future, there will be further options available.

### Microsoft:
[Visual C++ Redist](https://www.microsoft.com/de-de/download/details.aspx?id=48145)
[.Net](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)

### Matcalc
Matcalc offers a free trial, and for further information please refer to their [wepage](https://www.matcalc.at/) 

## Download the binaries

- [Matcalc API]() - (required)
- [Terminal application]()
- [WPF desktop application]()

## Setting up the terminal application
After downloading the binaries for ```MatCalc API``` and ```Terminal application``` run AMCore.exe to start the application.

You can also run AMCore from the terminal and the following flags are available:
|Flag|Option|usage|
|--|--|--|
|-h|Shows the help menu| -h |
|-t|Loads terminal (by default)| -t |
|-s|Open socket at port no.| -s "port number" |
|-l|Run LUA script| -l "filename"|

For more information please refer to the [wiki](https://github.com/SColibri/AMFramework/wiki)

## Setting up the WPF desktop application
After downloading the binaries for ```MatCalc API``` and ```WPF desktop application``` run AMFramework.exe to start the application.

### Initial setup
- In menu > configuration, add the path to the MatCalc API and the databases you plan to use.
- Make sure that the working directory is the one you want to use.

For more information on how to use, please refer to our [wiki page](https://github.com/SColibri/AMFramework/wiki)

# Features

- Visualization

- Data management

- Scripting

- Parallelism

# Documentation
More on how to build or contribute, please refer to the documentation [here](https://codedocs.xyz/SColibri/AMFramework)

