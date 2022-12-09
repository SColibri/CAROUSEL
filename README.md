# AMFramework
Is a framework used for simulating and mapping material properties when subjected to heat treatments and/or change in element composition. Additionally the framework offers various tools for analizing all generated data nodes.

Calculations are made using external CALPHAD software (e.g. Matcalc), and it is important to keep in mind that this framework does not provide any license.

# Installation
The framework is currently windows only.

## Windows
### AMCore
AMCore is a terminal application that handles all the backend functionality. Additionally, AMCore has to link to a library that specifies how to communicate with the external CALPHAD software by using the interface IAM_API. The library is linked dynamically, thus you don't have to rebuild the whole project to use the new implementation.

Create by running CMake, copy the [binaries](https://syncandshare.lrz.de/getlink/fiFESSiL5grTWwaT4a9R5K/) to any directory.

### GUI
The graphical user interface was done using WPF .NET framework, you can either download the binaries from [here](https://syncandshare.lrz.de/getlink/fiFESSiL5grTWwaT4a9R5K/) or build the project yourself using visual studio.

### Requirements
Besides being a windows only application, the framework uses and external CALPHAD software for running all simulations. For now, only the Matcalc library(IAM_API) is available, however, this will be extended in the future for other CALPHAD software.

Microsoft:
[Visual C++ Redist](https://www.microsoft.com/de-de/download/details.aspx?id=48145)
[.Net](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)

#### Matcalc
Matcalc offers a free trial, and for further information please refer to the Matcalc wepage https://www.matcalc.at/

# Features

## LUA scripting

### OOP LUA

### Autocomplete feature

## Plotting

### Project Map
### Case view
### Heat treatment

## Database

## Parallelism

