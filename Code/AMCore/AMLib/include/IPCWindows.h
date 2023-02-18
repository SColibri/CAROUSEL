#pragma once

#include <windows.h>
#include <string>
#include <thread>
#include <stdexcept>
#include <vector>
#include <iostream>
#include <sstream>
#include <strsafe.h>
#include <tchar.h>
#include "../exceptions/IPCPipeException.h"
#include "../interfaces/IIPCCommunication.h"
#include "../x_Helpers/string_manipulators.h"

#define BUFSIZE 4096 
#define PIPE_TIMEOUT 2000

namespace AMFramework
{
	namespace IPC
	{
		/// <summary>
		/// IPC implementation using windows socket communication
		/// </summary>
		class IPCWindows : public AMFramework::IPC::IIPCCommunication
		{
		private:
#pragma region Fields
			/// <summary>
			/// Path to application
			/// </summary>
			std::wstring _execPath;

			/// <summary>
			/// Buffer container that will the communication data
			/// </summary>
			CHAR _buffer[BUFSIZE];

			/// <summary>
			/// 
			/// </summary>
			std::string _stdout{ "" };

			/// <summary>
			/// Flag that specifies when the subprocess has finished
			/// executing the command.
			/// </summary>
			std::string _endFlag{ "" };

			/// <summary>
			/// Time in microseconds to wait for the subprocess to load
			/// </summary>
			int _subprocessWaitTime{ 1500 };

			/// <summary>
			/// Max read retries 
			/// </summary>
			int _retries{ 5 };

			/// <summary>
			/// Timeout used between intervals
			/// </summary>
			int _timeout{ 500 };

			/// <summary>
			/// Read pipe loop count
			/// </summary>
			int _retryCount{ 0 };

			/// <summary>
			/// Determines if a process is running and connected
			/// to our application.
			/// </summary>
			bool _isRunning{ false };

			/// <summary>
			/// Flag to specify if subprocess should be terminated when
			/// this class is disposed.
			/// </summary>
			bool _forceTerminate{ true };

			/// <summary>
			/// Process id
			/// </summary>
			int _processID{ -1 };

			/// <summary>
			/// Write Pipe Handle for Stdin
			/// </summary>
			HANDLE _inWPipe{ NULL };

			/// <summary>
			/// Read pipe handle for stdin
			/// </summary>
			HANDLE _inRPipe{ NULL };

			/// <summary>
			/// Write Pipe Handle for Stdin
			/// </summary>
			HANDLE _outWPipe{ NULL };

			/// <summary>
			/// Read pipe handle for stdin
			/// </summary>
			HANDLE _outRPipe{ NULL };

			/// <summary>
			/// SubProcess handle - winAPI
			/// </summary>
			HANDLE _process{ NULL };

			/// <summary>
			/// subprocess thread
			/// </summary>
			HANDLE _thread{ NULL };
#pragma endregion

		public:
#pragma region Constructor
			/// <summary>
			/// Default constructor, call Initialize for creating a new subprocess
			/// </summary>
			IPCWindows()
			{
				//Empty constructor, call Initialize
			}

			/// <summary>
			/// Constructor
			/// </summary>
			/// <param name="filename">subprocess filename</param>
			IPCWindows(std::wstring& filename)
			{
				memset(_buffer, 0, sizeof(_buffer));
				Initialize(filename);
			}

			/// <summary>
			/// Default destructor
			/// </summary>
			~IPCWindows()
			{
				_forceTerminate = true;

				// close child process
				Dispose();
			}
#pragma endregion

#pragma region Interface IIPCCommunication Implementation
			/// <summary>
			/// Initialize subprocess & communication
			/// </summary>
			/// <param name="filename"></param>
			virtual void Initialize(std::wstring& filename)
			{
				StartProcess(filename);
			}

			/// <summary>
			/// Send command to subprocess
			/// </summary>
			/// <returns></returns>
			virtual std::string& send_command(std::string command)
			{

				if (_isRunning)
				{
					// send command
					DWORD dwWritten{ 0 }, dwAvail{ 0 };
					bool bSuccess = WriteFile(_inWPipe, command.c_str(), command.size(), &dwWritten, NULL);

					// If command was written correctly into the pipe we then
					// proceed to listen to the pipe for the response, else
					// pipe to subprocess is broken (subprocess crash or other)
					if (bSuccess)
					{
						ClearSTDOUT();
						ReadPipeUntilFlag();
					}
					else
					{
						// Try dispose
						Dispose();
						throw new IPCPipeException("Lost connection to subprocess");
					}
				}
				else
				{
					// In Case IPC has not been initialized
					_stdout = "No subprocess running, please initialize";
				}

				return _stdout;
			}

