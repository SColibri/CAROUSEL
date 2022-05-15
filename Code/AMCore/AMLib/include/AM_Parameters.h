
#include <string>
#include <vector>

/** \addtogroup AMLib
 *  @{
 */

/// <summary>
/// Block parameters
/// </summary>
struct AM_Parameters {

public:
	enum class COMPOSITION_TYPE
	{
		WEIGHT
	};

	enum class TEMPERATURE_TYPE
	{
		CELSIUS
	};

	AM_Parameters()
	{
		_id = ID;
		_ID += 1;
		name = "new parameters";
		_temperature = 700;
		_compositionType = COMPOSITION_TYPE::WEIGHT;
		_temperatureType = TEMPERATURE_TYPE::CELSIUS;
		_noPrecipitateClasses = 0
	}

private:
	// General
	inline int static _ID{0};
	int _id;
	std::string name;

	//
	std::vector<std::string> _elements;
	std::vector<std::string> _phases;
	std::vector<double> _composition;
	COMPOSITION_TYPE _compositionType;
	double _temperature;
	TEMPERATURE_TYPE _temperatureType;

	int _noPrecipitateClasses;

};
/** @}*/