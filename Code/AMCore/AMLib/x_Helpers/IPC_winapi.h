#pragma once

#include <windows.h>
#include <iostream>
#include <thread>
#include <chrono>
#include <atltime.h>
#include "../interfaces/IAM_Communication.h"
#include "../x_Helpers/string_manipulators.h"

#define BUFSIZE 4096 
using namespace std;

class IPC_winapi : public AMFramework::Interfaces::IAM_Communication
{
public:
    struct subprocess_info {
        HANDLE child_in_r = NULL;
        HANDLE child_in_w = NULL;
        HANDLE child_out_r = NULL;
        HANDLE child_out_w = NULL;
        HANDLE proc;
    };

    IPC_winapi() 
    {
        //initialize(L"C:/Program Files/MatCalc 6/mcc.exe");
    }

    IPC_winapi(wchar_t* cmdexec)
    {
        initialize(cmdexec);
    }

    IPC_winapi(std::wstring& cmdexec)
    {
        initialize(cmdexec);
    }
    
    ~IPC_winapi() 
    {
        CloseHandles(&process_info);
    }
    
    void initialize(std::wstring commandExec)
    {
        if (_isRunning) return;
        _execPath = commandExec;
        create_subprocess(_execPath.data(), &process_info);
        std::this_thread::sleep_for(std::chrono::seconds(3));
        ReadFromPipe(&process_info);
    }

    virtual string& send_command(const string& commandString) override
    {
        //flush_pipe(); // check if pipe is empty
        DWORD dwWritten{0}, dwAvail{0};
        WriteFile(process_info.child_in_w, commandString.c_str(), commandString.size(), &dwWritten, NULL);
        if (commandString.compare("exit\r\n") == 0) dwWritten = 0;

        if(dwWritten == 0)
        {
            _stdout = "Error: command was not sent";
            _isRunning = false;
            CloseHandles(&process_info);
        }
        else 
        {
            for(;;)
            {
                int outTest = PeekNamedPipe(process_info.child_out_r, 0, 0, 0, &dwAvail, 0);
                if (dwAvail != 0) break;
                std::this_thread::sleep_for(std::chrono::microseconds(150));
            }
            
            ReadFromPipe(&process_info);
        }
        
        _stdout += "\nExit Code : 0 ";
        return  _stdout;
    }    

    const bool& isRunning() { return _isRunning; }
    void set_endflag(string flag) { _endflag = flag; }
    string& get_endflag() { return _endflag; }

private:
    subprocess_info process_info;
    std::wstring _execPath;
    CHAR _buffer[BUFSIZE];
    string _stdout{""};
    string _endflag{ "" };
    bool _isRunning{ false };
    bool _isDisposed{ false };

