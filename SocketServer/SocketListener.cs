using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace SocketServer
{
    /// <summary>
    /// Class responsible for manage the TCP Socket server.
    /// </summary>
    public class SocketListener
    {
        public static SocketListener CreateListener(IPEndPoint endpoint)
        {
            if (endpoint == null)
                throw new ArgumentNullException();

            Socket socket = new Socket(endpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(endpoint);
            socket.Listen(100);

            return new SocketListener(socket);
        }


        /// <summary>
        /// C# object that expose socket functionalities.
        /// </summary>
        public Socket Server { get; private set; }
        public bool Started { get; private set; }
        /// <summary>
        /// Event called when a connection is received;
        /// </summary>
        public event ConnectionReceivedDelegate ConnectionReceived;


        internal SocketListener(Socket socket)
        {
            Server = socket;
        }

        /// <summary>
        /// Initiate the server with IPEndPoint information.
        /// </summary>
        /// <param name="endpoint">Local endpoint what the server will work.</param>
        public void Start()
        {
            StartAccept(null);

            Started = true;
        }

        /// <summary>
        /// Starts the process of receiving connections.
        /// </summary>
        /// <param name="acceptEventArg">Parameter containing the socket information</param>
        private void StartAccept(SocketAsyncEventArgs acceptEventArg)
        {
            if (acceptEventArg == null)
            {
                acceptEventArg = new SocketAsyncEventArgs();
                acceptEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(AcceptEventArg_Completed);
            }
            else
            {
                // socket must be cleared since the context object is being reused
                acceptEventArg.AcceptSocket = null;
            }

            if (!Server.AcceptAsync(acceptEventArg))
            {
                ProcessAccept(acceptEventArg);
            }
        }

        /// <summary>
        /// Callback that is called when a connection is received.
        /// </summary>
        /// <param name="sender">The object the called the callback. In this case, the Serve object.</param>
        /// <param name="e">The parameter containing the connection information.</param>
        private void AcceptEventArg_Completed(object sender, SocketAsyncEventArgs e)
        {
            ProcessAccept(e);
        }

        /// <summary>
        /// Process the receive of a connection.
        /// </summary>
        /// <param name="acceptEventArg">The parameter containing the connection information.</param>
        private void ProcessAccept(SocketAsyncEventArgs acceptEventArg)
        {
            SocketClient client = new SocketClient(acceptEventArg.AcceptSocket);

            ConnectionReceived.Invoke(client);
        }
    }
}
