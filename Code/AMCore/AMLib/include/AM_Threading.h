#pragma once
#include <vector>
#include <thread>
#include "../interfaces/IAM_ThreadJob.h"
#include "../interfaces/IAM_ThreadJob.h"
#include "callbackFunctions/ErrorCallback.h"

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
			if (useThreads > 0)
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

namespace AMFramework
{
	namespace Threading
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
				if (useThreads > 0)
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

		/// <summary>
		/// Function method for running a CalculationsThreadJob that corresponds to
		/// the tasks that one thread has to handle
		/// </summary>
		/// <param name="threadJob"></param>
		static void run_thread_job(AMFramework::Interfaces::IAM_ThreadJob* threadJob)
		{
			threadJob->execute();
		}

		/// <summary>
		/// Blocking operation, runs all tasks in parallel
		/// </summary>
		/// <param name="threadJob"></param>
		static void run_thread_jobs_in_parallel(std::vector<AMFramework::Interfaces::IAM_ThreadJob*> jobList)
		{
			// Thread list
			std::vector<std::thread> threadList;
			
			try
			{
				// Start threads
				for (int i = 0; i < jobList.size() - 1; i++)
				{
					threadList.push_back(std::thread(run_thread_job, jobList[i]));
				}

				// Execute job in current thread
				jobList[jobList.size() - 1 ]->execute();

				// Wait for all threads to finish
				for (int n1 = 0; n1 < threadList.size(); n1++)
				{
					threadList[n1].join();
				}
			}
			catch (const std::exception& e)
			{
				std::string errorMessage = "run_parallel reported an error while executing one or more tasks:\n" + std::string(e.what());
				AMFramework::Callback::ErrorCallback::TriggerCallback(&errorMessage[0]);
			}
		}
	}
}