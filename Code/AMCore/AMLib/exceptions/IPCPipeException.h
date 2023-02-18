#pragma once
#include <exception>
#include <string>
#include <stdexcept>

namespace AMFramework
{
	namespace IPC
	{
		/// <summary>
		/// Exception when creating a pipe connection to subprocess
		/// was not possible
		/// </summary>
		class IPCPipeException : public std::exception
		{
		private:
			std::string m_message{""};

		public:
			IPCPipeException(const std::string & message) : m_message(message) {}

			virtual const char* what() const noexcept {
				return m_message.c_str();
			}
		};
	}
}
