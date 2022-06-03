#pragma once
#include <vector>
#include "AM_pixel_parameters.h"
#include "Database_implementations/Data_stuctures/DBS_Project.h"

class AM_pixel_layer
{
public:
	void AM_pixel_layer(DBS_Project* project, int zPosition)
	{
	
	}

private:
	double _z_position{ -1.0 }; // Current layer position
	double _height{ 0.0 }; // Layer height
	DBS_Project* _project{ nullptr }; // Project
	std::vector<AM_pixel_parameters> _pixels; // pixels of layer

};