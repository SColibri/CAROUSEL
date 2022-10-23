#pragma once
#include <string>
#include <vector>
#include "../AM_Triangle.h"

using namespace modeling;

namespace stl_reader
{
    struct AM_STL {
        std::string name;
        std::vector<AM_Triangle> triangles;

        AM_STL(std::string namep) : name(namep) {}

        /// <summary>
        /// Adds a triangle based on the 4 parameters that defines a triangle and its surface
        /// </summary>
        void Add_triangle(AM_Point normal, AM_Point v1, AM_Point v2, AM_Point v3) 
        {
            triangles.push_back(AM_Triangle(normal, v1, v2, v3));
        }
    };
}