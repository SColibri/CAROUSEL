# AMFramework
Advanced Material Computational Framework (AMCF) is a framework developed for simulating and mapping the material properties during advanced manufacturing. It is specially designed for a high-throughput screening of different chemical compositions and processing parameters. The approach is based on the calculation of phase diagrams known as the CALPHAD. The solidification process is modelled using the Scheil-Gulliver approach which gives the information about primary precipitates for further calculation of precipitation kinetics in solid state. It is possible to design speical heat treatments, test different chemical compositions and track the history of a single precipitation phase. The AMCF offers also various tools for analizing the generated data and making  decisions on the best material-processing parameters combination according to the applied criteria (e.g. strength).

The framework is using the external software MatCalc as the CALPHAD implementation. It is important to keep in mind that the AMCF framework does not provide any license to the MatCalc software. To be able to use the framework, the availability of the MatCalc license is required at the user side. Alternatively, one can use the MatCalc free version which is restricted to 3 chemical elements and free databases. 

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
    <img src="Design/img/vis_mapChart.png" alt="AL" title="Example AL alloy" style="border-radius: 1%;"/> 
</div>


## Data management

Data is saved in a SQL database, providing fast access and storage of all calculations. This enables users to quickly retrieve information and enjoy several other benefits.

## Scripting

The framework provides scripting capabilities using LUA as the base language, which offers access to various mapping options and the ability to create complex simulation parameters that would be difficult to automate otherwise. This makes automation more efficient and straightforward.

## Parallelism

Our thread manager utilizes all available resources to ensure you receive results as quickly as possible.


# Documentation
More on how to build or contribute, please refer to the documentation [here](https://codedocs.xyz/SColibri/AMFramework)

