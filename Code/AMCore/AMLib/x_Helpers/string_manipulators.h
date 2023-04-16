#pragma once
#include <iostream>
#include <iomanip>
#include <string>
#include <vector>
#include <algorithm>
#include <fstream>
#include <filesystem>
#include <iosfwd>
#include <sstream>

namespace string_manipulators
{
    /// <summary>
    /// Split csv string into std::vector<string>
    /// </summary>
    /// <param name="csv_text"></param>
    /// <param name="delimiter"></param>
    /// <returns></returns>
    static std::vector<std::string> split_text(std::string const& csv_text, std::string delimiter)
    {
        size_t start;
        size_t end = 0;
        std::vector<std::string> out;

        while ((start = csv_text.find_first_not_of(delimiter, end)) != std::string::npos)
        {
            // We check if other delimiters did not contain an empty value, if so
            // we add a string of length 0 into our output vector, this will return
            // a vector of correct length for the object
            if (start - end > 1) 
            {
                for(int n1 = 1; n1 < start - end; n1++)
                {
                    out.push_back("");
                }
            }

            end = csv_text.find(delimiter, start);
            out.push_back(csv_text.substr(start, end - start));
        }

        // Check if we did not jump to the last csv data (empty case)
        if (end == std::string::npos) return out;
        if (csv_text.size() - end > 1)
        {
            for (int n1 = 1; n1 < csv_text.size() - end; n1++)
            {
                out.push_back("");
            }
        }

        return out;
    }

    static size_t find_index_of_keyword(std::string const& text, std::string keyword) 
    {
        size_t index = text.find(keyword);
        return index;
    }

    static void replace_token_from_socketString(std::string& replaceThis)
    {
        std::replace(replaceThis.begin(), replaceThis.end(), '#',' ');
    }

    static void replace_token(std::string& replaceThis, std::string Oldvalue, std::string Newvalue)
    {   
        size_t index{ 0 };
        while ((index = replaceThis.find(Oldvalue)) != std::string::npos) {    //for each location where Hello is found
            replaceThis.replace(index, Newvalue.length(), Newvalue); //remove and replace from that position
        }
    }

    static void toCaps(std::string& replaceThis) 
    {
        std::transform(replaceThis.begin(), replaceThis.end(), replaceThis.begin(), ::toupper);
    }

    static bool isNumber(const std::string& str)
    {
        for (char const& c : str) {
            if (std::isdigit(c) == 0 && c != '-') return false;
        }
        return true;
    }

    static std::string get_numeric_value(std::string& stringy) 
    {
        std::string out{ "" };

        for (int n1 = 0; n1 < stringy.length(); n1++)
        {
            if(std::isdigit(stringy[n1]) || stringy[n1] == '.')
            {
                out += stringy[n1];
            }
        }

        return out;
    }

    static std::string double_to_string(double Value, int precision = 16) 
    {
        std::ostringstream decimalString;
        decimalString << std::fixed;
        decimalString << std::setprecision(precision);
        decimalString << Value;

        return decimalString.str();
    }

    static void remove_empty_entries(std::vector<std::string>& vectorItem) 
    {
        std::vector<std::string> tempVector;
        for (auto& item : vectorItem)
        {
            if (item.length() > 0) tempVector.push_back(item);
        }

        vectorItem.clear();
        for (auto& item : tempVector)
        {
            vectorItem.push_back(item);

        }
    }
#pragma region string_format
    /// <summary>
    /// Returns a string with the formatted value types usedin in the string formatter
    /// e.g output {%i,%g,%d}
    /// </summary>
    /// <param name="valCount">Number of variables</param>
    /// <param name="valueTypeChar">Type of value e.g. %d</param>
    /// <param name="delimiter">Char used as delimiter e.g. ","</param>
    /// <returns></returns>
    static std::string get_string_format_numeric_generic(int valCount, const std::string& valueTypeChar, const std::string& delimiter)
    {
        std::string out{};

        out = valueTypeChar;
        for (size_t n1 = 1; n1 < valCount; n1++)
        {
            out += delimiter + valueTypeChar;
        }

        return out;
    }
#pragma endregion

#pragma region trim
    static const std::string WHITESPACE = " \n\r\t\f\v";

    static std::string ltrim_byToken(const std::string& s, const std::string& token)
    {
        size_t indexToken = find_index_of_keyword(s, token) + token.size();
        if (indexToken == std::string::npos) return s;

        return s.substr(indexToken);
    }

    static std::string rtrim_byToken(const std::string& s, const std::string& token)
    {
        size_t indexToken = find_index_of_keyword(s, token) + token.size();
        if (indexToken == std::string::npos) return s;

        return s.substr(0, indexToken);
    }

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

#pragma region File
    /// <summary>
    /// Read file to end and return a vector of strings by line
    /// </summary>
    /// <param name="fileName">path to file</param>
    /// <returns></returns>
    static std::vector<std::string> read_file_to_end(std::string fileName)
    {
        if (!std::filesystem::exists(fileName)) return  std::vector<std::string>();
        
        std::ifstream iStream(fileName);
        std::vector<std::string> out;

        std::string tempString;
        while (std::getline(iStream, tempString)) {
            out.push_back(tempString);
        }

        iStream.close();
        return out;
    }

    static std::string read_file_content(std::string fileName)
    {
        std::string Result{""};

        if (!std::filesystem::exists(fileName)) return Result;

        std::ifstream filetxt(fileName);
        std::string tempString;
        while(std::getline(filetxt, tempString))
        {
            Result += tempString + "\n";
        }

        return Result;
    }

    static void write_to_file(std::string& fileName, std::string& content)
    {
        std::ofstream class_file;
        class_file.open(fileName);
        class_file << content;
        class_file.close();
    }


#pragma endregion

}