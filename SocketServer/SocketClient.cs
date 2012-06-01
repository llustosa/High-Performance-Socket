using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace SocketServer
{
    /// <summary>
    /// Class that offers a access interface with the socket client.
    /// </summary>
    public class SocketClient
    {
        internal SocketClient(Socket socket)
        {
            ClientSocket = socket;
        }

        /// <summary>
        /// Socket object that represents the connection with the client;
        /// </summary>
        public Socket ClientSocket { get; private set; }
    }
}
