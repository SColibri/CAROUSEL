#pragma once

#undef UNICODE
#define WIN32_LEAN_AND_MEAN
#include <windows.h>
#include <winsock2.h>
#include <ws2tcpip.h>
#include <stdlib.h>
#include <stdio.h>
#include <thread>
#include "../interfaces/IAM_API.h"

// Need to link with Ws2_32.lib
#pragma comment (lib, "Ws2_32.lib")
// #pragma comment (lib, "Mswsock.lib")

#define DEFAULT_BUFLEN 512
#define DEFAULT_PORT "27015"

class AM_Server 
{
private:
	std::string _port{ DEFAULT_PORT };
	std::thread _asyncT;
	std::vector<std::thread> _asyncH{ 5 };
	std::vector<std::string> _openPorts{ 5,"" };
	std::vector<std::string> _errorPorts{ 5,"" };

public:
	AM_Server(IAM_API* implementation):_implementation(implementation) {}

	AM_Server(IAM_API* implementation, std::string& port) :
		_implementation(implementation), _port(port){}

	void init()
	{
		initialize(_port);
	}

	void init_async(std::string& port)
	{
		int avail = get_available_slot();
		if (avail == -1) return; // TODO: display warning no available connections
		
		_port = port;
		_openPorts[avail] = _port;
		_asyncH.push_back(std::thread([this] { this->initialize(_port); }));
	}


private:
	WSADATA _wsaData;
	SOCKET _listenSocket{ INVALID_SOCKET };
	SOCKET _clientSocket{ INVALID_SOCKET };
	int _serverStatus{-1};
	int iSendResult;
	char recvbuf[DEFAULT_BUFLEN];
	int recvbuflen = DEFAULT_BUFLEN;

	addrinfo* _result = NULL;
	addrinfo _hints;

	bool _workListen{ false };
	IAM_API* _implementation{nullptr};

	int get_available_slot()
	{
		for (int i = 0; i < _openPorts.size(); i++)
		{
			if (_openPorts[i].length() == 0) return i;
		}
		return -1;
	}

	void remove_port_from_list(std::string port)
	{
		for (size_t i = 0; i < _openPorts.size(); i++)
		{
			if (std::strcmp(_openPorts[i].c_str(), port.c_str()) == 0) 
			{
				_openPorts[i] = "";
				break;
			}
		}
	}

	int& startup_wsa()
	{
		_serverStatus = WSAStartup(MAKEWORD(2, 2), &_wsaData);
		return _serverStatus;
	}

	void initialize_hints()
	{
		ZeroMemory(&_hints, sizeof(_hints));
		_hints.ai_family = AF_INET;
		_hints.ai_socktype = SOCK_STREAM;
		_hints.ai_protocol = IPPROTO_TCP;
		_hints.ai_flags = AI_PASSIVE;
	}

	int& get_address_info(std::string port)
	{
		_serverStatus = getaddrinfo(NULL, port.c_str(), &_hints, &_result);
		if (_serverStatus != 0) WSACleanup();
		return _serverStatus;
	}

	SOCKET& open_server_socket()
	{
		_listenSocket = socket(_result->ai_family, _result->ai_socktype, _result->ai_protocol);
		
		if (_listenSocket == INVALID_SOCKET) {
			freeaddrinfo(_result);
			WSACleanup();
		}

		return _listenSocket;
	}

	int& bind_to_socket()
	{
		_serverStatus = ::bind(_listenSocket, _result->ai_addr, (int)_result->ai_addrlen);
		
		freeaddrinfo(_result);
		if (_serverStatus == SOCKET_ERROR) {
			closesocket(_listenSocket);
			WSACleanup();
		}

		return _serverStatus;
	}

	int& listen_to_socket_port()
	{
		_serverStatus = listen(_listenSocket, SOMAXCONN);
		
		if (_serverStatus == SOCKET_ERROR) {
			closesocket(_listenSocket);
			WSACleanup();
		}

		return _serverStatus;
	}

	SOCKET& accept_client()
	{
		_clientSocket = accept(_listenSocket, NULL, NULL);

		if (_clientSocket == INVALID_SOCKET) {
			closesocket(_listenSocket);
			WSACleanup();
		}

		return _clientSocket;
	}

	void reset_buffer(char* buffer, int sizeBuffer)
	{
		for (size_t i = 0; i < sizeBuffer; i++)
		{
			buffer[i] = '\0';
		}
	}

