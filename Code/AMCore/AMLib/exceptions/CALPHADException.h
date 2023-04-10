#pragma once
#include <exception>
#include <string>
#include <stdexcept>

namespace AMFramework
{
	namespace Exceptions 
	{
		/// <summary>
		/// Exception class that is thrown whenever an error with the external
		/// CALPHAD software happens.
		/// </summary>
		class CALPHADException : public std::exception
		{
		private:
			std::string _message{ "" };

		public:
			CALPHADException(const std::string & message) : _message(message){}

			virtual const char* what() const noexcept {
				return _message.c_str();
			}
		};

	}
}