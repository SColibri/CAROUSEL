#include "../include/AM_Server.h"

void AM_Server::init()
{
	initialize(_port);
}

int& AM_Server::startup_wsa()
{
	_serverStatus = WSAStartup(MAKEWORD(2, 2), &_wsaData);
	return _serverStatus;
}

void AM_Server::initialize_hints()
{
	ZeroMemory(&_hints, sizeof(_hints));
	_hints.ai_family = AF_INET;
	_hints.ai_socktype = SOCK_STREAM;
	_hints.ai_protocol = IPPROTO_TCP;
	_hints.ai_flags = AI_PASSIVE;
}

int& AM_Server::get_address_info(std::string port)
{
	_serverStatus = getaddrinfo(NULL, port.c_str(), &_hints, &_result);
	if (_serverStatus != 0) WSACleanup();
	return _serverStatus;
}

SOCKET& AM_Server::open_server_socket()
{
	_listenSocket = socket(_result->ai_family, _result->ai_socktype, _result->ai_protocol);

	if (_listenSocket == INVALID_SOCKET) {
		freeaddrinfo(_result);
		WSACleanup();
	}

	return _listenSocket;
}

int& AM_Server::bind_to_socket()
{
	_serverStatus = ::bind(_listenSocket, _result->ai_addr, (int)_result->ai_addrlen);

	freeaddrinfo(_result);
	if (_serverStatus == SOCKET_ERROR) {
		closesocket(_listenSocket);
		WSACleanup();
	}

	return _serverStatus;
}

int& AM_Server::listen_to_socket_port()
{
	_serverStatus = listen(_listenSocket, SOMAXCONN);

	if (_serverStatus == SOCKET_ERROR) {
		closesocket(_listenSocket);
		WSACleanup();
	}

	return _serverStatus;
}

SOCKET& AM_Server::accept_client()
{
	_clientSocket = accept(_listenSocket, NULL, NULL);

	if (_clientSocket == INVALID_SOCKET) {
		closesocket(_listenSocket);
		WSACleanup();
	}

	return _clientSocket;
}

void AM_Server::reset_buffer(char* buffer, int sizeBuffer)
{
	for (size_t i = 0; i < sizeBuffer; i++)
	{
		buffer[i] = '\0';
	}
}

int& AM_Server::check_send(int& result)
{
	if (result == SOCKET_ERROR)
	{
		printf("send failed with error: %d\n", WSAGetLastError());
		closesocket(_clientSocket);
		WSACleanup();
	}

	return result;
}

int& AM_Server::send_buffer(std::string& sendMessage)
{
	size_t sendIndex = 0;
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

	if(sendMessage.length() != 0)
		while (sendMessage.length() - 1 > sendIndex)
		{
			reset_buffer(sendBuffer, sendBuffer_len);
			for (size_t i = sendIndex; i < sendMessage.length(); i++)
			{
				if (i - sendIndex == sendBuffer_len)
				{
					sendIndex = i;
					break;
				}

				sendBuffer[i - sendIndex] = sendMessage[i];

				if (i == sendMessage.length() - 1) sendIndex = i;
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

std::vector<std::string> AM_Server::split_commands(std::string& stringCommand)
{
	std::istringstream ss(stringCommand);
	std::string token;

	std::vector<std::string> commandInput;
	while (std::getline(ss, token, ' ')) {
		commandInput.push_back(token);
	}

	return commandInput;
}

std::vector<std::string> AM_Server::get_parameters(std::vector<std::string>& commandInput)
{
	std::vector<std::string> out;

	if (commandInput.size() > 0)
	{
		for (int n1 = 1; n1 < commandInput.size(); n1++)
		{
			out.push_back(commandInput[n1]);
		}
	}

	return out;
}

void AM_Server::initialize(std::string port)
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

			try
			{
				if (splitCommand.size() == 1)
				{
					out = _implementation->run_lua_command(splitCommand[0]);
				}
				else
				{
					out = _implementation->run_lua_command(splitCommand[0], get_parameters(splitCommand));
				}
			}
			catch (const std::exception& e)
			{
				out = "Error: " + *e.what();
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

