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

#pragma comment (lib, "Ws2_32.lib")

#define DEFAULT_BUFLEN 512
#define DEFAULT_PORT "27015"

/// <summary>
/// Server class for socket communication. To start communication
/// call the init function and it will wait for a client request
/// under the specified port number.
/// 
/// All avavilable commands are decribed by the api_lua_functions.h and
/// also the base functions described in IAM_lua_functions.h. 
/// </summary>
class AM_Server 
{
private:
	std::string _port{ DEFAULT_PORT }; //socket port number
	
	/// <summary>
	/// api implementation contains the run_lua command which
	/// describes how to run all related commands.
	/// </summary>
	IAM_API* _implementation{ nullptr }; 

public:
	AM_Server(IAM_API* implementation):_implementation(implementation) {}

	AM_Server(IAM_API* implementation, std::string& port) :
		_implementation(implementation), _port(port){}

	/// <summary>
	/// Initialize socket on _port, Default port is 27015
	/// </summary>
	void init();

private:
	WSADATA _wsaData; //wsa initialization data
	SOCKET _listenSocket{ INVALID_SOCKET }; // Server socket handle
	SOCKET _clientSocket{ INVALID_SOCKET }; // client socket handle
	int _serverStatus{-1}; // socket communication status
	int iSendResult; // send result code
	char recvbuf[DEFAULT_BUFLEN]; // buffer used for communication
	int recvbuflen = DEFAULT_BUFLEN; // buffer size default: 512

	addrinfo* _result = NULL;
	addrinfo _hints;
	

	/// <summary>
	/// Step 1 Initialization
	/// Initialize wsa to version 2
	/// </summary>
	/// <returns></returns>
	int& startup_wsa();

	/// <summary>
	/// Step 2 Initialization
	/// WSA initialize communication type
	/// </summary>
	void initialize_hints();

	/// <summary>
	/// Step 3 Initialization
	/// gets information about address
	/// </summary>
	/// <param name="port"></param>
	/// <returns></returns>
	int& get_address_info(std::string port);

	/// <summary>
	/// Step 4 Initialization
	/// Opens server socket and awaits client
	/// </summary>
	/// <returns></returns>
	SOCKET& open_server_socket();

	/// <summary>
	/// Step 5 Initialization
	/// Once the client connects with the server on
	/// the specified port, bind the socket.
	/// </summary>
	/// <returns></returns>
	int& bind_to_socket();

	/// <summary>
	/// Listen to client requests
	/// </summary>
	/// <returns></returns>
	int& listen_to_socket_port();

	/// <summary>
	/// Accept client requests
	/// </summary>
	/// <returns></returns>
	SOCKET& accept_client();

	/// <summary>
	/// Clears buffer content and initializes it to '\0'
	/// </summary>
	/// <param name="buffer"></param>
	/// <param name="sizeBuffer"></param>
	void reset_buffer(char* buffer, int sizeBuffer);

	/// <summary>
	/// Check if data buffer was sent
	/// </summary>
	/// <param name="result"></param>
	/// <returns></returns>
	int& check_send(int& result);

	/// <summary>
	/// Send data in buffer sized chuncks
	/// </summary>
	/// <param name="sendMessage"></param>
	/// <returns></returns>
	int& send_buffer(std::string& sendMessage);

	/// <summary>
	/// Split string commands by space char and returns
	/// a vector of the command on [0] and further parameters
	/// can be obtained [>0]
	/// </summary>
	/// <param name="stringCommand"></param>
	/// <returns></returns>
	std::vector<std::string> split_commands(std::string& stringCommand);

	/// <summary>
	/// From vector generated using split_commands obtains
	/// a vector with only the parameters
	/// </summary>
	/// <param name="commandInput"></param>
	/// <returns></returns>
	std::vector<std::string> get_parameters(std::vector<std::string>& commandInput);
	
	/// <summary>
	/// Main working loop, stops until the communication is broken
	/// </summary>
	/// <param name="port"></param>
	void initialize(std::string port);

};
