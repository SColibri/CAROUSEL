#pragma once
#include <iostream>
#include <string>
#include <vector>
#include <algorithm>

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

    static size_t find_index_of_keyword(std::string const& text, std::string keyword) 
    {
        size_t index = text.find(keyword);
        return index;
    }

#pragma region trim
    static const std::string WHITESPACE = " \n\r\t\f\v";

    static std::string ltrim_whiteSpace(const std::string& s)
    {
        size_t start = s.find_first_not_of(WHITESPACE);
        return (start == std::string::npos) ? "" : s.substr(start);
    }

    static std::string rtrim_whiteSpace(const std::string& s)
    {
        size_t end = s.find_last_not_of(WHITESPACE);
        return (end == std::string::npos) ? "" : s.substr(0, end + 1);
    }

    static std::string trim_whiteSpace(const std::string& s) {
        return rtrim_whiteSpace(ltrim_whiteSpace(s));
    }

    // No need to re-invent the wheel for trimming whitespaces.
    // code snippet from: https://www.techiedelight.com/trim-string-cpp-remove-leading-trailing-spaces/
#pragma endregion

}