#pragma once
#include <iostream>
#include <string>
#include <vector>

namespace string_manipulators
{

    static std::vector<std::string> split_text(std::string const& csv_text, std::string delimiter)
    {
        size_t start;
        size_t end = 0;
        std::vector<std::string> out;

        while ((start = csv_text.find_first_not_of(delimiter, end)) != std::string::npos)
        {
            end = csv_text.find(delimiter, start);
            out.push_back(csv_text.substr(start, end - start));
        }

        return out;
    }

}