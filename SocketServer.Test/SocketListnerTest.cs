using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Net.Sockets;


namespace SocketServer.Test
{
    [TestClass]
    public class SocketListnerTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateWithNullParam()
        {
            SocketListener listener = SocketListener.CreateListener(null);
        }

        [TestMethod]
        public void TestStartListner()
        {
            Moq.Mock<Socket> mock = new Moq.Mock<Socket>(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            SocketListener listener = new SocketListener(mock.Object);
            listener.Start();

            Assert.IsTrue(listener.Started);
            Assert.AreEqual(listener.Server,mock.Object);
            mock.Verify(s => s.AcceptAsync(Moq.It.IsAny<SocketAsyncEventArgs>()));
        }

        [TestMethod]
        public void ConnectionAcceptedTest()
        {
            
        }
    }
}
