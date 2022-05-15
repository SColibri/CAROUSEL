#pragma once

#include <windows.h>
#include <winsock2.h>
#include <ws2tcpip.h>
#include <stdlib.h>
#include <stdio.h>

#pragma comment (lib, "Ws2_32.lib")
#pragma comment (lib, "Mswsock.lib")
#pragma comment (lib, "AdvApi32.lib")

class AM_Client
{

public:
	void connect()
	{
	
	}



private:
	WSADATA _wsaData;
	SOCKET _connectSocket{ INVALID_SOCKET };


};
