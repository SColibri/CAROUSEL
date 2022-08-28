#pragma once
#include <exception>
#include <string>

class COMMAND_exception : std::exception
{
public:

	COMMAND_exception(char* newMsg) :_what(newMsg) {}

	char* what() 
	{
		return _what;
	}

private:
	char* _what;
};