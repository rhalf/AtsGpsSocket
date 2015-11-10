using System;
using System.Net.Sockets;

namespace AtsGps.Meitrack {
    public class MeitractTcpManager : TcpManager {
        public MeitractTcpManager (string ip, int port) : base(ip, port) { }


        public override void threadTcpClientRun (object obj) {
            TcpClient tcpClient = (TcpClient)obj;
            lock (base.listTcpClient) {
                listTcpClient.Add(tcpClient);
            }
            try {
                Byte[] readBytes = new Byte[256];
                String message = null;
                int index;

                NetworkStream stream = tcpClient.GetStream();

                if (stream == null)
                    return;


                byte[] msg = { 0x01 };

                while (stream.CanRead) {
                    //Receive message
                    index = stream.Read(readBytes, 0, readBytes.Length);

                    if (index == 0)
                        return;

                    message = System.Text.Encoding.ASCII.GetString(readBytes, 0, index);
                    //Byte[] data = new Byte[index];
                    //Array.Copy(readBytes, data, index);
                    //string hex = BitConverter.ToString(data);
                    //message = message.Trim();

                    ServerLog serverLog = new ServerLog(message, LogType.SERVER_INCOMING_DATA);
                    base.OnEvent(serverLog);
                    //serverLog = new ServerLog(message, LogType.SERVER_INCOMING_DATA);
                    //base.OnEvent(serverLog);

                    //Send message
                    if (readBytes[0] == 0x08) {

                        byte[] writeBytes = System.Text.Encoding.ASCII.GetBytes(message);
                        stream.Write(writeBytes, 0, writeBytes.Length);
                    }

                }
            } catch (Exception exception) {
                ServerLog serverLog = new ServerLog("TcpClient : " + exception.Message, LogType.SERVER_ERROR);
                base.OnEvent(serverLog);
            } finally {
                lock (listTcpClient) {
                    tcpClient.Close();
                    listTcpClient.Remove(tcpClient);
                }
            }
        }
    }
}
