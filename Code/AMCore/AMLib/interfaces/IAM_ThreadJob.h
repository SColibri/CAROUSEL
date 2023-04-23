#pragma once

#include <string>

namespace AMFramework
{
	namespace Interfaces
	{
		/// <summary>
		/// Set of instructions for a thread to execute
		/// </summary>
		class IAM_ThreadJob
		{	
		public:
			/// <summary>
			/// Execute thread tasks
			/// </summary>
			/// <returns></returns>
			virtual int execute() = 0;

			/// <summary>
			/// Disposes the thread job object
			/// </summary>
			virtual void Dispose() = 0;

			/// <summary>
			/// Gets output string
			/// </summary>
			/// <returns></returns>
			virtual std::string get_output() = 0;
		};

	}
}

