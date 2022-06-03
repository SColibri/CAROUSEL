#pragma once
#include <vector>
#include "AM_pixel_layer.h"
#include "Database_implementations/Data_stuctures/DBS_Project.h"
class AM_pixel_object 
{
public:
	void AM_pixel_object(DBS_Project* project) 
	{
	
	}



private:
	DBS_Project* _project{nullptr};
	std::vector<AM_pixel_layer> _layers;

	void load_Layers() {}
};