			/// <summary>
			/// Disposes subprocess object
			/// </summary>
			virtual void Dispose()
			{
				if (_isRunning)
				{
					try
					{
						if (_forceTerminate)
						{
							// Closes all handles, this is not the best
							// practice and if this can be avoided select
							// closing the subprocess gracefully
							UINT uExitCode = -1;
							TerminateProcess(_process, uExitCode);
						}
						else
						{
							// To close the application gracefully we would have
							// to send the exit command to the subprocess, because
							// Matcalc has some personal issues with this, we just 
							// apply the force Terminate, but you could also implement
							// HERE the exit command before closing the handles.

							// Close handles
							CloseHandle(_inRPipe);
							CloseHandle(_inWPipe);
							CloseHandle(_outRPipe);
							CloseHandle(_outWPipe);

							CloseHandle(_process);
							CloseHandle(_thread);
						}

						_isRunning = false;
					}
					catch (const std::exception& e)
					{
						throw IPCPipeException(e.what());
					}

				}
			}
#pragma endregion

		private:
#pragma region Private helpers
			/// <summary>
			/// Start subprocess and create pipe 
			/// </summary>
			/// <param name="filename"></param>
			/// <returns></returns>
			int StartProcess(std::wstring& filename)
			{
				// If process is running 
				if (!_isRunning)
				{
					_execPath = filename;
					CreateSubProcess(_execPath);
					return 0;
				}
				else
				{
					return 1;
				}
			}

			/// <summary>
			/// Starts subprocess and initializes pipe communication using 
			/// winAPI
			/// </summary>
			/// <param name="filename">Path to application</param>
			/// <returns></returns>
			void CreateSubProcess(std::wstring filename)
			{
				// Pipe attributes
				SECURITY_ATTRIBUTES security_attrib;
				security_attrib.nLength = sizeof(SECURITY_ATTRIBUTES);
				security_attrib.bInheritHandle = TRUE;
				security_attrib.lpSecurityDescriptor = NULL;

				// Create pipes
				SetNamedPipes(_inRPipe, _inWPipe, security_attrib);
				SetNamedPipes(_outRPipe, _outWPipe, security_attrib);

				if (!SetHandleInformation(_outRPipe, HANDLE_FLAG_INHERIT, 0))
					ErrorExit(TEXT("Error SetHandleInformation"));

				if (!SetHandleInformation(_inWPipe, HANDLE_FLAG_INHERIT, 0))
					ErrorExit(TEXT("Error SetHandleInformation"));

				// Create child process
				CreateChildProcess(filename);
			}

			/// <summary>
			/// Creates pipes
			/// </summary>
			/// <param name="WHandle">Write handle</param>
			/// <param name="RHandle">Read Handle</param>
			/// <param name="saAttribute"></param>
			void SetNamedPipes(HANDLE& RHandle, HANDLE& WHandle, SECURITY_ATTRIBUTES& saAttribute)
			{
				if (!CreatePipe(&RHandle, &WHandle, &saAttribute, BUFSIZE))
					ErrorExit(TEXT("Error CreatePipe"));
			}

			/// <summary>
			/// Creates the child process
			/// </summary>
			void CreateChildProcess(std::wstring filename)
			{
				if (_isRunning) return;

				STARTUPINFOW startup_info;
				PROCESS_INFORMATION process_info;

				// Subprocess startup setup
				memset(&startup_info, 0, sizeof(startup_info));
				memset(&process_info, 0, sizeof(process_info));
				startup_info.cb = sizeof(startup_info);
				startup_info.hStdInput = _inRPipe;
				startup_info.hStdOutput = _outWPipe;
				startup_info.hStdError = _outWPipe;
				startup_info.dwFlags |= STARTF_USESTDHANDLES;

				// Try create new subprocess
				bool bSuccess = CreateProcessW(NULL,
					filename.data(),     // command line 
					NULL,          // process security attributes 
					NULL,          // primary thread security attributes 
					TRUE,          // handles are inherited 
					CREATE_NO_WINDOW,             // creation flags 
					NULL,          // use parent's environment 
					NULL,          // use parent's current directory 
					&startup_info,  // STARTUPINFO pointer 
					&process_info);  // receives PROCESS_INFORMATION 

				if (!bSuccess)
					ErrorExit(TEXT("CreateProcess"));
				else
				{
					// Close handles to the child process and its primary thread.
					// Some applications might keep these handles to monitor the status
					// of the child process, for example. 

					_process = process_info.hProcess;
					_processID = process_info.dwProcessId;
					CloseHandle(process_info.hThread);
					_isRunning = true;

					// Close handles to the stdin and stdout pipes no longer needed by the child process.
					// If they are not explicitly closed, there is no way to recognize that the child process has ended.

					CloseHandle(_outWPipe);
					CloseHandle(_inRPipe);

					ReadPipeUntilFlag();

					// Wait for the application to start
					std::this_thread::sleep_for(std::chrono::milliseconds(_subprocessWaitTime));
				}
			}