    int create_subprocess(wchar_t* cmdexec, struct subprocess_info* info)
    {
        static int pipe_serial_no = 0;
        wchar_t pipe_name[50];
        SECURITY_ATTRIBUTES security_attrib;
        STARTUPINFOW startup_info;
        PROCESS_INFORMATION process_info;

        security_attrib.nLength = sizeof(security_attrib);
        security_attrib.bInheritHandle = TRUE;
        security_attrib.lpSecurityDescriptor = NULL;

        if (CreatePipe(&info->child_in_r, &info->child_in_w, &security_attrib, 0) == 0) {

            return -1;
        }

        _snwprintf(pipe_name, sizeof(pipe_name), L"\\\\.\\pipe\\ncat-%d-%d", GetCurrentProcessId(), pipe_serial_no);

        info->child_out_r = CreateNamedPipeW(pipe_name,
            PIPE_ACCESS_INBOUND ,
            PIPE_TYPE_BYTE | PIPE_WAIT, 1, BUFSIZE, BUFSIZE, NMPWAIT_USE_DEFAULT_WAIT, &security_attrib);

        if (info->child_out_r == 0) {
            CloseHandle(info->child_in_r);
            CloseHandle(info->child_in_w);
            return -1;
        }

        info->child_out_w = CreateFileW(pipe_name,
            GENERIC_WRITE, 0, &security_attrib, OPEN_EXISTING,
            FILE_ATTRIBUTE_NORMAL , NULL);

        if (info->child_out_w == 0) {
            CloseHandle(info->child_in_r);
            CloseHandle(info->child_in_w);
            CloseHandle(info->child_out_r);
            return -1;
        }

        pipe_serial_no++;

        SetHandleInformation(info->child_in_w, HANDLE_FLAG_INHERIT, 0);
        SetHandleInformation(info->child_out_r, HANDLE_FLAG_INHERIT, 0);

        memset(&startup_info, 0, sizeof(startup_info));
        startup_info.cb = sizeof(startup_info);
        startup_info.hStdInput = info->child_in_r;
        startup_info.hStdOutput = info->child_out_w;
        startup_info.hStdError = GetStdHandle(STD_ERROR_HANDLE);
        startup_info.dwFlags |= STARTF_USESTDHANDLES;

        memset(&process_info, 0, sizeof(process_info));

        std::wstring cmdArgslistSetChannel = cmdexec;

        if (CreateProcessW(NULL, &cmdArgslistSetChannel[0], NULL, NULL, TRUE, CREATE_NO_WINDOW, NULL, NULL, &startup_info, &process_info) == 0) {
            CloseHandle(info->child_in_r);
            CloseHandle(info->child_in_w);
            CloseHandle(info->child_out_r);
            CloseHandle(info->child_out_w);
            return -1;
        }


        CloseHandle(process_info.hThread);
        info->proc = process_info.hProcess;
        _isRunning = true; // process is running
        return process_info.dwProcessId;
    }

    void ReadFromPipe(subprocess_info* si)
    {
        DWORD dwRead{ 0 }, dwWritten{ 0 }, dwAvail{ 0 }, dwleft{ 0 };
        BOOL bSuccess = FALSE;
        HANDLE hParentStdOut = GetStdHandle(STD_OUTPUT_HANDLE);
        _stdout = "";

        
        // read all data from pipe
        for (;;)
        {
            // check if pipe has data before calling
            PeekNamedPipe(si->child_out_r, 0, 0, 0, &dwAvail, &dwleft);
            if (dwAvail == 0 &&
                string_manipulators::find_index_of_keyword(_stdout.substr(_stdout.size() - dwRead, _stdout.size()), _endflag) != string::npos) break;
            
            WaitForInputIdle(process_info.child_in_w, INFINITE);
            if (dwAvail == 0) { std::this_thread::sleep_for(std::chrono::microseconds(20000)); continue; }
            
            // read pipe content
            fill(std::begin(_buffer), std::end(_buffer), '\0');
            bSuccess = ReadFile(si->child_out_r, _buffer, BUFSIZE, &dwRead, NULL);
            if (dwRead == 0) break;

            string s(_buffer, dwRead);  
            _stdout += string(_buffer, dwRead);

            if (_stdout.size() > 20) { dwRead = 19; }
        }

    }

    void flush_pipe()
    {
        do
        {
            ReadFromPipe(&process_info);
            if(_stdout.size() > 0) std::this_thread::sleep_for(std::chrono::seconds(3));
        } while (_stdout.size() > 0);
        
    }

    void CloseHandles(subprocess_info* si)
    {
        try
        {
            UINT uExitCode = 0;
            TerminateProcess(si->proc, uExitCode);
            //CloseHandle(si->child_in_r);
            //CloseHandle(si->child_in_w);
            //CloseHandle(si->child_out_r);
            //CloseHandle(si->child_out_w);
            // CloseHandle(si->proc);
        }
        catch (const std::exception&)
        {

        }
    }

    /// <summary>
    /// Dispose method (IAM_Communication.h)
    /// </summary>
    virtual void Dispose() override
    {
        if (!_isDisposed)
        {
            CloseHandles(&process_info);
        }
    }


};
