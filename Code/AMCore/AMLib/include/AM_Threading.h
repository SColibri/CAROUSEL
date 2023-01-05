#pragma once
#include <vector>

namespace AMThreading
{
	/// <summary>
	/// Returns a vector with the amount of threads to be used and their workload distribution, 
	/// there might be uneven distributions
	/// </summary>
	/// <param name="MaxThreads"></param>
	/// <param name="NoIterations"></param>
	/// <returns></returns>
	static std::vector<int> thread_workload_distribution(int MaxThreads, int NoIterations)
	{
		std::vector<int> out;
		int useThreads = MaxThreads;
		int splitSize = NoIterations / useThreads;

		// check if combination is optimal (at least one workload iteration for each thread!)
		while (splitSize == 0)
		{
			if(useThreads > 0)
			{
				useThreads -= 1;
			}
			else 
			{
				// No combination was found that uses more than one thread!
				out.push_back(NoIterations);
				return out;
			}

			splitSize = NoIterations / useThreads;
		}

		// check if combination is unbalanced
		int remainder = NoIterations % useThreads;

		// Distribute workload until all iterations are covered
		int workSum{ 0 };
		for (int n1 = 0; n1 < useThreads; n1++)
		{
			if (workSum + splitSize <= NoIterations)
			{
				if (n1 == useThreads - 1)
				{
					out.push_back(splitSize + remainder);
					workSum += splitSize + remainder;
				}
				else
				{
					out.push_back(splitSize);
					workSum += splitSize;
				}
			}
			else if (workSum + remainder <= NoIterations)
			{
				out.push_back(remainder);
				workSum += remainder;
			}
			else
			{
				break;
			}
		}

		return out;
	}

}