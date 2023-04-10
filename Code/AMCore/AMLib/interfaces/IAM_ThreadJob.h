#pragma once

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
		};

	}
}

