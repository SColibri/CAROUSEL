#pragma once


#include <iostream>
#include <sys/stat.h>
#include <fcntl.h>
#include <stdio.h>
#include <fstream>
#include <sstream>
#include <string>

#include "../external/sshlib/include/libssh/libssh.h"
#include "../external/sshlib/include/libssh/sftp.h"
#include "../include/callbackFunctions/ErrorCallback.h"

extern "C"
{
#include "../external/sshlib/include/libssh/callbacks.h"
}

void my_log_callback(int priority, const char* function, const char* buffer, void* userdata) {
	std::cerr << "Callback: " << priority << " " << function << buffer << std::endl;
}

namespace AMFramework
{
	namespace Networking
	{
		// bits from: https://docs.oracle.com/cd/E26502_01/html/E29032/chmod-2.html
#define S_IRUSR 00400
#define S_IWUSR 00200
#define S_IXUSR 00100

		/// <summary>
		/// ssh config structure
		/// </summary>
		struct SshHostConfig {
			std::string host{""};
			std::string hostname{""};
			std::string user{""};
			std::string identityFile{ "" };
			int port{22};
		};

		/// <summary>
		/// ssh wrapper for sending files and executing commands
		/// </summary>
		class AM_ssh
		{
		private:
			/// <summary>
			/// Create new ssh session
			/// </summary>
			/// <returns></returns>
			ssh_session CreateSession()
			{
				return ssh_new();
			}

		public:
			/// <summary>
			/// Default constructor
			/// </summary>
			AM_ssh()
			{
				ssh_init();
				SendFile();
			}

			/// <summary>
			/// Send file using ssh
			/// </summary>
			/// <returns></returns>
			int SendFile()
			{
				int rc;
				int verbosity = SSH_LOG_PROTOCOL;

				// create new session
				ssh_session session = CreateSession();
				if (session == NULL) {
					AMFramework::Callback::ErrorCallback::TriggerCallback("Failed to create SSH session");
					std::cerr << "Failed to create SSH session" << std::endl;
					return SSH_ERROR;
				}

				// Get ssh configuration data
				SshHostConfig sConfig;
				readSshConfig("local_ssh", sConfig);

				// Connect through ssh using password
				ssh_options_set(session, SSH_OPTIONS_HOST, sConfig.hostname.c_str());
				ssh_options_set(session, SSH_OPTIONS_USER, sConfig.user.c_str());
				ssh_options_set(session, SSH_OPTIONS_LOG_VERBOSITY, &verbosity);

				// log callback
				ssh_logging_callback lCallback = &my_log_callback;
				rc = ssh_set_log_callback(lCallback);

				// load configuration from config file
				ssh_options_parse_config(session, NULL);
				rc = ssh_connect(session);

				// Import private key and authenticate
				ssh_key sKey = ssh_key_new();
				rc = ssh_pki_import_privkey_file(sConfig.identityFile.c_str(), "______", NULL, NULL, &sKey);

				rc = ssh_userauth_publickey(session, "ssh", sKey);
				std::cerr << "Any errors here? " << ssh_get_error(session) << std::endl;

				//rc = ssh_connect(session);
				if (rc != SSH_OK)
				{
					AMFramework::Callback::ErrorCallback::TriggerCallback("Failed to create SSH session");
					std::cerr << "Failed to create SSH session " << ssh_get_error(session) << std::endl;
					return 1;
				}
				// rc = ssh_connect(session);

				sftp_session sftpSession = sftp_new(session);
				if (sftpSession == NULL)
				{
					AMFramework::Callback::ErrorCallback::TriggerCallback("Failed to create SSH session");
					std::cerr << "Failed to create sftp session " << ssh_get_error(session) << std::endl;
					return 1;
				}

				rc = sftp_init(sftpSession);
				sftp_file file = sftp_open(sftpSession, "~/file.txt", O_WRONLY | O_CREAT | O_TRUNC, S_IRUSR | S_IWUSR);
				const char* data = "Hello, world!";
				size_t nbytes = strlen(data);
				//rc = sftp_write(file, data, nbytes);
				//sftp_file remote_file = sftp_open(sftpSession, "remote_file_path", O_WRONLY | O_CREAT | O_TRUNC, S_IRUSR | S_IWUSR);

				ssh_scp scp;
				scp = ssh_scp_new(session, SSH_SCP_WRITE | SSH_SCP_RECURSIVE, "~");
				if (scp == NULL)
				{
					fprintf(stderr, "Error allocating scp session: %s\n",
						ssh_get_error(session));
					return SSH_ERROR;
				}
				rc = ssh_scp_init(scp);
				// rc = ssh_scp_push_directory(scp, "helloworld", S_IRUSR | S_IWUSR | S_IXUSR);
				if (rc != SSH_OK)
				{
					fprintf(stderr, "Error initializing scp session: %s\n",
						ssh_get_error(session));
					ssh_scp_free(scp);
					return rc;
				}

				int returnScp = scp_helloworld(session, scp);

				ssh_disconnect(session);
				ssh_free(session);
			}

