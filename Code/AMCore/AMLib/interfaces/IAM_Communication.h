#pragma once

namespace AMFramework 
{
	namespace Interfaces
	{
		/// <summary>
		/// Communication channel interface for receiving and sending data / commands
		/// </summary>
		class IAM_Communication
		{
		public:
			/// <summary>
			/// Send command to subprocess
			/// </summary>
			/// <returns>subprocess output</returns>
			virtual std::string& send_command(const std::string& command) = 0;

			/// <summary>
			/// Disposes subprocess object
			/// </summary>
			virtual void Dispose() = 0;

			/// <summary>
			/// Return true if communication is still ongoing
			/// </summary>
			virtual const bool& isRunning() = 0;
		};
	}
}