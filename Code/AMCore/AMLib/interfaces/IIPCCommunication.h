#pragma once
#include <iostream>
#include <string>


namespace AMFramework
{
	namespace IPC 
	{
		/// <summary>
		/// Interface IPC communication.
		/// 
		/// Create subprocess and establish communication so that sending
		/// and receiving data is possible.
		/// </summary>
		class IIPCCommunication
		{
		public:
			/// <summary>
			/// Initialize IPC communication using subprocess path
			/// </summary>
			/// <param name="filename">Path to executable</param>
			virtual void Initialize(std::wstring& filename) = 0;

			/// <summary>
			/// Send command to subprocess
			/// </summary>
			/// <returns>subprocess output</returns>
			virtual std::string& send_command(std::string command) = 0;

			/// <summary>
			/// Disposes subprocess object
			/// </summary>
			virtual void Dispose() = 0;
		};
	}
}