			int scp_helloworld(ssh_session session, ssh_scp scp)
			{
				int rc;
				const char* helloworld = "Hello, world!\n";
				int length = strlen(helloworld);
				rc = ssh_scp_push_directory(scp, "helloworld", S_IRUSR | S_IWUSR | S_IXUSR);
				if (rc != SSH_OK)
				{
					fprintf(stderr, "Can't create remote directory: %s\n",
						ssh_get_error(session));
					return rc;
				}
				rc = ssh_scp_push_file(scp, "helloworld.txt", length, S_IRUSR | S_IWUSR);
				if (rc != SSH_OK)
				{
					fprintf(stderr, "Can't open remote file: %s\n",
						ssh_get_error(session));
					return rc;
				}
				rc = ssh_scp_write(scp, helloworld, length);
				if (rc != SSH_OK)
				{
					fprintf(stderr, "Can't write to remote file: %s\n",
						ssh_get_error(session));
					return rc;
				}
				return SSH_OK;
			}

			/// <summary>
			/// Reads the .ssh config file
			/// </summary>
			/// <param name="hostName"></param>
			/// <param name="config"></param>
			void readSshConfig(const std::string& hostName, SshHostConfig& config) {
				std::string homeDir = getenv("USERPROFILE");
				std::string configFile = homeDir + "/.ssh/config";

				std::ifstream sshConfig(configFile);
				std::string line;
				std::string currentHost;
				bool foundHost = false;

				while (std::getline(sshConfig, line)) {
					// Remove leading and trailing whitespaces from the line
					line.erase(0, line.find_first_not_of(" \t"));
					line.erase(line.find_last_not_of(" \t") + 1);

					if (line.empty() || line[0] == '#') {
						// Skip empty or commented lines
						continue;
					}

					if (!foundHost && line.substr(0, 4) == "Host") {
						// A new host block has started
						currentHost = line.substr(5);
						foundHost = currentHost == hostName;
					}
					else if (foundHost && line.substr(0, 8) == "HostName") {
						// Set the hostname for the target host
						config.hostname = line.substr(9);
					}
					else if (foundHost && line.substr(0, 4) == "User") {
						// Set the username for the target host
						config.user = line.substr(5);
					}
					else if (foundHost && line.substr(0, 4) == "Port") {
						// Set the port number for the target host
						std::istringstream iss(line.substr(5));
						iss >> config.port;
					}
					else if (foundHost && line.substr(0, 12) == "IdentityFile") {
						// Set the username for the target host
						config.identityFile = line.substr(13);
						size_t pos = config.identityFile.find("~");
						if (pos != std::string::npos) {
							config.identityFile.replace(pos, 1, homeDir);
						}
					}

					if (foundHost && 
						!config.hostname.empty() && 
						!config.user.empty() && 
						!config.identityFile.empty()) {
						// All required fields have been populated, so we can stop reading the file
						break;
					}
				}

				config.host = currentHost;
			}

			void SendCommand() 
			{
			
			}

		};
	}
}