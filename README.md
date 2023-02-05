# AMFramework
The AMCF framework enables mapping of material properties by considering various parameters like composition, heat treatments, and others. It integrates with external CALPHAD software and databases using a unified scripting language, so that a single script can be utilized by any CALPHAD software. The framework offers rich visualization tools for convenient material design analysis.

## AMCore
AMCORE is a command-line interface that facilitates automation of tasks. It provides all backend functionalities without any graphical representation options.

## GUI
The graphical user interface offers a full set of visualization tools to enhance the material design process. It is built using C# and WPF .Net framework on Visual Studio.

# Windows
The framework is currently only available for the Windows desktop, however, it is planned to expand its availability to other platforms in the future.

## Requirements

- CALPHAD software: The AMCF framework does not include or promote any CALPHAD based software, and does not provide a license. Currently, only the [MatCalc](https://www.matcalc.at/) API is supported, but more options will be added in the future.

## Install

- Download the [MatCalc interpreter API]()
- Download the [WPF desktop application]()
- Extract all files and place them in your desired directory.
- The application is now ready for use and you just need to follow the [configuration guide](https://github.com/SColibri/AMFramework/wiki) to get started.

# Features

## Visualization

We offer more than just plotting visualization tools. You can interact with the data, create custom queries, and access many other useful features.

<div style="width:70%; display: block; margin-left: auto; margin-right: auto; box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19);">
    <img src="design/img/vis_mapChart.png" alt="AL" title="Example AL alloy" style="border-radius: 1%;"/> 
</div>


## Data management

Data is saved in a SQL database, providing fast access and storage of all calculations. This enables users to quickly retrieve information and enjoy several other benefits.

## Scripting

The framework provides scripting capabilities using LUA as the base language, which offers access to various mapping options and the ability to create complex simulation parameters that would be difficult to automate otherwise. This makes automation more efficient and straightforward.

## Parallelism

Our thread manager utilizes all available resources to ensure you receive results as quickly as possible.


# Documentation
More on how to build or contribute, please refer to the documentation [here](https://codedocs.xyz/SColibri/AMFramework)

