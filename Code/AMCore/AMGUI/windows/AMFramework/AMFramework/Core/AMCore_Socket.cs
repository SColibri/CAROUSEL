using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace AMFramework.Core
{
    public class AMCore_Socket : IAMCore_Comm
    {
        private Socket s = new(AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp);
        private bool _connected = false;
        public bool connected { get { return _connected; } }
        public void init() 
        {
            s.ReceiveTimeout = 10000;
            connect_to_server("127.0.0.1", 27015);
        }
        private void connect_to_server(string host, int port) 
        {
            try 
            {
                IPAddress[] IPs = Dns.GetHostAddresses(host);

                Console.WriteLine("Establishing Connection to {0}", host);
                s.Connect(IPs[0], port);
                Console.WriteLine("Connection established");
                _connected = true;
            }
            catch(Exception e)
            { 
                Console.WriteLine(e.Message);
            }
            
        }

        public string send_receive(string sendMessage) 
        {
            string strBuild = "";

            try 
            {
                if (!_connected) { init(); }
                byte[] msg = Encoding.UTF8.GetBytes(sendMessage);
                byte[] bytes = new byte[2048];
                int byteCount = s.Send(msg, 0, msg.Length, SocketFlags.None);
                Console.WriteLine("Sent {0} bytes.", byteCount);

                byteCount = s.Receive(bytes, 0, bytes.Length, SocketFlags.None);
                string tempBuild = System.Text.ASCIIEncoding.ASCII.GetString(bytes, 0, byteCount).Trim('\0');
                if (tempBuild.CompareTo("START") == 0)
                {
                    while (byteCount > 0)
                    {
                        SocketError sE;
                        byteCount = s.Receive(bytes, 0, bytes.Length, 0, out sE);
                        tempBuild = System.Text.ASCIIEncoding.ASCII.GetString(bytes, 0, byteCount);

                        if (sE != SocketError.Success) break;
                        if (tempBuild.CompareTo("END") != 0)
                        {
                            strBuild = strBuild + tempBuild.Trim('\0');
                        }
                        else { break; }

                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                s.Close();
                
                s = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp);
                _connected = false;

                strBuild = "Error: " + e.Message;
            }


            return strBuild;
        }

        public void dispose() 
        { 
        
        }

        #region Interface
        public string run_lua_command(string command, string parameters)
        {
            return send_receive(command + parameters);
        }

        public void update_path(string apiPath)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
