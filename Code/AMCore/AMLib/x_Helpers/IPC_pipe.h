#pragma  once
#include <windows.h>
#include <string>
#include <filesystem>
#include <fstream>

namespace IPC_pipe
{

	class IPC_pipe 
	{
	public:
		
		IPC_pipe(const std::string& filename) : _exec_path(filename)
		{

			_pipeSA.nLength = sizeof(SECURITY_ATTRIBUTES);
			_pipeSA.bInheritHandle = TRUE;
			_pipeSA.lpSecurityDescriptor = NULL;

			_createPipe_OUT = CreatePipe(&_handleRead_OUT, &_handleWrite_OUT, &_pipeSA, _bufferSize);
			if (_createPipe_OUT == FALSE || 
				SetHandleInformation(_handleRead_OUT, HANDLE_FLAG_INHERIT, 0) == FALSE)
			{
				_IPC_status = 1;
				_errorStatus = "Pipe OUT was not created";
				return;
			}

			_createPipe_IN = CreatePipe(&_handleRead_IN, &_handleWrite_IN, &_pipeSA, _bufferSize);
			if (_createPipe_IN == FALSE ||
				SetHandleInformation(_handleWrite_IN, HANDLE_FLAG_INHERIT, 0) == FALSE)
			{
				_IPC_status = 1;
				_errorStatus = "Pipe IN was not created";
				return;
			}

			STARTUPINFO _si;
			PROCESS_INFORMATION _pi;
			ZeroMemory(&_pi, sizeof(PROCESS_INFORMATION));
			ZeroMemory(&_si, sizeof(STARTUPINFO));
			_si.cb = sizeof(STARTUPINFO);
			_si.hStdError = _handleWrite_OUT;
			_si.hStdOutput = _handleWrite_OUT;
			_si.hStdInput = _handleRead_IN;
			_si.dwFlags |= STARTF_USESTDHANDLES;

			BOOL bSuccess = CreateProcess(NULL, "\"C:/Program Files/MatCalc 6/mcr.exe\" -i 7890", NULL, NULL, TRUE, 0, NULL, NULL, &_si, &_pi);
			if (bSuccess)
			{	
				//WaitForSingleObject(_pi.hProcess, INFINITE);
				CloseHandle(_pi.hProcess);
				CloseHandle(_pi.hThread);
				CloseHandle(_handleWrite_OUT);
				CloseHandle(_handleRead_IN);
			} 
			else
			{
				_IPC_status = 1;
				_errorStatus = "Error creating new process";
			}

			if(!std::filesystem::exists(std::filesystem::path(inputFile_path)))
			{
				std::ofstream ofs("pipeThis.txt", std::ofstream::out | std::ofstream::trunc);
				ofs.close();
			}

			//DWORD dwRead;
			//bSuccess = ReadFile(_handleRead_OUT, _buffer, _bufferSize, &dwRead, NULL);

			COMMTIMEOUTS llpTimeouts;
			memset(&llpTimeouts, 0, sizeof(COMMTIMEOUTS));
			llpTimeouts.ReadIntervalTimeout = MAXWORD;
			llpTimeouts.ReadTotalTimeoutConstant = 0;
			llpTimeouts.ReadTotalTimeoutMultiplier = 0;

			BOOL setTimeout = SetCommTimeouts(_handleRead_OUT, &llpTimeouts);

			ReadFromPipe();
		}

		std::string& send_command(std::string command_string)
		{
			//if (_IPC_status != 0) return "IPC nor ready! ";

			std::ofstream ofs("pipeThis.txt");
			ofs << command_string;
			ofs.close();

			bool ourty = std::filesystem::exists(std::filesystem::path("pipeThis.txt"));

			std::ifstream ifs("pipeThis.txt");
			std::string outfromT{""};
			bool isOpenT = ifs.is_open();

			while(std::getline(ifs, outfromT))
			{
				bool stophere{ true };
			}

			ifs.close();

			_handleInputFile = CreateFile(
				"pipeThis.txt",
				GENERIC_READ,
				0,
				NULL,
				OPEN_EXISTING,
				FILE_ATTRIBUTE_READONLY,
				NULL);

			

			if (_handleInputFile == INVALID_HANDLE_VALUE)
				bool stophere = true;
			

			WriteToPipe();
			ReadFromPipe();

			return result;
		}



	private:
		
		DWORD _exitCode = -1;

		const std::string _exec_path;
		std::string result {""};
		std::string inputFile_path{ "pipeThis.txt" };
		
		SECURITY_ATTRIBUTES _pipeSA;
		BOOL _createPipe_OUT;
		BOOL _createPipe_IN;
		HANDLE _handleRead_OUT = NULL;
		HANDLE _handleWrite_OUT = NULL;
		HANDLE _handleRead_IN = NULL;
		HANDLE _handleWrite_IN = NULL;

		HANDLE _handleInputFile = NULL;

		char _buffer[1024];
		DWORD _bufferSize = sizeof(_buffer);

		BOOL _readFile;
		BOOL _writeFile;
		DWORD _noBytesWrite;
		DWORD _noBytesRead;

		int _IPC_status{ 0 };
		std::string _errorStatus;


		void WriteToPipe(void)

			// Read from a file and write its contents to the pipe for the child's STDIN.
			// Stop when there is no more data. 
		{
			DWORD dwRead, dwWritten;
			BOOL bSuccess = FALSE;
			std::fill_n(_buffer, _bufferSize, '\0');

			for (;;)
			{
				bSuccess = ReadFile(_handleInputFile, _buffer, _bufferSize, &dwRead, NULL);
				if (!bSuccess || dwRead == 0) break;

				bSuccess = WriteFile(_handleWrite_IN, _buffer, dwRead, &dwWritten, NULL);
				if (!bSuccess) break;
			}

			// Close the pipe handle so the child process stops reading. 

			//if (!CloseHandle(_handleWrite_IN))
			//	ErrorExit(TEXT("StdInWr CloseHandle"));
		}

		void ReadFromPipe(void)

			// Read output from the child process's pipe for STDOUT
			// and write to the parent process's pipe for STDOUT. 
			// Stop when there is no more data. 
		{
			DWORD dwRead, dwWritten, dwAvail;
			BOOL bSuccess = FALSE;
			HANDLE hParentStdOut = GetStdHandle(STD_OUTPUT_HANDLE);
			std::fill_n(_buffer, _bufferSize, '\0');

			for (;;)
			{
				PeekNamedPipe(_handleRead_OUT, 0, 0, 0, &dwAvail, 0);
				if (dwAvail == 0) break;

				bSuccess = ReadFile(_handleRead_OUT, _buffer, _bufferSize, &dwRead, NULL);
				if (!bSuccess || dwRead == 0) break;

				result += _buffer;

				bSuccess = WriteFile(hParentStdOut, _buffer,
					dwRead, &dwWritten, NULL);
				if (!bSuccess) break;
			}
		}

		void ErrorExit(PTSTR lpszFunction)

			// Format a readable error message, display a message box, 
			// and exit from the application.
		{
			LPVOID lpMsgBuf;
			LPVOID lpDisplayBuf;
			DWORD dw = GetLastError();

			LocalFree(lpMsgBuf);
			LocalFree(lpDisplayBuf);
			ExitProcess(1);
		}

	};

}