			/// <summary>
			/// Read stdout from subprocess in established pipe
			/// </summary>
			int ReadFromPipe()
			{
				int segmentCount{ 0 };
				char prevBuffer[BUFSIZE];
				memset(prevBuffer, 0, sizeof(prevBuffer));

				if (_isRunning)
				{
					// output
					std::stringstream stringStream;

					try
					{
						// read from pipe
						DWORD bytesRead;
						DWORD bytesAvailable = 0;

						PeekNamedPipe(_outRPipe, NULL, 0, NULL, &bytesAvailable, NULL);
						while (bytesAvailable > 0) {
							ReadFile(_outRPipe, _buffer, sizeof(BUFSIZE), &bytesRead, NULL);
							stringStream.write(_buffer, bytesRead);

							if (strcmp(prevBuffer, _buffer) == 0) break;
							strcpy(prevBuffer, _buffer);

							PeekNamedPipe(_outRPipe, NULL, 0, NULL, &bytesAvailable, NULL);
							segmentCount += 1;
						}

						// get string
						_stdout += stringStream.str();
					}
					catch (const std::exception& e)
					{
						throw IPCPipeException(e.what());
					}
				}
				else
				{
					_stdout = "Subprocess is not running";
				}

				return segmentCount;
			}

			/// <summary>
			/// Read pipe until flag was found
			/// </summary>
			void ReadPipeUntilFlag()
			{
				// set retry counter to zero
				_retryCount = 0;

				// Because the subprocess can take more time to respond
				// we loop until we have no further output from the
				// external application, this depends on the internals
				// of the subprocess
				while (_retryCount <= _retries)
				{
					// Read pipe, returns the amount of segments read from pipe
					int counter = ReadFromPipe();

					// Add to retry counter
					if (counter == 0)
					{
						_retryCount++;
					}
					else
					{
						_retryCount = 0;
					}

					// Since the subprocess might need a bigger interval in between
					// writes, we have to sleep, this really depends on the external
					// software. So remove if the external software implements a 
					// end flag or something similar.
					std::this_thread::sleep_for(std::chrono::microseconds(100));
				}
			}

			/// <summary>
			/// Clears captured output from subprocess
			/// </summary>
			void ClearSTDOUT()
			{
				_stdout = "";
			}

			// Format a readable error message, display a message box, 
			// and exit from the application.
			void ErrorExit(PTSTR lpszFunction)
			{
				LPVOID lpMsgBuf;
				LPVOID lpDisplayBuf;
				DWORD dw = GetLastError();

				FormatMessage(
					FORMAT_MESSAGE_ALLOCATE_BUFFER |
					FORMAT_MESSAGE_FROM_SYSTEM |
					FORMAT_MESSAGE_IGNORE_INSERTS,
					NULL,
					dw,
					MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT),
					(LPTSTR)&lpMsgBuf,
					0, NULL);

				lpDisplayBuf = (LPVOID)LocalAlloc(LMEM_ZEROINIT,
					(lstrlen((LPCTSTR)lpMsgBuf) + lstrlen((LPCTSTR)lpszFunction) + 40) * sizeof(TCHAR));
				StringCchPrintf((LPTSTR)lpDisplayBuf,
					LocalSize(lpDisplayBuf) / sizeof(TCHAR),
					TEXT("%s failed with error %d: %s"),
					lpszFunction, dw, lpMsgBuf);
				MessageBox(NULL, (LPCTSTR)lpDisplayBuf, TEXT("Error"), MB_OK);

				LocalFree(lpMsgBuf);
				LocalFree(lpDisplayBuf);
				
				throw new IPCPipeException("IPC module Error!");
			}
#pragma endregion

		};
	}
}