	int& check_send(int& result) 
	{
		if(result == SOCKET_ERROR)
		{
			printf("send failed with error: %d\n", WSAGetLastError());
			closesocket(_clientSocket);
			WSACleanup();
		}

		return result;
	}

	int& send_buffer(std::string& sendMessage)
	{
		int sendIndex = 0;
		const int sendBuffer_len{ 500 };
		char sendBuffer[sendBuffer_len];

		reset_buffer(sendBuffer, sendBuffer_len);
		sendBuffer[0] = 'S';
		sendBuffer[1] = 'T';
		sendBuffer[2] = 'A';
		sendBuffer[3] = 'R';
		sendBuffer[4] = 'T';

		iSendResult = send(_clientSocket, sendBuffer, sendBuffer_len, 0);
		if (check_send(iSendResult) == SOCKET_ERROR) return iSendResult;
		printf("Bytes sent -START-: %d\n", iSendResult);

		

		while (sendMessage.length() - 1 > sendIndex)
		{
			reset_buffer(sendBuffer, sendBuffer_len);
			for (int i = sendIndex; i < sendMessage.length(); i++)
			{
				if (i - sendIndex == sendBuffer_len || i == sendMessage.length() -1)
				{
					sendIndex = i;
					break;
				}
				sendBuffer[i - sendIndex] = sendMessage[i];
			}
			iSendResult = send(_clientSocket, sendBuffer, sendBuffer_len, 0);
			if (check_send(iSendResult) == SOCKET_ERROR) return iSendResult;
			printf("Bytes sent: %d\n", iSendResult);
		}

		reset_buffer(sendBuffer, sendBuffer_len);
		sendBuffer[0] = 'E';
		sendBuffer[1] = 'N';
		sendBuffer[2] = 'D';
	
		iSendResult = send(_clientSocket, sendBuffer, sendBuffer_len, 0);
		if (check_send(iSendResult) == SOCKET_ERROR) return iSendResult;
		printf("Bytes sent -END-: %d\n", iSendResult);
		return iSendResult;
	}

	std::vector<std::string> split_commands(std::string& stringCommand)
	{
		std::istringstream ss(stringCommand);
		std::string token;

		std::vector<std::string> commandInput;
		while (std::getline(ss, token, ',')) {
			commandInput.push_back(token);
		}

		return commandInput;
	}

	std::vector<std::string> get_parameters(std::vector<std::string>& commandInput)
	{
		std::vector<std::string> out;

		if(commandInput.size() > 0)
		{
			for(int n1 = 1; n1 < commandInput.size(); n1++)
			{
				out.push_back(commandInput[n1]);
			}
		}

		return out;
	}
	

	void initialize(std::string port)
	{
		if (startup_wsa() != 0) return;
		initialize_hints();
		if (get_address_info(port) != 0) return;
		if (open_server_socket() == INVALID_SOCKET) return;
		if (bind_to_socket() == SOCKET_ERROR) return;
		if (listen_to_socket_port() == SOCKET_ERROR) return;
		if (accept_client() == INVALID_SOCKET) return;
		closesocket(_listenSocket);

		do {

			reset_buffer(recvbuf, recvbuflen); // initialize buffer with \0
			_serverStatus = recv(_clientSocket, recvbuf, recvbuflen, 0);
			if (_serverStatus > 0) {
				printf("Bytes received: %d\n", _serverStatus);
				
				std::vector<std::string> splitCommand = split_commands(std::string(recvbuf));
				std::string out;

				if(splitCommand.size() == 1)
				{
					out = _implementation->run_lua_command(splitCommand[0]);
				}
				else 
				{
					out = _implementation->run_lua_command(splitCommand[0], get_parameters(splitCommand));
				}

				if (send_buffer(out) == SOCKET_ERROR) return;
				
			}
			else if (_serverStatus == 0)
				printf("Connection closing...\n");
			else {
				printf("recv failed with error: %d\n", WSAGetLastError());
				closesocket(_clientSocket);
				WSACleanup();
				return;
			}

		} while (_serverStatus > 0);

		// shutdown the connection since we're done
		_serverStatus = shutdown(_clientSocket, SD_SEND);
		if (_serverStatus == SOCKET_ERROR) {
			printf("shutdown failed with error: %d\n", WSAGetLastError());
			closesocket(_clientSocket);
			WSACleanup();
			return;
		}

		// cleanup
		closesocket(_clientSocket);
		WSACleanup();

	}

};
