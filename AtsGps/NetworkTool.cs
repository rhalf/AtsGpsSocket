using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace AtsGps {
    public class NetworkTool {

        /// <summary>
        /// Checks the connection state
        /// </summary>
        /// <returns>True on connected. False on disconnected.</returns>
        public static bool IsConnected (TcpClient tcpClient) {
            if (tcpClient.Connected) {
                if ((tcpClient.Client.Poll(0, SelectMode.SelectWrite)) && (!tcpClient.Client.Poll(0, SelectMode.SelectError))) {
                    byte[] buffer = new byte[1];
                    if (tcpClient.Client.Receive(buffer, SocketFlags.Peek) == 0) {
                        return false;
                    } else {
                        return true;
                    }
                } else {
                    return false;
                }
            } else {
                return false;
            }
        }

    }
}
