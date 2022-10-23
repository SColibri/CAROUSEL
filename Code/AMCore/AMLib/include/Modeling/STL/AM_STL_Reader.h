#pragma once
#include <string>
#include <vector>
#include <iostream>
#include <cassert>
#include <fstream>
#include "AM_STL.h"
#include "../AM_Point.h"
#include "../AM_Triangle.h"

using namespace modeling;

/// <summary>
/// STL file format can be found on wikipedia: https://en.wikipedia.org/wiki/STL_(file_format)
/// </summary>
namespace stl_reader
{
    /// <summary>
    /// For STL files floating numbers are defined using 4 bites as defined in the wiki page
    /// </summary>
    static float parse_float(std::ifstream& s) {
        char f_buf[sizeof(float)];
        s.read(f_buf, 4);
        float* fptr = (float*)f_buf;
        return *fptr;
    }

    /// <summary>
    /// Helper that parses through the point structure 
    /// </summary>
    static AM_Point parse_point(std::ifstream& s) {
        float x = parse_float(s);
        float y = parse_float(s);
        float z = parse_float(s);
        return AM_Point(x, y, z);
    }

    /// <summary>
    /// Parser, returns a AM_STL object that contains all triangles an normal faces
    /// </summary>
    static AM_STL parse_stl(const std::string& stl_path) {

        // Open stl file (binary)
        std::ifstream stl_file(stl_path.c_str(), std::ios::in | std::ios::binary);
        if (!stl_file) {
            std::cout << "ERROR: COULD NOT READ FILE" << std::endl;
            assert(false);
        }

        // From file format, header has 80 bytes, No. triangles 4 bytes and each
        // triangle has 50 bytes (3 vertexes, 1 normal vector and an attribute count of 2 bytes)
        char header_info[80] = "";
        char n_triangles[4];
        stl_file.read(header_info, 80);
        stl_file.read(n_triangles, 4);

        // create stl structure and header info to it
        std::string h(header_info);
        AM_STL stlObject(h);

        // cast number of triangles into unsigned int using the pointer method
        unsigned int* r = (unsigned int*)n_triangles;

        for (unsigned int i = 0; i < *r; i++) {
            
            // Triangle definitions
            AM_Point normal = parse_point(stl_file); // <- 12 bytes
            AM_Point v1 = parse_point(stl_file); // <- 12 bytes
            AM_Point v2 = parse_point(stl_file); // <- 12 bytes
            AM_Point v3 = parse_point(stl_file); // <- 12 bytes

            // Add triangle to stl object
            stlObject.Add_triangle(normal, v1, v2, v3);

            char attributeCount[2]; // <- attribute count 2 bytes, we don't need this for now
            stl_file.read(attributeCount, 2);
        }
        return stlObject;
    }
}